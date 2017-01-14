using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Web;

namespace PersonalAssistant
{
    public class IOHelper
    {
        private const int defaultNumFileRetries = 3;    // Number of times to retry a file IO operation
        private const int fileRetryDelay = 1000;        // Delay in milliseconds between retries

        /// <summary>
        /// Loads a serialized Dictionary from a file. Should be used in conjunction with saveDictionary.
        /// </summary>
        /// <typeparam name="T">Type of the serializable object to load</typeparam>
        /// <param name="filePath">Path of file to load from</param>
        /// <param name="serializable">Loaded serializable object</param>
        /// <param name="numFileRetries">How many times to retry the file read operation</param>
        /// <param name="fileRetryDelay">How long, in milliseconds, to wait between file read operation retries</param>
        /// <returns>True if the load was successful, otherwise false</returns>
        /// <exception cref="SerializationException">The file couldn't be deserialized. Probably a formatting issue</exception>
        public static bool loadSerializable<T>(
            string filePath, out T serializable, int numFileRetries = defaultNumFileRetries, int fileRetryDelay = fileRetryDelay)
        {
            serializable = default(T);
            if (File.Exists(filePath))      // only attempt to load if the file exists. Note, that this state could change if multiple threads access the same file
            {
                for (int fileAccessIteration = 1; fileAccessIteration <= numFileRetries; fileAccessIteration++) // retry if there is an IOException
                {
                    try
                    {
                        using (Stream settingsFileStream = File.OpenRead(filePath))
                        {
                            BinaryFormatter deserializer = new BinaryFormatter();   // Must have the same format as the save operation
                            serializable = (T)deserializer.Deserialize(settingsFileStream);  // load the dictionary from file
                        }
                        return true;    // no exceptions. File loaded successfully.
                    }
                    catch (IOException)
                    {
                        Thread.Sleep(fileRetryDelay);   // wait before retrying, so that an intermittant problem may resolve.
                        continue;   // try again
                    }
                }
            }
            return false;   // Retries exhausted. Couldn't load file.
        }

        /// <summary>
        /// Saves a dictionary to a file. Should be used in conjunction wth loadDictionary.
        /// </summary>
        /// <typeparam name="T">Type of serializable to save</typeparam>
        /// <param name="filePath">File to save to. Will be overwritten if it already exists.</param>
        /// <param name="serializable">Serializable to save</param>
        /// <param name="overwrite">Pass in true to override a file that is already there.</param>
        /// <param name="numFileRetries">How many times to retry the file read operation</param>
        /// <param name="fileRetryDelay">How long, in milliseconds, to wait between file read operation retries</param>
        /// <returns>True if the save operation was successful. Otherwise, false.</returns>
        public static bool saveSerializable<T>(string filePath, T serializable, bool overwrite = true, int numFileRetries = 3, int fileRetryDelay = 1000)
        {
            if (!overwrite && File.Exists(filePath))
            {
                return false;   // if the file already exists, and we don't want to overwrite it.
            }
            for (int fileAccessIteration = 1; fileAccessIteration <= numFileRetries; fileAccessIteration++)     // Try the IO operations a couple of times to resolve intermittant issues
            {
                try
                {
                    using (Stream settingsFileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        BinaryFormatter serializer = new BinaryFormatter();     // format needs to match with the format used to load
                        serializer.Serialize(settingsFileStream, serializable);   // perform the actual save operation
                    }
                    return true;    // no exceptions. Save successful.
                }
                catch (IOException)
                {
                    Thread.Sleep(fileRetryDelay);   // wait a bit before retrying.
                    continue;   // try again
                }
            }
            return false;   // retries exhausted. Couldn't save dictionary.
        }
    }
}
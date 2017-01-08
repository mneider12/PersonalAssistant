using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

public static class DictionaryIOHelper
{
    private const int defaultNumFileRetries = 3;    // Number of times to retry a file IO operation
    private const int fileRetryDelay = 1000;        // Delay in milliseconds between retries

    /// <summary>
    /// Loads a serialized Dictionary from a file. Should be used in conjunction with saveDictionary.
    /// </summary>
    /// <param name="filePath">Path of file to load from</param>
    /// <param name="dictionary">Loaded dictionary, or a new Dictionary if the load fails</param>
    /// <param name="numFileRetries">How many times to retry the file read operation</param>
    /// <param name="fileRetryDelay">How long, in milliseconds, to wait between file read operation retries</param>
    /// <returns>True if the load was successful, otherwise false</returns>
    /// <exception cref="SerializationException">The file couldn't be deserialized. Probably a formatting issue</exception>
	public static bool loadDictionary(
        string filePath, out Dictionary<string, string> dictionary, int numFileRetries = 3, int fileRetryDelay = 1000)
    {
        dictionary = new Dictionary<string, string>();  // set dictionary to a new Dictionary, in case the file load fails.
        if (File.Exists(filePath))      // only attempt to load if the file exists. Note, that this state could change if multiple threads access the same file
        {
            for (int fileAccessIteration = 1; fileAccessIteration <= numFileRetries; fileAccessIteration++) // retry if there is an IOException
            {
                try
                {
                    using (Stream settingsFileStream = File.OpenRead(filePath))
                    {
                        BinaryFormatter deserializer = new BinaryFormatter();   // Must have the same format as the save operation
                        dictionary = (Dictionary<string, string>)deserializer.Deserialize(settingsFileStream);  // load the dictionary from file
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
    /// <param name="filePath">File to save to. Will be overwritten if it already exists.</param>
    /// <param name="dictionary">Dictionary to save</param>
    /// <param name="numFileRetries">How many times to retry the file read operation</param>
    /// <param name="fileRetryDelay">How long, in milliseconds, to wait between file read operation retries</param>
    /// <returns>True if the save operation was successful. Otherwise, false.</returns>
    public static bool saveDictionary(string filePath, Dictionary<string, string> dictionary, int numFileRetries = 3, int fileRetryDelay = 1000)
    {
        for (int fileAccessIteration = 1; fileAccessIteration <= numFileRetries; fileAccessIteration++)     // Try the IO operations a couple of times to resolve intermittant issues
        {
            try
            {
                using (Stream settingsFileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    BinaryFormatter serializer = new BinaryFormatter();     // format needs to match with the format used to load
                    serializer.Serialize(settingsFileStream, dictionary);   // perform the actual save operation
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

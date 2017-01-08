using System;

public static class DictionaryIOHelper
{
	public bool loadDictionary(
        string filePath, out Dictionary<string, string> dictionary, int numFileRetries = 3, int fileRetryDelay = 1000)
    {
        if (File.Exists(filePath))
        {
            for (int fileAccessIteration = 1; fileAccessIteration <= numFileRetries; fileAccessIteration++)
            {
                try
                {
                    using (Stream settingsFileStream = File.OpenRead(settingsFilePath))
                    {
                        BinaryFormatter deserializer = new BinaryFormatter();
                        dictionary = (Dictionary<string, string>)deserializer.Deserialize(settingsFileStream);
                    }
                    return true;
                }
                catch (IOException)
                {
                    Thread.Sleep(fileRetryDelay);
                    continue;
                }
            }
        }
        else
        {
            dictionary = new Dictionary<string, string>();
            return false;
        }
    }
}

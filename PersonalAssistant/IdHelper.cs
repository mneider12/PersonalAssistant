using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static PersonalAssistant.IOHelper;

namespace PersonalAssistant
{
    public static class IdHelper
    {
        private static Dictionary<string, int> idSettings;
        private const string idSettingsFileNameRelative = "~/data/idSettings.ser";
        private static string idSettingsFilePathMapped;
        private static FileLoadResult idSettingsFileLoaded;

        static IdHelper()
        {
            idSettingsFilePathMapped = HttpContext.Current.Server.MapPath(idSettingsFileNameRelative);
            loadSettings();
        }

        public static int nextId(string recordType)
        {
            if (idSettings.ContainsKey(recordType))
            {
                return incrementIdCounter(recordType, idSettings[recordType]);
            }
            else
            {
                initializeIdCounter(recordType);
                return incrementIdCounter(recordType, idSettings[recordType]);
            }
        }

        private static int incrementIdCounter(string recordType, int currentId)
        {
            int newId = currentId + 1;
            idSettings[recordType] = newId;
            IOHelper.saveSerializable<Dictionary<string, int>>(idSettingsFilePathMapped, idSettings);
            return newId;
        }

        private static void initializeIdCounter(string recordType)
        {
            idSettings[recordType] = getDefaults()[recordType];
        }

        private static Dictionary<string, int> getDefaults()
        {
            Dictionary<string, int> defaults = new Dictionary<string, int>();
            defaults["order"] = 1000;
            return defaults;
        }

        private static void loadSettings()
        {
            idSettingsFileLoaded = IOHelper.loadSerializable<Dictionary<string, int>>(idSettingsFilePathMapped, out idSettings);
            switch (idSettingsFileLoaded)
            {
                case FileLoadResult.FileAccessFailed:   // settings file exists and we couldn't load it. That's bad, and not safe to allocate any new ids.
                    throw new FileAccessException("Failed to read id settings file. Continuing could cause data corruption");
                case FileLoadResult.FileDoesNotExist:   // settings file doesn't exist yet. We can create it.
                    idSettings = new Dictionary<string, int>();
                    break;
            }

        }
    }
}
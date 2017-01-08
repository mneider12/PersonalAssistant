using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PersonalAssistant
{
    public partial class Configuration : System.Web.UI.Page
    {
        private const string settingsFileName = "~/Data/settings.ser";
        private const int numFileRetries = 3;
        private const int fileRetryDelay = 1000;
        private string settingsFilePath;
        private Dictionary<string, string> Settings { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            loadSettings();
            populateSettingsForm();

            btnSave.ServerClick += new EventHandler(btnSave_Click);
        }

        public void btnSave_Click(object sender, EventArgs e)
        {
            string name = txtName.Value;
            if (name.Length > 0 && name.Length < 70)
            {
                Settings["name"] = name;
            }
            int id;
            if (Int32.TryParse(numId.Value, out id) && id < 100000000)
            {
                Settings["id"] = numId.Value;
            }

            using (Stream settingsFileStream = new FileStream(settingsFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                BinaryFormatter serializer = new BinaryFormatter();
                serializer.Serialize(settingsFileStream, Settings);
            }
        }

        private void loadSettings()
        {
            settingsFilePath = Server.MapPath(settingsFileName);
            if (File.Exists(settingsFilePath))
            {
                for (int fileAccessIteration = 1; fileAccessIteration <= numFileRetries; fileAccessIteration++)
                {
                    try
                    {
                        using (Stream settingsFileStream = File.OpenRead(settingsFilePath))
                        {
                            BinaryFormatter deserializer = new BinaryFormatter();
                            Settings = (Dictionary<string, string>)deserializer.Deserialize(settingsFileStream);
                        }
                        break;  // load successful
                    }
                    catch (IOException)
                    {
                        continue;
                    }
                }
            }
            else
            {
                Settings = new Dictionary<string, string>();
            }
        }
        public void populateSettingsForm()
        {
            txtName.Value = Settings["name"];
            numId.Value = Settings["id"];
        }
    }
}
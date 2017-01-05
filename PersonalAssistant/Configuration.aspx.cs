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
        private string settingsFilePath;
        private Dictionary<string, string> Settings { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            settingsFilePath = Server.MapPath(settingsFileName);
            if (File.Exists(settingsFilePath))
            {
                Stream settingsFileStream = File.OpenRead(settingsFilePath);
                BinaryFormatter deserializer = new BinaryFormatter();
                Settings = (Dictionary<string, string>)deserializer.Deserialize(settingsFileStream);
            }
            else
            {
                Settings = new Dictionary<string, string>();
            }

            btnSave.ServerClick += new EventHandler(btnSave_Click);
        }

        public void btnSave_Click(object sender, EventArgs e)
        {

        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.SQLite;
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
        private const string settingsFileName = "~/Data/settings.ser";  // loaction of the file storing the serialized dictionary of settings. Will be created if it doesn't exist.
        private const int numFileRetries = 3;   // how many times to retry file access operations before giving up
        private const int fileRetryDelay = 1000;    // how long, in milliseconds to wait between retries for file access operations
        private string settingsFilePath;    // file name mapped to a location of the server
        /// <summary>
        /// Settings
        /// name : current user name
        /// name_maxLength : maximum allowed length for names
        /// id : current user id
        /// id_maxValue : maximum user id value
        /// </summary>
        private Dictionary<string, string> settings;

        /// <summary>
        /// Runs when the page loads
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            settingsFilePath = Server.MapPath(settingsFileName);    // Set the global value for the settings file path
            IOHelper.loadSerializable<Dictionary<string, string>>(settingsFilePath, out settings);    // load the dictionary
            loadDefaultSettings();
            if (!IsPostBack)    // don't repopulate values if its a post back
            {
                populateSettingsForm();     // fill in fields with current settings
            }
        }

        /// <summary>
        /// Save settings including user entered updates
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnSave_Click(object sender, EventArgs e)
        {
            string name = txtName.Value;    // grab the value from the name field on the page
            if (name.Length > 0 && name.Length < Int32.Parse(settings["name_maxLength"]))   // validate the user_name field
            {
                settings["name"] = name;
            }
            int id;
            if (Int32.TryParse(numId.Value, out id) && id <= Int32.Parse(settings["id_maxValue"]))  // validate the id
            {
                settings["id"] = numId.Value;
            }

            IOHelper.saveSerializable<Dictionary<string, string>>(settingsFilePath, settings);  // save all the settings
        }

        /// <summary>
        /// Populate settings page with existing values
        /// </summary>
        public void populateSettingsForm()
        {
            txtName.Value = settings["name"];
            numId.Value = settings["id"];
        }

        /// <summary>
        /// Load default settings. Only sets a default setting, if nothing else is already set.
        /// </summary>
        public void loadDefaultSettings()
        {
            if (!settings.ContainsKey("name_maxLength"))
            {
                settings["name_maxLength"] = "70";  // random article said british gov't recommends 35 per name, or 70 total
            }
            if (!settings.ContainsKey("id_maxValue"))
            {
                settings["id_maxValue"] = "99999999";   // 8 digit ids
            }
        }

        public void btnReinstall_Click(object sender, EventArgs e)
        {
            SQLiteConnection.CreateFile(dbHelper.DbPath);   // create the database

            using (SQLiteConnection dbConnection = dbHelper.getDbConnection())
            {
                dbConnection.Open();
                createOrdersTable(dbConnection);
            }
        }

        public void createOrdersTable(SQLiteConnection dbConnection)
        {
            string createOrdersTableString = "CREATE TABLE ORDERS (" +
                                    "ID INT PRIMARY KEY NOT NULL, " +
                                    "DATE REAL, " +
                                    "TICKER TEXT NOT NULL, " +
                                    "SHARES INT NOT NULL, " +
                                    "PRICE REAL);";
            using (SQLiteCommand createOrdersTable = new SQLiteCommand(createOrdersTableString, dbConnection))
            {
                createOrdersTable.ExecuteNonQuery();
            }
        }
    }
}
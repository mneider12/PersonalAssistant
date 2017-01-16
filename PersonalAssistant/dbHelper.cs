using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Web;

namespace PersonalAssistant
{
    public static class dbHelper
    {
        public static string DbPath;
        static dbHelper()
        {
            DbPath = HttpContext.Current.Server.MapPath("~/data/database.db");
        }

        public static SQLiteConnection getDbConnection()
        {
            return new SQLiteConnection(String.Format("Data Source={0}", DbPath));
        }
    }
}
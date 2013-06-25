using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Collections;

namespace WebServDemo
{
    class Helper
    {
        private const string SETTING_NAME_PORT = "Port";
        private const string SETTING_NAME_XMLFILE = "XmlFile";
        private const string SETTING_NAME_SQLITE = "Sqlite";

        private const uint SETTING_NAME_PORT_VALUE = 8080;
        private const uint SETTING_NAME_XMLFILE_VALUE = 0;
        private const uint SETTING_NAME_SQLITE_VALUE = 1;

        private static string _GetValueByKey(string _Key, string _ValueDefault = "0")
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            KeyValueConfigurationElement setting = config.AppSettings.Settings[_Key];

            if (setting == null)
            {
                config.AppSettings.Settings.Add(_Key, _ValueDefault);
                config.Save();
                setting = config.AppSettings.Settings[_Key];
            }
            return setting.Value;
        }

        public static uint GetSettingPort()
        {
            return Convert.ToUInt16(_GetValueByKey(SETTING_NAME_PORT, SETTING_NAME_PORT_VALUE.ToString()));
        }

        public static uint GetSettingXmlFile()
        {
            return Convert.ToUInt16(_GetValueByKey(SETTING_NAME_XMLFILE, SETTING_NAME_XMLFILE_VALUE.ToString()));
        }

        public static uint GetSettingSqlite()
        {
            return Convert.ToUInt16(_GetValueByKey(SETTING_NAME_SQLITE, SETTING_NAME_SQLITE_VALUE.ToString()));
        }
        public static string GetPath()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            return config.FilePath;
        }
    }
}

using System;
using System.Configuration;

namespace Seznam.User
{
    public class AppConfig : IConfig
    {
        public void LoadConfig()
        {
            var settings = ConfigurationManager.AppSettings;
            Host = settings["Host"];
            Port = Convert.ToInt32(settings["Port"]);
        }

        public int Port { get; set; }
        public string Host { get; set; }
    }
}
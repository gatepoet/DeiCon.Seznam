namespace Seznam.Data
{
    public class Config
    {
        private static IConfig _instance;
        public static IConfig Current { get { return _instance ?? (_instance = CreateAppConfig()); } }

        private static AppConfig CreateAppConfig()
        {
            var config =  new AppConfig();
            config.LoadConfig();
            return config;
        }

        public static void SetCurrent(IConfig config)
        {
            _instance = config;
        }

    }
}
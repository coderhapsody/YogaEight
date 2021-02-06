using System;
using System.Linq;
using GoGym.Data;

namespace GoGym.Providers
{
    public class ConfigurationSingletonProvider
    {
        private static volatile ConfigurationSingletonProvider instance;
        private static object syncRoot = new Object();

        private ConfigurationSingletonProvider() { }
        private string userName;

        public void SetUserName(string userName)
        {
            this.userName = userName;
        }

        public static ConfigurationSingletonProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ConfigurationSingletonProvider();
                    }
                }

                return instance;
            }
        }

        public string GetValue(string key)
        {
            string result;
            using(var ctx = new FitnessEntities())
            {
                var config = ctx.Configurations.SingleOrDefault(configuration => configuration.Key == key);
                result = config == null ? String.Empty : config.Value;
            }
            return result;
        }

        public T GetValue<T>(string key)
        {
            string result = GetValue(key);
            return (T)Convert.ChangeType(result, typeof(T));
        }

        public void SetValue(string key, string value)
        {
            using(var ctx = new FitnessEntities())
            {
                var configuration = ctx.Configurations.SingleOrDefault(config => config.Key == key);
                if (configuration != null)
                {
                    configuration.Key = key;
                    configuration.Value = value;
                    configuration.ChangedWhen = DateTime.Now;
                    configuration.ChangedWho = userName;
                    ctx.SaveChanges();    
                }                
            }
        }

        public void SetValue<T>(string key, T value)
        {
            SetValue(key, Convert.ToString(value));
        }

        public string this[string key]
        {
            get
            {
                return GetValue(key);
            }
            set
            {
                if (GetValue(key) != value)
                {
                    SetValue(key, value);
                }
            }
        }

    }
}
namespace Dynamic.Translator.Core.Config
{
    #region using

    using System;
    using System.Collections.Generic;
    using Extensions;

    #endregion

    public class DictionayBasedConfig : IDictionaryBasedConfig
    {
        protected DictionayBasedConfig()
        {
            this.CustomSettings = new Dictionary<string, object>();
        }

        public object this[string name]
        {
            get { return this.CustomSettings.GetOrDefault(name); }
            set { this.CustomSettings[name] = value; }
        }

        protected Dictionary<string, object> CustomSettings { get; }

        public T Get<T>(string name)
        {
            var value = this[name];
            return value == null
                ? default(T)
                : (T) Convert.ChangeType(value, typeof (T));
        }

        public void Set<T>(string name, T value)
        {
            this[name] = value;
        }

        public object Get(string name)
        {
            return this.Get(name, null);
        }

        public object Get(string name, object defaultValue)
        {
            var value = this[name];
            if (value == null)
                return defaultValue;

            return this[name];
        }

        public T Get<T>(string name, T defaultValue)
        {
            return (T) this.Get(name, (object) defaultValue);
        }

        public T GetOrCreate<T>(string name, Func<T> creator)
        {
            var value = this.Get(name);
            if (value == null)
            {
                value = creator();
                this.Set(name, value);
            }
            return (T) value;
        }
    }
}
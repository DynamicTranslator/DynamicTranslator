using System;

namespace DynamicTranslator.Config
{
    public interface IDictionaryBasedConfig
    {
        object Get(string name);

        T Get<T>(string name);

        object Get(string name, object defaultValue);

        T Get<T>(string name, T defaultValue);

        T GetOrCreate<T>(string name, Func<T> creator);

        void Set<T>(string name, T value);

        void SetAndPersistConfigurationManager(string name, string value);

        void SetViaConfigurationManager(string name);
    }
}
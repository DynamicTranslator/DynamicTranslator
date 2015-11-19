namespace DynamicTranslator.Core.Config
{
    #region using

    using System;

    #endregion

    #region using

    #endregion

    public interface IDictionaryBasedConfig
    {
        void Set<T>(string name, T value);

        void SetViaConfigurationManager(string name);
        void SetAndPersistConfigurationManager(string name, string value);

        object Get(string name);

        T Get<T>(string name);

        object Get(string name, object defaultValue);

        T Get<T>(string name, T defaultValue);

        T GetOrCreate<T>(string name, Func<T> creator);
    }
}
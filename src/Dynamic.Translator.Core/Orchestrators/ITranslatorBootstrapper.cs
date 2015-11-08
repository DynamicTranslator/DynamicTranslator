namespace Dynamic.Translator.Core.Orchestrators
{
    using System;

    public interface ITranslatorBootstrapper : IDisposable, IEvents
    {
        bool IsInitialized { get; }

        void Initialize();
    }
}
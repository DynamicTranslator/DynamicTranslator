namespace DynamicTranslator.Core.Orchestrators
{
    #region using

    using System;

    #endregion

    public interface ITranslatorBootstrapper : IDisposable, IEvents
    {
        bool IsInitialized { get; }

        void Initialize();
    }
}
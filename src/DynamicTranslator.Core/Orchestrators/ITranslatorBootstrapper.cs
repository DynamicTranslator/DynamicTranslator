namespace DynamicTranslator.Core.Orchestrators
{
    #region using

    using System;
    using System.Threading.Tasks;

    #endregion

    public interface ITranslatorBootstrapper : IDisposable, IEvents
    {
        bool IsInitialized { get; }

        void Initialize();

        Task InitializeAsync();

       
        void SubscribeShutdownEvents();
    }
}
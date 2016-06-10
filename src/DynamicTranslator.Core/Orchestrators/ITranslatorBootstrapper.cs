using System;
using System.Threading.Tasks;

using DynamicTranslator.Core.Orchestrators.Event;

namespace DynamicTranslator.Core.Orchestrators
{
    #region using

    

    #endregion

    public interface ITranslatorBootstrapper : IDisposable, IEvents
    {
        bool IsInitialized { get; }

        void Initialize();

        Task InitializeAsync();

        void SubscribeShutdownEvents();
    }
}
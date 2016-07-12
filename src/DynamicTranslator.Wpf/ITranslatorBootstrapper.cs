using System;
using System.Threading.Tasks;

using DynamicTranslator.Domain.Events;

namespace DynamicTranslator.Wpf
{
    public interface ITranslatorBootstrapper : IDisposable, IDynamicTranslatorEvent
    {
        bool IsInitialized { get; }

        void Initialize();

        Task InitializeAsync();

        void SubscribeShutdownEvents();
    }
}
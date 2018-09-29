using System;
using System.Threading.Tasks;

using DynamicTranslator.Application.Events;

namespace DynamicTranslator.Wpf
{
    public interface ITranslatorBootstrapper : IDisposable
    {
        bool IsInitialized { get; }

        Task InitializeAsync();

        void SubscribeShutdownEvents();
    }
}
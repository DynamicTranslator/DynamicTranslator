namespace Dynamic.Translator.Driver
{
    using System;
    using System.Linq;
    using Castle.MicroKernel;
    using Castle.MicroKernel.Facilities;
    using Castle.MicroKernel.Registration;

    public class EventHandlerConvention : AbstractFacility
    {
        protected override void Init()
        {
            this.Kernel.HandlerRegistered += this.OnHandlerRegistered;
        }

        private void OnHandlerRegistered(IHandler handler, ref bool stateChanged)
        {
            var messageTypes = from t in handler.ComponentModel.Services
                               where t.IsInterface
                                     && t.IsGenericType
                                     && t.GetGenericTypeDefinition() == typeof(IEventHandler<>)
                               select t.GetGenericArguments().Single();

            foreach (var t in messageTypes)
            {
                this.Kernel.Register(Component
                                         .For<IObserver<object>>()
                                         .ImplementedBy(typeof(EventDispatcher<>).MakeGenericType(t)));
            }
        }
    }
}
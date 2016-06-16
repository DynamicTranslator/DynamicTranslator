using System.Linq;

using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Facilities;

using DynamicTranslator.Application.Orchestrators;
using DynamicTranslator.Dependency.Interceptors;
using DynamicTranslator.Wpf.Orchestrators.Detector;
using DynamicTranslator.Wpf.Orchestrators.Finders;
using DynamicTranslator.Wpf.Orchestrators.Organizers;

namespace DynamicTranslator.Wpf
{
    public class InterceptorFacility : AbstractFacility
    {
        protected override void Init()
        {
            Kernel.ComponentRegistered += KernelOnComponentRegistered;
        }

        private static void ApplyForDetector(IHandler handler)
        {
            var isDetector = handler.ComponentModel.Implementation.GetInterfaces().Contains(typeof(ILanguageDetector));
            if (isDetector)
            {
                handler.ComponentModel.Interceptors.AddFirst(new InterceptorReference(typeof(ExceptionInterceptor)));
            }
        }

        private static void ApplyForMeanFinder(IHandler handler)
        {
            var isMeanFinder = handler.ComponentModel.Implementation.GetInterfaces().Contains(typeof(IMeanFinder));
            if (isMeanFinder)
            {
                handler.ComponentModel.Interceptors.AddFirst(new InterceptorReference(typeof(ExceptionInterceptor)));
                handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof(TextGuardInterceptor)));
            }
        }

        private static void ApplyForMeanOrganizer(IHandler handler)
        {
            var isMeanOrganizer = handler.ComponentModel.Implementation.GetInterfaces().Contains(typeof(IMeanOrganizer));
            if (isMeanOrganizer)
            {
                handler.ComponentModel.Interceptors.AddFirst(new InterceptorReference(typeof(ExceptionInterceptor)));
            }
        }

        private void KernelOnComponentRegistered(string key, IHandler handler)
        {
            ApplyForMeanFinder(handler);
            ApplyForMeanOrganizer(handler);
            ApplyForDetector(handler);
        }
    }
}
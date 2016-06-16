using System;
using System.Windows;

using Abp;
using Abp.Dependency;
using Abp.Runtime.Caching.Configuration;

using Castle.Facilities.Logging;

using DynamicTranslator.Application;
using DynamicTranslator.Configuration;
using DynamicTranslator.Wpf.ViewModel;

namespace DynamicTranslator.Wpf
{
    public partial class App
    {
        private readonly AbpBootstrapper bootstrapper;

        public App()
        {
            bootstrapper = new AbpBootstrapper();
            bootstrapper.IocManager.IocContainer.AddFacility<LoggingFacility>(f => f.UseNLog());
        }

        protected override void OnStartup(StartupEventArgs eventArgs)
        {
            bootstrapper.Initialize();

            IocManager.Instance.Register<IGrowlNotifications, GrowlNotifiactions>();

            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                var exp = args.ExceptionObject;
            };

            var defaultSlidingExpireTime = TimeSpan.FromHours(24);
            IocManager.Instance.Resolve<ICachingConfiguration>().ConfigureAll(cache => { cache.DefaultSlidingExpireTime = defaultSlidingExpireTime; });

            var configurations = IocManager.Instance.Resolve<IDynamicTranslatorStartupConfiguration>();
            configurations.Initialize();
            base.OnStartup(eventArgs);
        }
    }
}
using System;
using System.Windows;

using Abp;
using Abp.Dependency;
using Abp.Extensions;
using Abp.Runtime.Caching.Configuration;

using Castle.Core.Logging;
using Castle.Facilities.Logging;

using DynamicTranslator.Configuration.Startup;
using DynamicTranslator.Service.GoogleAnalytics;
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

            bootstrapper.IocManager.Register<IGrowlNotifications, GrowlNotifications>();

            HandleExceptionsOrNothing();

            ConfigureMemoryCache();

            base.OnStartup(eventArgs);
        }

        private void ConfigureMemoryCache()
        {
            var defaultSlidingExpireTime = TimeSpan.FromHours(24);
            bootstrapper.IocManager.Resolve<ICachingConfiguration>().ConfigureAll(cache => { cache.DefaultSlidingExpireTime = defaultSlidingExpireTime; });
        }

        private void HandleExceptionsOrNothing()
        {
            using (var applicationConfiguration = bootstrapper.IocManager.ResolveAsDisposable<IApplicationConfiguration>())
            {
                var isExtraLoggingEnabled = applicationConfiguration.Object.IsExtraLoggingEnabled;

                AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
                {
                    using (var googleClient = bootstrapper.IocManager.ResolveAsDisposable<IGoogleAnalyticsService>())
                    using (var logger = bootstrapper.IocManager.ResolveAsDisposable<ILogger>())
                    {
                        if (isExtraLoggingEnabled)
                        {
                            logger.Object.Error($"Unhandled Exception occured: {args.ExceptionObject.ToString()}");
                        }

                        googleClient.Object.TrackException(args.ExceptionObject.ToString(), false);
                    }
                };

                AppDomain.CurrentDomain.FirstChanceException += (sender, args) =>
                {
                    using (var googleClient = bootstrapper.IocManager.ResolveAsDisposable<IGoogleAnalyticsService>())
                    using (var logger = bootstrapper.IocManager.ResolveAsDisposable<ILogger>())
                    {
                        if (isExtraLoggingEnabled)
                        {
                            logger.Object.Error($"First Chance Exception: {args.Exception.ToString()}");
                        }

                        googleClient.Object.TrackException(args.Exception.ToString(), false);
                    }
                };

            }
        }
    }
}
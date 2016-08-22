using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

using Abp;
using Abp.Dependency;
using Abp.Runtime.Caching.Configuration;

using Castle.Core.Logging;
using Castle.Facilities.Logging;

using DynamicTranslator.Configuration.Startup;
using DynamicTranslator.Constants;
using DynamicTranslator.Service.GoogleAnalytics;
using DynamicTranslator.Wpf.ViewModel;

namespace DynamicTranslator.Wpf
{
    public partial class App
    {
        private readonly AbpBootstrapper bootstrapper;

        public App()
        {
            bootstrapper = AbpBootstrapper.Create<DynamicTranslatorWpfModule>();
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

        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
        }

        private void ConfigureMemoryCache()
        {
            var cacheConfiguration = bootstrapper.IocManager.Resolve<ICachingConfiguration>();

            cacheConfiguration.Configure(CacheNames.MeanCache, cache => { cache.DefaultSlidingExpireTime = TimeSpan.FromHours(24); });

            cacheConfiguration.Configure(CacheNames.ReleaseCache, cache => { cache.DefaultSlidingExpireTime = TimeSpan.FromMinutes(10); });
        }

        private void HandleExceptionsOrNothing()
        {
            using (var applicationConfiguration = bootstrapper.IocManager.ResolveAsDisposable<IApplicationConfiguration>())
            {
                var isExtraLoggingEnabled = applicationConfiguration.Object.IsExtraLoggingEnabled;

                AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
                {
                    using (var googleClient = bootstrapper.IocManager.ResolveAsDisposable<IGoogleAnalyticsService>())
                    {
                        using (var logger = bootstrapper.IocManager.ResolveAsDisposable<ILogger>())
                        {
                            if (isExtraLoggingEnabled)
                            {
                                logger.Object.Error($"Unhandled Exception occured: {args.ExceptionObject.ToString()}");
                            }

                            googleClient.Object.TrackException(args.ExceptionObject.ToString(), false);
                        }
                    }
                };

                AppDomain.CurrentDomain.FirstChanceException += (sender, args) =>
                {
                    using (var googleClient = bootstrapper.IocManager.ResolveAsDisposable<IGoogleAnalyticsService>())
                    {
                        using (var logger = bootstrapper.IocManager.ResolveAsDisposable<ILogger>())
                        {
                            if (isExtraLoggingEnabled)
                            {
                                logger.Object.Error($"First Chance Exception: {args.Exception.ToString()}");
                            }

                            googleClient.Object.TrackException(args.Exception.ToString(), false);
                        }
                    }
                };

                TaskScheduler.UnobservedTaskException += (sender, args) =>
                {
                    using (var googleClient = bootstrapper.IocManager.ResolveAsDisposable<IGoogleAnalyticsService>())
                    {
                        using (var logger = bootstrapper.IocManager.ResolveAsDisposable<ILogger>())
                        {
                            if (isExtraLoggingEnabled)
                            {
                                logger.Object.Error($"Unhandled Exception occured: {args.Exception.ToString()}");
                            }

                            googleClient.Object.TrackException(args.Exception.ToString(), false);
                        }
                    }
                };
            }
        }
    }
}
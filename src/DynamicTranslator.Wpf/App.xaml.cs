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
        private readonly AbpBootstrapper _bootstrapper;

        public App()
        {
            _bootstrapper = AbpBootstrapper.Create<DynamicTranslatorWpfModule>();
            _bootstrapper.IocManager.IocContainer.AddFacility<LoggingFacility>(f => f.UseNLog());
        }

        protected override void OnStartup(StartupEventArgs eventArgs)
        {
            _bootstrapper.Initialize();

            _bootstrapper.IocManager.Register<IGrowlNotifications, GrowlNotifications>();

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
            var cacheConfiguration = _bootstrapper.IocManager.Resolve<ICachingConfiguration>();

            cacheConfiguration.Configure(CacheNames.MeanCache, cache => { cache.DefaultSlidingExpireTime = TimeSpan.FromHours(24); });

            cacheConfiguration.Configure(CacheNames.ReleaseCache, cache => { cache.DefaultSlidingExpireTime = TimeSpan.FromMinutes(10); });
        }

        private void HandleExceptionsOrNothing()
        {
            using (var applicationConfiguration = _bootstrapper.IocManager.ResolveAsDisposable<IApplicationConfiguration>())
            {
                var isExtraLoggingEnabled = applicationConfiguration.Object.IsExtraLoggingEnabled;

                AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
                {
                    using (var googleClient = _bootstrapper.IocManager.ResolveAsDisposable<IGoogleAnalyticsService>())
                    {
                        using (var logger = _bootstrapper.IocManager.ResolveAsDisposable<ILogger>())
                        {
                            if (isExtraLoggingEnabled)
                            {
                                logger.Object.Error($"Unhandled Exception occured: {args.ExceptionObject.ToString()}");
                            }

                            try
                            {
                                googleClient.Object.TrackException(args.ExceptionObject.ToString(), false);
                            }
                            catch (Exception)
                            {
                                //throw;
                            }
                        }
                    }
                };

                TaskScheduler.UnobservedTaskException += (sender, args) =>
                {
                    args.Exception.Handle(exception =>
                        {
                            using (var googleClient = _bootstrapper.IocManager.ResolveAsDisposable<IGoogleAnalyticsService>())
                            {
                                using (var logger = _bootstrapper.IocManager.ResolveAsDisposable<ILogger>())
                                {
                                    if (isExtraLoggingEnabled)
                                    {
                                        logger.Object.Error($"Unhandled Exception occured: {exception.ToString()}");
                                    }

                                    try
                                    {
                                        googleClient.Object.TrackException(exception.ToString(), false);
                                    }
                                    catch (Exception)
                                    {
                                        //throw;
                                    }
                                }
                            }
                            return true;
                        });
                };
            }
        }
    }
}

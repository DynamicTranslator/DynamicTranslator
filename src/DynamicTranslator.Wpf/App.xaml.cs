using System;
using System.Security.AccessControl;
using System.Threading;
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
        private Mutex _mutex;
        private const string MutexName = @"Global\1109F104-B4B4-4ED1-920C-F4D8EFE9E834}";
        private bool _isMutexCreated;
        private bool _isMutexUnauthorized;

        public App()
        {
            _bootstrapper = AbpBootstrapper.Create<DynamicTranslatorWpfModule>();
            _bootstrapper.IocManager.IocContainer.AddFacility<LoggingFacility>(f => f.UseNLog());
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _bootstrapper.Dispose();
        }

        protected override void OnStartup(StartupEventArgs eventArgs)
        {
            _bootstrapper.Initialize();

            _bootstrapper.IocManager.Register<IGrowlNotifications, GrowlNotifications>();

            HandleExceptionsOrNothing();

            ConfigureMemoryCache();

            CheckApplicationInstanceExist();

            base.OnStartup(eventArgs);
        }

        private void CheckApplicationInstanceExist()
        {
            string user = Environment.UserDomainName + "\\" + Environment.UserName;

            try
            {
                Mutex.TryOpenExisting(MutexName, out _mutex);

                if (_mutex == null)
                {
                    var mutexSecurity = new MutexSecurity();

                    var rule = new MutexAccessRule(user, MutexRights.Synchronize | MutexRights.Modify, AccessControlType.Deny);

                    mutexSecurity.AddAccessRule(rule);

                    rule = new MutexAccessRule(user, MutexRights.ReadPermissions | MutexRights.ChangePermissions, AccessControlType.Allow);

                    mutexSecurity.AddAccessRule(rule);

                    _mutex = new Mutex(true, MutexName, out _isMutexCreated, mutexSecurity);
                }
            }
            catch (UnauthorizedAccessException)
            {
                _isMutexUnauthorized = true;
            }

            if (!_isMutexUnauthorized && _isMutexCreated)
            {
                _mutex?.WaitOne();
                GC.KeepAlive(_mutex);
                return;
            }

            _mutex?.ReleaseMutex();
            _mutex?.Dispose();
            Current.Shutdown();
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
            using (IScopedIocResolver scope = _bootstrapper.IocManager.CreateScope())
            {
                bool isExtraLoggingEnabled = scope.Resolve<IApplicationConfiguration>().IsExtraLoggingEnabled;

                AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
                {
                    var googleClient = scope.Resolve<IGoogleAnalyticsService>();
                    var logger = scope.Resolve<ILogger>();

                    if (isExtraLoggingEnabled) logger.Error($"Unhandled Exception occured: {args.ExceptionObject.ToString()}");

                    try
                    {
                        googleClient.TrackException(args.ExceptionObject.ToString(), false);
                    }
                    catch (Exception)
                    {
                        //throw;
                    }
                };

                TaskScheduler.UnobservedTaskException += (sender, args) =>
                {
                    args.Exception.Handle(exception =>
                    {
                        var googleClient = scope.Resolve<IGoogleAnalyticsService>();
                        var logger = scope.Resolve<ILogger>();

                        if (isExtraLoggingEnabled) logger.Error($"Unhandled Exception occured: {exception.ToString()}");

                        try
                        {
                            googleClient.TrackException(exception.ToString(), false);
                        }
                        catch (Exception)
                        {
                            //throw;
                        }
                        return true;
                    });
                };
            }
        }
    }
}

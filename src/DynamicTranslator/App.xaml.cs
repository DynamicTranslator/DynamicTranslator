namespace DynamicTranslator
{
    using System;
    using System.IO;
    using System.Security.AccessControl;
    using System.Threading;
    using System.Windows;
    using System.Windows.Threading;
    using Core;
    using Microsoft.Extensions.DependencyInjection;
    using ViewModel;

    public partial class App
    {
        const string MutexName = @"Global\1109F104-B4B4-4ED1-920C-F4D8EFE9E834}";
        bool isMutexCreated;
        bool isMutexUnauthorized;
        Mutex mutex;
        WireUp wireUp;

        public App()
        {
            GuardAgainstMultipleInstances();
        }

        protected override void OnStartup(StartupEventArgs eventArgs)
        {
            this.wireUp = new WireUp(postConfigureServices: services =>
            {
                services.AddSingleton<Notifications>();
                services.AddTransient<IClipboardManager, ClipboardManager>();
                services.AddSingleton<GrowlNotifications>();
                services.AddTransient<TranslatorBootstrapper>();
                services.AddTransient<INotifier, GrowlNotifier>();
                services.AddSingleton<MainWindow>();
            });

            DispatcherUnhandledException += (sender, args) =>
            {
                args.Handled = true;
                this.wireUp.ServiceProvider.GetRequiredService<GrowlNotifications>().AddNotification(
                    new Notification {Title = "Error", Message = "An unhandled exception occurred!"});
            };

            var mainWindow = this.wireUp.ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Closed += (sender, args) => { Current.Shutdown(0); };
            mainWindow.InitializeComponent();
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            this.wireUp.Dispose();
        }

        void GuardAgainstMultipleInstances()
        {
            string user = Environment.UserDomainName + Path.DirectorySeparatorChar + Environment.UserName;

            try
            {
                Mutex.TryOpenExisting(MutexName, out this.mutex);

                if (this.mutex == null)
                {
                    var mutexSecurity = new MutexSecurity();

                    var rule = new MutexAccessRule(user, MutexRights.Synchronize | MutexRights.Modify,
                        AccessControlType.Deny);

                    mutexSecurity.AddAccessRule(rule);

                    rule = new MutexAccessRule(user, MutexRights.ReadPermissions | MutexRights.ChangePermissions,
                        AccessControlType.Allow);

                    mutexSecurity.AddAccessRule(rule);

                    this.mutex = new Mutex(true, MutexName, out this.isMutexCreated);
                }
            }
            catch (UnauthorizedAccessException) { this.isMutexUnauthorized = true; }

            if (!this.isMutexUnauthorized && this.isMutexCreated)
            {
                this.mutex?.WaitOne();
                GC.KeepAlive(this.mutex);
                return;
            }

            this.mutex?.ReleaseMutex();
            this.mutex?.Dispose();
            Current.Shutdown();
        }

        void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
        }
    }
}
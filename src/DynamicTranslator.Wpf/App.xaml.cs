using System;
using System.IO;
using System.Security.AccessControl;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using DynamicTranslator.Wpf.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace DynamicTranslator.Wpf
{
	public partial class App : Application
	{
		private Mutex _mutex;
		private const string MutexName = @"Global\1109F104-B4B4-4ED1-920C-F4D8EFE9E834}";
		private bool _isMutexCreated;
		private bool _isMutexUnauthorized;
        private WireUp _wireUp;

        public App()
		{
			GuardAgainstMultipleInstances();
		}

        protected override void OnStartup(StartupEventArgs eventArgs)
        {
            _wireUp = new WireUp(postConfigureServices: services =>
            {
                services.AddSingleton<Notifications>();
                services.AddTransient<ClipboardManager>();
                services.AddSingleton<GrowlNotifications>();
                services.AddTransient<TranslatorBootstrapper>();
                services.AddTransient<INotifier, GrowlNotifier>();
                services.AddSingleton<MainWindow>();
            });

            var mainWindow = _wireUp.ServiceProvider.GetService<MainWindow>();
            Current.Exit += (sender, args) =>
            {
                _wireUp.Dispose();
            };
            mainWindow.InitializeComponent();
            mainWindow.Show();
        }

        private void GuardAgainstMultipleInstances()
		{
			string user = Environment.UserDomainName + Path.DirectorySeparatorChar + Environment.UserName;

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

					_mutex = new Mutex(true, MutexName, out _isMutexCreated);
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
    }
}

using System;
using System.Reflection;
using System.Windows;

using DynamicTranslator.Config;
using DynamicTranslator.Dependency;
using DynamicTranslator.Dependency.Installer;
using DynamicTranslator.Dependency.Manager;
using DynamicTranslator.Optimizers.Runtime.Caching;
using DynamicTranslator.ViewModel.Interfaces;
using DynamicTranslator.Wpf.ViewModel;

namespace DynamicTranslator.Wpf
{
    /// <summary>
    ///     The app root class.
    /// </summary>
    public partial class App
    {
        /// <summary>
        ///     First place of program start.
        /// </summary>
        /// <param name="eventArgs">
        ///     Bootstrap of program.
        /// </param>
        protected override void OnStartup(StartupEventArgs eventArgs)
        {
            IocManager.Instance.AddConventionalRegistrar(new BasicConventionalRegistrar());
            UnitOfWorkRegistrar.Initialize(IocManager.Instance);

            IocManager.Instance.RegisterAssemblyByConvention(Assembly.Load("DynamicTranslator"));
            IocManager.Instance.RegisterAssemblyByConvention(Assembly.Load("DynamicTranslator.Wpf"));

            IocManager.Instance.Register<IGrowlNotifications, GrowlNotifiactions>();

            var defaultSlidingExpireTime = TimeSpan.FromHours(24);
            IocManager.Instance.Resolve<ICachingConfiguration>().ConfigureAll(cache => { cache.DefaultSlidingExpireTime = defaultSlidingExpireTime; });

            var configurations = IocManager.Instance.Resolve<IStartupConfiguration>();
            configurations.Initialize();
            base.OnStartup(eventArgs);
        }
    }
}
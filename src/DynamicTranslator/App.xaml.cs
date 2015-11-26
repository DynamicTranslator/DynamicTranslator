namespace DynamicTranslator
{
    #region using

    using System;
    using System.Reflection;
    using System.Windows;
    using Core.Config;
    using Core.Dependency;
    using Core.Dependency.Installer;
    using Core.Dependency.Manager;
    using Core.Optimizers.Runtime.Caching;
    using Core.ViewModel.Interfaces;
    using ViewModel;

    #endregion

    /// <summary>
    /// The app root class.
    /// </summary>
    public partial class App
    {
        /// <summary>
        /// First place of program start.
        /// </summary>
        /// <param name="eventArgs">
        /// Bootstrap of program.
        /// </param>
        protected override void OnStartup(StartupEventArgs eventArgs)
        {
            IocManager.Instance.AddConventionalRegistrar(new BasicConventionalRegistrar());
            UnitOfWorkRegistrar.Initialize(IocManager.Instance);

            IocManager.Instance.RegisterAssemblyByConvention(Assembly.Load("DynamicTranslator.Core"));
            IocManager.Instance.RegisterAssemblyByConvention(Assembly.Load("DynamicTranslator"));

            IocManager.Instance.Register<IGrowlNotifications, GrowlNotifiactions>();

            var defaultSlidingExpireTime = TimeSpan.FromHours(24);
            IocManager.Instance.Resolve<ICachingConfiguration>().ConfigureAll(cache => { cache.DefaultSlidingExpireTime = defaultSlidingExpireTime; });

            var configurations = IocManager.Instance.Resolve<IStartupConfiguration>();
            configurations.Initialize();
            base.OnStartup(eventArgs);
        }
    }
}
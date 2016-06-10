using System;
using System.Reflection;
using System.Windows;

using DynamicTranslator.Core.Config;
using DynamicTranslator.Core.Dependency;
using DynamicTranslator.Core.Dependency.Installer;
using DynamicTranslator.Core.Dependency.Manager;
using DynamicTranslator.Core.Optimizers.Runtime.Caching;
using DynamicTranslator.Core.ViewModel.Interfaces;
using DynamicTranslator.ViewModel;

namespace DynamicTranslator
{
    #region using

    

    #endregion

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
namespace Dynamic.Translator
{
    #region using

    using System;
    using System.Reflection;
    using System.Windows;
    using Core.Config;
    using Core.Dependency;
    using Core.Dependency.Manager;
    using Core.Optimizers.Runtime;
    using Core.Optimizers.Runtime.Caching;
    using Core.ViewModel.Interfaces;
    using ViewModel;

    #endregion

    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            IocManager.Instance.AddConventionalRegistrar(new BasicConventionalRegistrar());

            IocManager.Instance.RegisterAssemblyByConvention(Assembly.Load("Dynamic.Translator.Core"));
            IocManager.Instance.RegisterAssemblyByConvention(Assembly.Load("Dynamic.Translator"));

            IocManager.Instance.Register<IGrowlNotifications, GrowlNotifiactions>();

            var defaultSlidingExpireTime = TimeSpan.FromHours(24);
            IocManager.Instance.Resolve<ICachingConfiguration>().ConfigureAll(cache =>
            {
                cache.DefaultSlidingExpireTime = defaultSlidingExpireTime;
            });

            var configurations = IocManager.Instance.Resolve<IStartupConfiguration>();
            configurations.Initialize();
            base.OnStartup(e);
        }
    }
}
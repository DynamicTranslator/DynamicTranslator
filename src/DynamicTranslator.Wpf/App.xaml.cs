using System;
using System.Windows;

using Abp;

using DynamicTranslator.Config;
using DynamicTranslator.Optimizers.Runtime.Caching;
using DynamicTranslator.ViewModel.Interfaces;
using DynamicTranslator.Wpf.ViewModel;

namespace DynamicTranslator.Wpf
{
    public partial class App
    {
        private readonly AbpBootstrapper bootstrapper;

        public App()
        {
            bootstrapper = new AbpBootstrapper();
            bootstrapper.Initialize();
        }

        protected override void OnStartup(StartupEventArgs eventArgs)
        {
            IocManager.Instance.Register<IGrowlNotifications, GrowlNotifiactions>();

            var defaultSlidingExpireTime = TimeSpan.FromHours(24);
            IocManager.Instance.Resolve<ICachingConfiguration>().ConfigureAll(cache => { cache.DefaultSlidingExpireTime = defaultSlidingExpireTime; });

            var configurations = IocManager.Instance.Resolve<IDynamicTranslatorConfiguration>();
            configurations.Initialize();
            base.OnStartup(eventArgs);
        }
    }
}
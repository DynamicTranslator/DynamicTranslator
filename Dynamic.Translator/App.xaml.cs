namespace Dynamic.Tureng.Translator
{
    #region using

    using System.Reflection;
    using System.Windows;
    using Dynamic.Translator.Core.Config;
    using Dynamic.Translator.Core.Dependency;
    using Dynamic.Translator.Core.ViewModel;
    using Dynamic.Translator.Core.ViewModel.Interfaces;

    #endregion

    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            IocManager.Instance.AddConventionalRegistrar(new BasicConventionalRegistrar());
            IocManager.Instance.RegisterAssemblyByConvention(Assembly.Load("Dynamic.Translator.Core"));

            IocManager.Instance.Register<IGrowlNotifications, GrowlNotifiactions>(DependencyLifeStyle.Transient);
            IocManager.Instance.Register<Notifications>(DependencyLifeStyle.Transient);

            var configurations = IocManager.Instance.Resolve<IStartupConfiguration>();
            configurations.Initialize();
        }
    }
}
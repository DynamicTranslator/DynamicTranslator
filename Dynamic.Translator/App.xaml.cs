namespace Dynamic.Tureng.Translator
{
    #region using

    using System.Reflection;
    using System.Windows;
    using Dynamic.Translator.Core.Config;
    using Dynamic.Translator.Core.Dependency;
    using Dynamic.Translator.Core.Dependency.Manager;
    using Dynamic.Translator.Core.Dependency.Markers;
    using Dynamic.Translator.Core.Orchestrators;
    using Dynamic.Translator.Core.ViewModel;
    using Dynamic.Translator.Core.ViewModel.Interfaces;
    using Orchestrators.Finders;
    using Orchestrators.Organizers;

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
            IocManager.Instance.Register<IGrowlNotifications, GrowlNotifiactions>(DependencyLifeStyle.Transient);
            IocManager.Instance.Register<Notifications>(DependencyLifeStyle.Transient);

            IocManager.Instance.RegisterAssemblyByConvention(Assembly.Load("Dynamic.Tureng.Translator"));
            IocManager.Instance.RegisterAssemblyByConvention(Assembly.Load("Dynamic.Translator.Core"));

            var configurations = IocManager.Instance.Resolve<IStartupConfiguration>();
            configurations.Initialize();
        }
    }
}
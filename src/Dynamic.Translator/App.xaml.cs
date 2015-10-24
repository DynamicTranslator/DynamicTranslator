namespace Dynamic.Translator
{
    #region using

    using System.Reflection;
    using System.Windows;
    using Core.Config;
    using Core.Dependency;
    using Core.Dependency.Manager;
    using Core.Dependency.Markers;
    using Core.Orchestrators;
    using Core.ViewModel;
    using Core.ViewModel.Interfaces;
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

            IocManager.Instance.Register<IMeanFinder, SesliSozlukFinder>();
            IocManager.Instance.Register<IMeanFinder, TurengFinder>();
            IocManager.Instance.Register<IMeanFinder, YandexFinder>();


            IocManager.Instance.Register<IMeanOrganizer, SesliSozlukMeanOrganizer>();
            IocManager.Instance.Register<IMeanOrganizer, TurengMeanOrganizer>();
            IocManager.Instance.Register<IMeanOrganizer, YandexMeanOrganizer>();


            IocManager.Instance.RegisterAssemblyByConvention(Assembly.Load("Dynamic.Translator.Core"));

            var configurations = IocManager.Instance.Resolve<IStartupConfiguration>();
            configurations.Initialize();
        }
    }
}
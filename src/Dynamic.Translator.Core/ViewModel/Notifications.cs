namespace Dynamic.Translator.Core.ViewModel
{
    using Dependency.Markers;

    public class Notifications : MultiThreadObservableCollection<Notification>, ITransientDependency
    {
    }
}
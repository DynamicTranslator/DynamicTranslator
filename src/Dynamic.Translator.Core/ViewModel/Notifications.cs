namespace Dynamic.Translator.Core.ViewModel
{
    #region using

    using Dependency.Markers;

    #endregion

    public class Notifications : MultiThreadObservableCollection<Notification>, ITransientDependency
    {
    }
}
namespace DynamicTranslator.Core.ViewModel
{
    #region using

    using Dependency.Markers;

    #endregion

    public class Notifications : MultiThreadObservableCollection<Notification>, ITransientDependency
    {
    }
}
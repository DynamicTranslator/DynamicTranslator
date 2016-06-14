using DynamicTranslator.Dependency.Markers;

namespace DynamicTranslator.ViewModel
{
    #region using

    

    #endregion

    public class Notifications : MultiThreadObservableCollection<Notification>, ITransientDependency {}
}
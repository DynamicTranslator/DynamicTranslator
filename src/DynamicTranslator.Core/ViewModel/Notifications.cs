using DynamicTranslator.Core.Dependency.Markers;

namespace DynamicTranslator.Core.ViewModel
{
    #region using

    

    #endregion

    public class Notifications : MultiThreadObservableCollection<Notification>, ITransientDependency {}
}
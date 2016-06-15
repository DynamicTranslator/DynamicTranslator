using Abp.Dependency;

namespace DynamicTranslator.ViewModel
{
    public class Notifications : MultiThreadObservableCollection<Notification>, ITransientDependency {}
}
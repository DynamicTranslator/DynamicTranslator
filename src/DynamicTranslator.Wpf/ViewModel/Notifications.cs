using Abp.Dependency;

namespace DynamicTranslator.Wpf.ViewModel
{
    public class Notifications : MultiThreadObservableCollection<Notification>, ITransientDependency {}
}
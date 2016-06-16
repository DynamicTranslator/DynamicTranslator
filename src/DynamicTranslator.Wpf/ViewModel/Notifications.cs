using Abp.Dependency;

using DynamicTranslator.ObservableUtil;

namespace DynamicTranslator.Wpf.ViewModel
{
    public class Notifications : MultiThreadObservableCollection<Notification>, ITransientDependency {}
}
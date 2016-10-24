using System.Threading.Tasks;

using Abp.Dependency;

using DynamicTranslator.Wpf.ViewModel;

namespace DynamicTranslator.Wpf.Notification
{
    public class Notifier : INotifier, ITransientDependency
    {
        private readonly IGrowlNotifications _growlNotifiactions;

        public Notifier(IGrowlNotifications growlNotifiactions)
        {
            _growlNotifiactions = growlNotifiactions;
        }

        public void AddNotification(string title, string imageUrl, string text)
        {
            _growlNotifiactions.AddNotification(new ViewModel.Notification { ImageUrl = imageUrl, Message = text, Title = title });
        }

        public Task AddNotificationAsync(string title, string imageUrl, string text)
        {
            return _growlNotifiactions.AddNotificationAsync(new ViewModel.Notification { ImageUrl = imageUrl, Message = text, Title = title });
        }
    }
}

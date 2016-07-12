using System.Threading.Tasks;

using Abp.Dependency;

using DynamicTranslator.Wpf.ViewModel;

namespace DynamicTranslator.Wpf.Notification
{
    public class Notifier : INotifier, ITransientDependency
    {
        public Notifier(IGrowlNotifications growlNotifiactions)
        {
            this.growlNotifiactions = growlNotifiactions;
        }

        private readonly IGrowlNotifications growlNotifiactions;

        public void AddNotification(string title, string imageUrl, string text)
        {
            growlNotifiactions.AddNotification(new ViewModel.Notification {ImageUrl = imageUrl, Message = text, Title = title});
        }

        public Task AddNotificationAsync(string title, string imageUrl, string text)
        {
            return growlNotifiactions.AddNotificationAsync(new ViewModel.Notification {ImageUrl = imageUrl, Message = text, Title = title});
        }
    }
}
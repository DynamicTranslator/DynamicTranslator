using System;
using System.Threading.Tasks;

namespace DynamicTranslator.Wpf.ViewModel
{
    public interface IGrowlNotifications : IDisposable
    {
        event EventHandler OnDispose;

        void AddNotification(Notification notification);

        Task AddNotificationAsync(Notification notification);

        void RemoveNotification(Notification notification);
    }
}
using System;
using System.Threading.Tasks;

namespace DynamicTranslator.ViewModel.Interfaces
{
    #region using

    

    #endregion

    public interface IGrowlNotifications : IDisposable
    {
        event EventHandler OnDispose;

        void AddNotification(Notification notification);

        Task AddNotificationAsync(Notification notification);

        void RemoveNotification(Notification notification);
    }
}
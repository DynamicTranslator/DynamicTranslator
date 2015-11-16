namespace DynamicTranslator.Core.ViewModel.Interfaces
{
    #region using

    using System;
    using System.Threading.Tasks;

    #endregion

    public interface IGrowlNotifications : IDisposable
    {
        event EventHandler OnDispose;

        Task AddNotificationAsync(Notification notification);

        void AddNotification(Notification notification);

        void RemoveNotification(Notification notification);
    }
}
namespace DynamicTranslator.Core.ViewModel.Interfaces
{
    #region using

    using System;
    using System.Threading.Tasks;

    #endregion

    public interface IGrowlNotifications : IDisposable
    {
        event EventHandler OnDispose;

        void AddNotification(Notification notification);

        Task AddNotificationAsync(Notification notification);

        void RemoveNotification(Notification notification);
    }
}
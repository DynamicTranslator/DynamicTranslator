namespace Dynamic.Translator.Core.ViewModel.Interfaces
{
    #region using

    using System;
    using System.Threading.Tasks;

    #endregion

    public interface IGrowlNotifications : IDisposable
    {
        Task AddNotificationAsync(Notification notification);
        void AddNotificationSync(Notification notification);
        void RemoveNotification(Notification notification);

        event EventHandler OnDispose;
    }
}
namespace DynamicTranslator.ViewModel
{
    #region using

    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using Core.Config;
    using Core.Extensions;
    using Core.ViewModel;
    using Core.ViewModel.Interfaces;

    #endregion

    public partial class GrowlNotifiactions : IGrowlNotifications
    {
        private readonly Notifications buffer = new Notifications();
        public readonly Notifications Notifications;
        private readonly IStartupConfiguration startupConfiguration;
        private int count;
        public bool IsDisposed;

        public GrowlNotifiactions(IStartupConfiguration startupConfiguration, Notifications notifications)
        {
            InitializeComponent();
            this.startupConfiguration = startupConfiguration;
            Notifications = notifications;
            NotificationsControl.DataContext = Notifications;
        }

        public event EventHandler OnDispose;

        public async Task AddNotificationAsync(Notification notification)
        {
            await Task.Run(() => AddNotification(notification)).ConfigureAwait(false);
        }

        public void AddNotification(Notification notification)
        {
            Dispatcher.InvokeAsync(() =>
            {
                notification.Id = count++;
                if (Notifications.Count + 1 > startupConfiguration.MaxNotifications)
                    buffer.Add(notification);
                else
                    Notifications.Add(notification);

                if (Notifications.Count > 0 && !IsActive)
                    Show();
            });
        }

        public void RemoveNotification(Notification notification)
        {
            if (Notifications.Contains(notification))
                Notifications.Remove(notification);

            if (buffer.Count > 0)
            {
                Notifications.Add(buffer[0]);
                buffer.RemoveAt(0);
            }

            if (Notifications.Count < 1)
            {
                Hide();
                OnDispose.InvokeSafely(this, new EventArgs());
            }
        }

        public void Dispose()
        {
            if (IsDisposed)
                return;

            OnDispose.InvokeSafely(this, new EventArgs());

            IsDisposed = true;
        }

        private void NotificationWindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Math.Abs(e.NewSize.Height) > 0.0)
                return;

            var element = sender as Grid;
            RemoveNotification(Notifications.First(n => element != null && n.Id == int.Parse(element.Tag.ToString())));
        }
    }
}
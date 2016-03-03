namespace DynamicTranslator.ViewModel
{
    #region using

    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Threading;
    using Core.Config;
    using Core.Extensions;
    using Core.ViewModel;
    using Core.ViewModel.Interfaces;

    #endregion

    public partial class GrowlNotifiactions : IGrowlNotifications
    {
        public readonly Notifications Notifications;
        public bool IsDisposed;
        private readonly Notifications buffer = new Notifications();
        private readonly IStartupConfiguration startupConfiguration;
        private int count;

        public GrowlNotifiactions(IStartupConfiguration startupConfiguration, Notifications notifications)
        {
            InitializeComponent();
            this.startupConfiguration = startupConfiguration;
            Notifications = notifications;
            NotificationsControl.DataContext = Notifications;
        }

        public event EventHandler OnDispose;

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
            }, DispatcherPriority.Background);
        }

        public async Task AddNotificationAsync(Notification notification)
        {
            await Task.Run(() => AddNotification(notification));
        }

        public void Dispose()
        {
            if (IsDisposed)
                return;

            Notifications.Clear();
            buffer.Clear();

            OnDispose.InvokeSafely(this, new EventArgs());

            IsDisposed = true;
            GC.SuppressFinalize(this);
        }

        public void RemoveNotification(Notification notification)
        {
            Dispatcher.InvokeAsync(() =>
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
            }, DispatcherPriority.Background);
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
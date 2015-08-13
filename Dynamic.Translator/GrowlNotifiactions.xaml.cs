namespace Dynamic.Tureng.Translator
{
    #region using

    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using Dynamic.Translator.Core.Config;
    using Dynamic.Translator.Core.Extensions;
    using Dynamic.Translator.Core.ViewModel;
    using Dynamic.Translator.Core.ViewModel.Interfaces;

    #endregion

    public partial class GrowlNotifiactions : IGrowlNotifications
    {
        private readonly Notifications buffer = new Notifications();
        public readonly Notifications Notifications;
        private readonly IStartupConfiguration startupConfiguration;
        private int count;
        public bool IsDisposed;
        private int _dynamicHeight;

        public GrowlNotifiactions(IStartupConfiguration startupConfiguration, Notifications notifications)
        {
            InitializeComponent();
            this.startupConfiguration = startupConfiguration;
            Notifications = notifications;
            NotificationsControl.DataContext = Notifications;
        }

        public async Task AddNotificationAsync(Notification notification)
        {
            AddNotificationSync(notification);
        }

        public void AddNotificationSync(Notification notification)
        {
            notification.Id = count++;
            if (Notifications.Count + 1 > startupConfiguration.MaxNotifications)
            {
                buffer.Add(notification);
            }
            else
            {
                Notifications.Add(notification);
            }

            if (Notifications.Count > 0 && !IsActive)
            {
                Show();
            }
        }

        public void RemoveNotification(Notification notification)
        {
            if (Notifications.Contains(notification))
            {
                Notifications.Remove(notification);
            }

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

        public event EventHandler OnDispose;

        public void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            OnDispose.InvokeSafely(this, new EventArgs());

            IsDisposed = true;
        }

        private void NotificationWindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Math.Abs(e.NewSize.Height) > 0.0)
            {
                return;
            }

            var element = sender as Grid;
            RemoveNotification(Notifications.First(n => element != null && n.Id == int.Parse(element.Tag.ToString())));
        }

        public int DynamicHeight
        {
            get { return 1; }
            set { _dynamicHeight = value; }
        }
    }
}
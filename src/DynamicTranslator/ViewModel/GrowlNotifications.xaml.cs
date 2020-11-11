namespace DynamicTranslator.ViewModel
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Threading;
    using Core.Configuration;
    using Core.Extensions;

    public partial class GrowlNotifications
    {
        readonly IApplicationConfiguration applicationConfiguration;
        readonly Notifications buffer = new Notifications();
        public readonly Notifications Notifications;
        int count;
        public bool IsDisposed;

        public GrowlNotifications(IApplicationConfiguration applicationConfiguration, Notifications notifications)
        {
            InitializeComponent();
            this.applicationConfiguration = applicationConfiguration;
            this.Notifications = notifications;
            this.NotificationsControl.DataContext = this.Notifications;
        }

        public event EventHandler OnDispose;

        public void Dispose()
        {
            if (this.IsDisposed) return;

            this.Notifications.Clear();
            this.buffer.Clear();

            OnDispose.InvokeSafely(this, new EventArgs());

            this.IsDisposed = true;
            GC.SuppressFinalize(this);
        }

        public void AddNotification(Notification notification)
        {
            Dispatcher.InvokeAsync(
                () =>
                {
                    notification.Id = this.count++;
                    if (this.Notifications.Count + 1 > this.applicationConfiguration.MaxNotifications)
                        this.buffer.Add(notification);
                    else
                        this.Notifications.Add(notification);

                    if (this.Notifications.Count > 0 && !IsActive) Show();
                },
                DispatcherPriority.Background);
        }

        public void RemoveNotification(Notification notification)
        {
            Dispatcher.InvokeAsync(
                () =>
                {
                    if (this.Notifications.Contains(notification)) this.Notifications.Remove(notification);

                    if (this.buffer.Count > 0)
                    {
                        this.Notifications.Add(this.buffer[0]);
                        this.buffer.RemoveAt(0);
                    }

                    if (this.Notifications.Count < 1)
                    {
                        Hide();
                        OnDispose.InvokeSafely(this, new EventArgs());
                    }
                },
                DispatcherPriority.Background);
        }

        void NotificationWindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Math.Abs(e.NewSize.Height) > 0.0) return;

            var element = sender as Grid;
            RemoveNotification(this.Notifications.First(n =>
                element != null && n.Id == int.Parse(element.Tag.ToString())));
        }
    }
}
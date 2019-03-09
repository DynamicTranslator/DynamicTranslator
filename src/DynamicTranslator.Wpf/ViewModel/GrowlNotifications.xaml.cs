using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using DynamicTranslator.Configuration;
using DynamicTranslator.Extensions;

namespace DynamicTranslator.Wpf.ViewModel
{
    public partial class GrowlNotifications
    {
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly Notifications _buffer = new Notifications();
        public readonly Notifications Notifications;
        private int _count;
        public bool IsDisposed;

        public GrowlNotifications(ApplicationConfiguration applicationConfiguration, Notifications notifications)
        {
            InitializeComponent();
            _applicationConfiguration = applicationConfiguration;
            Notifications = notifications;
            NotificationsControl.DataContext = Notifications;
        }

        public event EventHandler OnDispose;

        public void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            Notifications.Clear();
            _buffer.Clear();

            OnDispose.InvokeSafely(this, new EventArgs());

            IsDisposed = true;
            GC.SuppressFinalize(this);
        }

        public void AddNotification(Notification notification)
        {
            Dispatcher.InvokeAsync(
                () =>
                {
                    notification.Id = _count++;
                    if (Notifications.Count + 1 > _applicationConfiguration.MaxNotifications)
                    {
                        _buffer.Add(notification);
                    }
                    else
                    {
                        Notifications.Add(notification);
                    }

                    if ((Notifications.Count > 0) && !IsActive)
                    {
                        Show();
                    }
                },
                DispatcherPriority.Background);
        }

        public void RemoveNotification(Notification notification)
        {
            Dispatcher.InvokeAsync(
                () =>
                {
                    if (Notifications.Contains(notification))
                    {
                        Notifications.Remove(notification);
                    }

                    if (_buffer.Count > 0)
                    {
                        Notifications.Add(_buffer[0]);
                        _buffer.RemoveAt(0);
                    }

                    if (Notifications.Count < 1)
                    {
                        Hide();
                        OnDispose.InvokeSafely(this, new EventArgs());
                    }
                },
                DispatcherPriority.Background);
        }

        private void NotificationWindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Math.Abs(e.NewSize.Height) > 0.0)
            {
                return;
            }

            var element = sender as Grid;
            RemoveNotification(Notifications.First(n => (element != null) && (n.Id == int.Parse(element.Tag.ToString()))));
        }
    }
}

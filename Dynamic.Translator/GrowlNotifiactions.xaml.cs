// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GrowlNotifiactions.xaml.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the GrowlNotifiactions type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Dynamic.Tureng.Translator
{
    #region Using

    using System.Configuration;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    #endregion

    public partial class GrowlNotifiactions
    {
        private static readonly byte MaxNotifications = byte.Parse(ConfigurationManager.AppSettings["MaxNotifications"]);
        private readonly Notifications _buffer = new Notifications();
        private int _count;
        public Notifications Notifications = new Notifications();

        public GrowlNotifiactions()
        {
            InitializeComponent();
            NotificationsControl.DataContext = Notifications;
        }

        public void AddNotification(Notification notification)
        {
            notification.Id = _count++;
            if (Notifications.Count + 1 > MaxNotifications)
            {
                _buffer.Add(notification);
            }
            else
            {
                Notifications.Add(notification);
            }

            // Show window if there're notifications
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

            if (_buffer.Count > 0)
            {
                Notifications.Add(_buffer[0]);
                _buffer.RemoveAt(0);
            }

            // Close window if there's nothing to show
            if (Notifications.Count < 1)
            {
                Hide();
            }
        }

        private void NotificationWindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Height != 0.0)
            {
                return;
            }

            var element = sender as Grid;
            RemoveNotification(Notifications.First(n => n.Id == int.Parse(element.Tag.ToString())));
        }
    }
}
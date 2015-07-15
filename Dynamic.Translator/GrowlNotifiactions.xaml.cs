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
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using Dynamic.Tureng.Translator.Model;

    #endregion

    public partial class GrowlNotifiactions
    {
        private static readonly byte MaxNotifications = byte.Parse(ConfigurationManager.AppSettings["MaxNotifications"]);
        private readonly Notifications buffer = new Notifications();
        private int count;
        public Notifications Notifications = new Notifications();

        public GrowlNotifiactions()
        {
            InitializeComponent();
            NotificationsControl.DataContext = Notifications;
        }

        public async Task AddNotification(Notification notification)
        {
            notification.Id = count++;
            if (Notifications.Count + 1 > MaxNotifications)
            {
                buffer.Add(notification);
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

            if (buffer.Count > 0)
            {
                Notifications.Add(buffer[0]);
                buffer.RemoveAt(0);
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
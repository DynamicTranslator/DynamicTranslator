using System;
using System.Linq;
using System.Speech.Synthesis;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

using DynamicTranslator.Configuration.Startup;
using DynamicTranslator.Extensions;

namespace DynamicTranslator.Wpf.ViewModel
{
    public partial class GrowlNotifications : IGrowlNotifications
    {
        public readonly Notifications Notifications;
        private readonly IApplicationConfiguration _applicationConfiguration;
        private readonly Notifications _buffer = new Notifications();
        public bool IsDisposed;
        private int _count;

        public GrowlNotifications(IApplicationConfiguration applicationConfiguration, Notifications notifications)
        {
            InitializeComponent();
            this._applicationConfiguration = applicationConfiguration;
            Notifications = notifications;
            _notificationsControl.DataContext = Notifications;
        }

        public event EventHandler OnDispose;

        public void Dispose()
        {
            if (IsDisposed)
                return;

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
                        _buffer.Add(notification);
                    else
                        Notifications.Add(notification);

                    if (Notifications.Count > 0 && !IsActive)
                        Show();
                },
                DispatcherPriority.Background);
        }

        public async Task AddNotificationAsync(Notification notification)
        {
            await Task.Run(() => AddNotification(notification));
        }

        public void RemoveNotification(Notification notification)
        {
            Dispatcher.InvokeAsync(
                () =>
                {
                    if (Notifications.Contains(notification))
                        Notifications.Remove(notification);

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
                return;

            var element = sender as Grid;
            RemoveNotification(Notifications.First(n => element != null && n.Id == int.Parse(element.Tag.ToString())));
        }

        private async void TextToSpeechButton_OnClick(object sender, RoutedEventArgs e)
        {
            await Task.Run(async
                () =>
                {
                    await Dispatcher.InvokeAsync(
                        () =>
                        {
                            var notification = ((FrameworkElement)sender).DataContext as Notification;
                            using (var synthesizer = new SpeechSynthesizer())
                            {
                                synthesizer.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Adult);
                                synthesizer.SetOutputToDefaultAudioDevice();
                                synthesizer.Volume = 100;
                                synthesizer.Rate = 0;

                                if (notification != null)
                                    synthesizer.Speak(notification.Title);
                            }
                        });
                });
        }
    }
}
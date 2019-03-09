using DynamicTranslator.Wpf.ViewModel;

namespace DynamicTranslator.Wpf
{
    public class GrowlNotifier : INotifier
    {
        private readonly GrowlNotifications _growlNotifications;

        public GrowlNotifier(GrowlNotifications growlNotifications)
        {
            _growlNotifications = growlNotifications;
        }

        public void AddNotification(string title, string imageUrl, string text)
        {
            _growlNotifications.AddNotification(new Notification { ImageUrl = imageUrl, Message = text, Title = title });
        }
    }
}

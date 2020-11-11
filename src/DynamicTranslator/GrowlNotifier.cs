namespace DynamicTranslator
{
    using Core;
    using ViewModel;

    public class GrowlNotifier : INotifier
    {
        readonly GrowlNotifications growlNotifications;

        public GrowlNotifier(GrowlNotifications growlNotifications)
        {
            this.growlNotifications = growlNotifications;
        }

        public void AddNotification(string title, string imageUrl, string text)
        {
            this.growlNotifications.AddNotification(new Notification
            {
                ImageUrl = imageUrl, Message = text, Title = title
            });
        }
    }
}
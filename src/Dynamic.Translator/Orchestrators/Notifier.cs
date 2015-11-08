namespace Dynamic.Translator.Orchestrators
{
    using System.Threading.Tasks;
    using Core.Dependency.Markers;
    using Core.Orchestrators;
    using Core.ViewModel;
    using Core.ViewModel.Interfaces;

    public class Notifier : INotifier, ITransientDependency
    {
        private readonly IGrowlNotifications growlNotifiactions;

        public Notifier(IGrowlNotifications growlNotifiactions)
        {
            this.growlNotifiactions = growlNotifiactions;
        }
        
        public void AddNotification(string title, string imageUrl, string text)
        {
            this.growlNotifiactions.AddNotification(new Notification {ImageUrl = imageUrl, Message = text, Title = title});
        }

        public async Task AddNotificationAsync(string title, string imageUrl, string text)
        {
            await this.growlNotifiactions.AddNotificationAsync(new Notification {ImageUrl = imageUrl, Message = text, Title = title});
        }
    }
}
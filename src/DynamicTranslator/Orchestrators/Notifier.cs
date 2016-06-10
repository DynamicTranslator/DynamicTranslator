#region using

using System;
using System.Threading.Tasks;

using DynamicTranslator.Core.Dependency.Markers;
using DynamicTranslator.Core.Orchestrators;
using DynamicTranslator.Core.ViewModel;
using DynamicTranslator.Core.ViewModel.Interfaces;

#endregion

namespace DynamicTranslator.Orchestrators
{
    public class Notifier : INotifier, ITransientDependency
    {
        private readonly IGrowlNotifications growlNotifiactions;

        public Notifier(IGrowlNotifications growlNotifiactions)
        {
            if (growlNotifiactions == null)
                throw new ArgumentNullException(nameof(growlNotifiactions));

            this.growlNotifiactions = growlNotifiactions;
        }

        public void AddNotification(string title, string imageUrl, string text)
        {
            growlNotifiactions.AddNotification(new Notification {ImageUrl = imageUrl, Message = text, Title = title});
        }

        public Task AddNotificationAsync(string title, string imageUrl, string text)
        {
            return growlNotifiactions.AddNotificationAsync(new Notification {ImageUrl = imageUrl, Message = text, Title = title});
        }
    }
}
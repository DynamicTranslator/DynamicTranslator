using System.Threading.Tasks;

namespace DynamicTranslator.Wpf.Orchestrators.Notification
{
    public interface INotifier
    {
        void AddNotification(string title, string imageUrl, string text);

        Task AddNotificationAsync(string title, string imageUrl, string text);
    }
}
namespace Dynamic.Translator.Core.Orchestrators
{
    using System.Threading.Tasks;

    public interface INotifier
    {
        void AddNotification(string title, string imageUrl, string text);
        Task AddNotificationAsync(string title, string imageUrl, string text);
    }
}
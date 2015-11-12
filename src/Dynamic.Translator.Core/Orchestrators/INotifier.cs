namespace Dynamic.Translator.Core.Orchestrators
{
    #region using

    using System.Threading.Tasks;

    #endregion

    public interface INotifier
    {
        void AddNotification(string title, string imageUrl, string text);
        Task AddNotificationAsync(string title, string imageUrl, string text);
    }
}
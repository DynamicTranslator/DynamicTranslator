namespace DynamicTranslator
{
    public interface INotifier
    {
        void AddNotification(string title, string imageUrl, string text);
    }
}
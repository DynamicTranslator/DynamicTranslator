namespace DynamicTranslator.Wpf.ViewModel
{
    public interface INotification
    {
        int Id { get; set; }

        string ImageUrl { get; set; }

        string Message { get; set; }

        string Title { get; set; }
    }
}
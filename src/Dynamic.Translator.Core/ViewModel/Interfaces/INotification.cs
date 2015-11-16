namespace DynamicTranslator.Core.ViewModel.Interfaces
{
    public interface INotification
    {
        int Id { get; set; }

        string ImageUrl { get; set; }

        string Message { get; set; }

        string Title { get; set; }
    }
}
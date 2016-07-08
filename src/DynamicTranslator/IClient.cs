namespace DynamicTranslator
{
    public interface IClient
    {
        string AppVersion { get; set; }

        string Id { get; set; }

        string MachineName { get; set; }
    }
}
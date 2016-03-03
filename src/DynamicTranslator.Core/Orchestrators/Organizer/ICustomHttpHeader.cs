namespace DynamicTranslator.Core.Orchestrators.Organizer
{
    public interface ICustomHttpHeader
    {
        string UserAgent { get; set; }
        string AcceptLangauge { get; set; }

    }
}

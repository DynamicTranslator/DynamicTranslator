namespace DynamicTranslator.Application.Orchestrators
{
    public interface ICustomHttpHeader
    {
        string UserAgent { get; set; }

        string AcceptLangauge { get; set; }
    }
}
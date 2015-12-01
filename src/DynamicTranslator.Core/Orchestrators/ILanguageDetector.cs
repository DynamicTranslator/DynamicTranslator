namespace DynamicTranslator.Core.Orchestrators
{
    #region using

    using System.Threading.Tasks;

    #endregion

    public interface ILanguageDetector
    {
        Task<string> DetectLanguage(string text);
    }
}
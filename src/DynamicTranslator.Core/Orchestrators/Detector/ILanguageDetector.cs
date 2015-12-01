namespace DynamicTranslator.Core.Orchestrators.Detector
{
    #region using

    using System.Threading.Tasks;

    #endregion

    public interface ILanguageDetector
    {
        Task<string> DetectLanguage(string text);
    }
}
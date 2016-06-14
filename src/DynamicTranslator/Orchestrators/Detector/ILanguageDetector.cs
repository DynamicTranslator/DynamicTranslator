using System.Threading.Tasks;

namespace DynamicTranslator.Orchestrators.Detector
{
    #region using

    

    #endregion

    public interface ILanguageDetector
    {
        Task<string> DetectLanguage(string text);
    }
}
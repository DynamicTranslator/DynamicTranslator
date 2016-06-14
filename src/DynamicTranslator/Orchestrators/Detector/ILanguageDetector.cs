using System.Threading.Tasks;

namespace DynamicTranslator.Orchestrators.Detector
{
    public interface ILanguageDetector
    {
        Task<string> DetectLanguage(string text);
    }
}
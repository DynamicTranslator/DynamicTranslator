using System.Threading.Tasks;

namespace DynamicTranslator.Wpf.Orchestrators.Detector
{
    public interface ILanguageDetector
    {
        Task<string> DetectLanguage(string text);
    }
}
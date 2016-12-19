using System.Threading.Tasks;

namespace DynamicTranslator.Application.Orchestrators.Detectors
{
    public interface ILanguageDetector
    {
        Task<string> DetectLanguage(string text);
    }
}

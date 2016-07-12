using System.Threading.Tasks;

namespace DynamicTranslator.Application
{
    public interface ILanguageDetector
    {
        Task<string> DetectLanguage(string text);
    }
}
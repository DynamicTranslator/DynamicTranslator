namespace DynamicTranslator.Orchestrators.Detector
{
    using System;
    using System.Threading.Tasks;
    using Core.Orchestrators;
    public class YandexLanguageDetector :ILanguageDetector
    {
        public Task<string> DetectLanguage(string text)
        {
            throw new NotImplementedException();
        }
    }
}
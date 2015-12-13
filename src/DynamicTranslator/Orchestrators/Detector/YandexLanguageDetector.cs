namespace DynamicTranslator.Orchestrators.Detector
{
    #region using

    using System;
    using System.Threading.Tasks;
    using Core.Orchestrators.Detector;

    #endregion

    public class YandexLanguageDetector : ILanguageDetector
    {
        public Task<string> DetectLanguage(string text)
        {
            throw new NotImplementedException();
        }
    }
}
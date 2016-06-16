using System.Collections.Generic;

namespace DynamicTranslator.Wpf.Orchestrators.Detector
{
    public interface ILanguageDetectorFactory
    {
        ICollection<ILanguageDetector> GetLanguageDetectors();
    }
}
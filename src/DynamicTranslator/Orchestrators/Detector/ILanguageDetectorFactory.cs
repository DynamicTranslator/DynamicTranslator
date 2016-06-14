using System.Collections.Generic;

namespace DynamicTranslator.Orchestrators.Detector
{
    public interface ILanguageDetectorFactory
    {
        ICollection<ILanguageDetector> GetLanguageDetectors();
    }
}
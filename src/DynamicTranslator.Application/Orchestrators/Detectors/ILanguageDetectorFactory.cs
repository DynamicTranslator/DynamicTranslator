using System.Collections.Generic;

namespace DynamicTranslator.Application.Orchestrators.Detectors
{
    public interface ILanguageDetectorFactory
    {
        ICollection<ILanguageDetector> GetLanguageDetectors();
    }
}

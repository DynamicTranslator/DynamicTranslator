using System.Collections.Generic;

namespace DynamicTranslator.Application
{
    public interface ILanguageDetectorFactory
    {
        ICollection<ILanguageDetector> GetLanguageDetectors();
    }
}
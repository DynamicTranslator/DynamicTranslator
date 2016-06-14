using System.Collections.Generic;

namespace DynamicTranslator.Orchestrators.Detector
{
    #region using

    

    #endregion

    public interface ILanguageDetectorFactory
    {
        ICollection<ILanguageDetector> GetLanguageDetectors();
    }
}
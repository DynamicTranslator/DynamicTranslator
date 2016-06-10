using System.Collections.Generic;

namespace DynamicTranslator.Core.Orchestrators.Detector
{
    #region using

    

    #endregion

    public interface ILanguageDetectorFactory
    {
        ICollection<ILanguageDetector> GetLanguageDetectors();
    }
}
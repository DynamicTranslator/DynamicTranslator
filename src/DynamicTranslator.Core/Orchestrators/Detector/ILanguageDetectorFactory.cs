namespace DynamicTranslator.Core.Orchestrators.Detector
{
    #region using

    using System.Collections.Generic;

    #endregion

    public interface ILanguageDetectorFactory
    {
        ICollection<ILanguageDetector> GetLanguageDetectors();
    }
}
namespace DynamicTranslator.Core.Orchestrators
{
    #region using

    using System.Collections.Generic;

    #endregion

    public interface ILanguageDetectorFactory
    {
        ICollection<ILanguageDetector> GetLanguageDetectors();
    }
}
namespace DynamicTranslator.Core.Service
{
    #region using

    using System.Collections.Generic;
    using Orchestrators;

    #endregion

    public interface IResultService 
    {
        ICollection<TranslateResult> Save(string key, ICollection<TranslateResult> translateResult);

        ICollection<TranslateResult> Get(string key);
    }
}
namespace DynamicTranslator.Core.DBReezeNoSQL.Repository
{
    #region using

    using System.Collections.Generic;
    using Orchestrators;

    #endregion

    public interface ITranslateResultRepository
    {
        ICollection<TranslateResult> GetTranslateResult(string key);

        ICollection<TranslateResult> SetTranslateResult(string key, ICollection<TranslateResult> result);
    }
}
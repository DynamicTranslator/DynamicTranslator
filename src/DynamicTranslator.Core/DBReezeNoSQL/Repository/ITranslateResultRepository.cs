namespace DynamicTranslator.Core.DBReezeNoSQL.Repository
{
    #region using

    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Orchestrators;

    #endregion

    public interface ITranslateResultRepository
    {
        ICollection<TranslateResult> GetTranslateResult(string key);

        Task<ICollection<TranslateResult>> GetTranslateResultAsync(string key);

        ICollection<TranslateResult> SetTranslateResult(string key, ICollection<TranslateResult> result);

        Task<ICollection<TranslateResult>> SetTranslateResultAsync(string key, ICollection<TranslateResult> result);
    }
}
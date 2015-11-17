namespace DynamicTranslator.Core.Service
{
    #region using

    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Orchestrators;

    #endregion

    public interface IResultService
    {
        ICollection<TranslateResult> Save(string key, ICollection<TranslateResult> translateResult);

        Task<ICollection<TranslateResult>> SaveAsync(string key, ICollection<TranslateResult> translateResult);

        ICollection<TranslateResult> Get(string key);

        Task<ICollection<TranslateResult>> GetAsync(string key);
    }
}
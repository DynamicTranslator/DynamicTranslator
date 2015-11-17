namespace DynamicTranslator.Core.Service
{
    #region using

    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Orchestrators;

    #endregion

    public interface IResultService
    {
        CompositeTranslateResult Save(string key, CompositeTranslateResult translateResult);

        Task<CompositeTranslateResult> SaveAsync(string key, CompositeTranslateResult translateResult);

        CompositeTranslateResult Get(string key);

        Task<CompositeTranslateResult> GetAsync(string key);
    }
}
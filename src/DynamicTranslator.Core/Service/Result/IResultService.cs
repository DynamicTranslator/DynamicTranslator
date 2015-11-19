namespace DynamicTranslator.Core.Service.Result
{
    #region using

    using System.Threading.Tasks;
    using Orchestrators;

    #endregion

    public interface IResultService
    {
        CompositeTranslateResult Save(string key, CompositeTranslateResult translateResult);

        CompositeTranslateResult SaveAndUpdateFrequency(string key, CompositeTranslateResult translateResult);

        Task<CompositeTranslateResult> SaveAsync(string key, CompositeTranslateResult translateResult);

        Task<CompositeTranslateResult> SaveAndUpdateFrequencyAsync(string key, CompositeTranslateResult translateResult);

        CompositeTranslateResult Get(string key);

        Task<CompositeTranslateResult> GetAsync(string key);
    }
}
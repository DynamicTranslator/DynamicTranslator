using System.Threading.Tasks;

using DynamicTranslator.Orchestrators.Model;

namespace DynamicTranslator.DBReezeNoSQL.Repository.TranslateResultRepository
{
    #region using

    

    #endregion

    public interface ITranslateResultRepository
    {
        CompositeTranslateResult GetTranslateResult(string key);

        Task<CompositeTranslateResult> GetTranslateResultAsync(string key);

        CompositeTranslateResult SetTranslateResult(string key, CompositeTranslateResult result);

        CompositeTranslateResult SetTranslateResultAndUpdateFrequency(string key, CompositeTranslateResult result);

        Task<CompositeTranslateResult> SetTranslateResultAndUpdateFrequencyAsync(string key, CompositeTranslateResult result);

        Task<CompositeTranslateResult> SetTranslateResultAsync(string key, CompositeTranslateResult result);
    }
}
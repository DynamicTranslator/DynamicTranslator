using System.Threading.Tasks;

using DynamicTranslator.Domain.Model;

namespace DynamicTranslator.Domain.Repository
{
    public interface ITranslateResultRepository
    {
        CompositeTranslateResult GetTranslateResult(string key);

        Task<CompositeTranslateResult> GetTranslateResultAsync(string key);

        CompositeTranslateResult SetTranslateResult(CompositeTranslateResult result);

        CompositeTranslateResult SetTranslateResultAndUpdateFrequency(CompositeTranslateResult result);

        Task<CompositeTranslateResult> SetTranslateResultAndUpdateFrequencyAsync(CompositeTranslateResult result);

        Task<CompositeTranslateResult> SetTranslateResultAsync(CompositeTranslateResult result);
    }
}
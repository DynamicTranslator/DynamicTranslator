using System.Threading.Tasks;

using Abp.Application.Services;

using DynamicTranslator.Orchestrators.Model;

namespace DynamicTranslator.Application.Result
{
    public interface IResultService : IApplicationService
    {
        CompositeTranslateResult Get(string key);

        Task<CompositeTranslateResult> GetAsync(string key);

        CompositeTranslateResult Save(CompositeTranslateResult translateResult);

        CompositeTranslateResult SaveAndUpdateFrequency(CompositeTranslateResult translateResult);

        Task<CompositeTranslateResult> SaveAndUpdateFrequencyAsync(CompositeTranslateResult translateResult);

        Task<CompositeTranslateResult> SaveAsync(CompositeTranslateResult translateResult);
    }
}
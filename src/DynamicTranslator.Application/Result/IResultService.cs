using System.Threading.Tasks;

using Abp.Application.Services;
using Abp.Runtime.Validation;

using DynamicTranslator.Domain.Model;

namespace DynamicTranslator.Application.Result
{
    public interface IResultService : IApplicationService
    {
        [DisableValidation]
        CompositeTranslateResult Get(string key);

        [DisableValidation]
        Task<CompositeTranslateResult> GetAsync(string key);

        [DisableValidation]
        CompositeTranslateResult Save(CompositeTranslateResult translateResult);

        [DisableValidation]
        CompositeTranslateResult SaveAndUpdateFrequency(CompositeTranslateResult translateResult);

        [DisableValidation]
        Task<CompositeTranslateResult> SaveAndUpdateFrequencyAsync(CompositeTranslateResult translateResult);

        [DisableValidation]
        Task<CompositeTranslateResult> SaveAsync(CompositeTranslateResult translateResult);
    }
}
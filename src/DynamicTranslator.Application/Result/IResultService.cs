using System.Threading.Tasks;

using Abp.Application.Services;

using DynamicTranslator.Domain.Model;

namespace DynamicTranslator.Application.Result
{
    public interface IResultService : IApplicationService
    {
        Task<CompositeTranslateResult> GetAsync(string key);

        Task<CompositeTranslateResult> SaveOrUpdateAsync(CompositeTranslateResult translateResult);
    }
}
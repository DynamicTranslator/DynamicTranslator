using System;
using System.Threading.Tasks;

using Abp.Application.Services;
using Abp.Domain.Uow;

using DynamicTranslator.Domain.Model;
using DynamicTranslator.Domain.Repository;

namespace DynamicTranslator.Application.Result
{
    public class ResultService : ApplicationService, IResultService
    {
        private readonly ITranslateResultRepository resultRepository;

        public ResultService(ITranslateResultRepository resultRepository)
        {
            this.resultRepository = resultRepository;
        }

        [UnitOfWork]
        public Task<CompositeTranslateResult> GetAsync(string key)
        {
            return resultRepository.GetAsync(key);
        }

        [UnitOfWork]
        public Task<CompositeTranslateResult> SaveOrUpdateAsync(CompositeTranslateResult translateResult)
        {
            var lastResult = resultRepository.FirstOrDefault(translateResult.Id);

            if (lastResult != null)
            {
                lastResult = translateResult
                    .SetResults(translateResult.Results)
                    .SetCreateDate(DateTime.Now)
                    .IncreaseFrequency();
            }
            else
            {
                lastResult = translateResult;
            }

            return resultRepository.InsertOrUpdateAsync(lastResult);
        }
    }
}
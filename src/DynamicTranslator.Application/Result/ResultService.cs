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
        private readonly ITranslateResultRepository _resultRepository;

        public ResultService(ITranslateResultRepository resultRepository)
        {
            _resultRepository = resultRepository;
        }

        [UnitOfWork]
        public Task<CompositeTranslateResult> GetAsync(string key)
        {
            return _resultRepository.GetAsync(key);
        }

        [UnitOfWork]
        public Task<CompositeTranslateResult> SaveOrUpdateAsync(CompositeTranslateResult translateResult)
        {
            CompositeTranslateResult lastResult = _resultRepository.FirstOrDefault(translateResult.Id);

            if (lastResult != null)
            {
                lastResult = translateResult
                    .SetResults(translateResult.Results)
                    .SetCreateDate(DateTime.Now)
                    .IncreaseFrequency();

                return _resultRepository.UpdateAsync(lastResult);
            }

            lastResult = translateResult;
            return _resultRepository.InsertAsync(lastResult);
        }
    }
}

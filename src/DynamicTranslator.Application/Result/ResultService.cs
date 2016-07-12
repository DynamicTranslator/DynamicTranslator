using System.Threading.Tasks;

using Abp.Domain.Uow;

using DynamicTranslator.Domain.Model;
using DynamicTranslator.Domain.Repository;

namespace DynamicTranslator.Application.Result
{
    public class ResultService : IResultService
    {
        private readonly ITranslateResultRepository resultRepository;

        public ResultService(ITranslateResultRepository resultRepository)
        {
            this.resultRepository = resultRepository;
        }

        [UnitOfWork]
        public CompositeTranslateResult Get(string key)
        {
            return resultRepository.GetTranslateResult(key);
        }

        [UnitOfWork]
        public async Task<CompositeTranslateResult> GetAsync(string key)
        {
            return await resultRepository.GetTranslateResultAsync(key);
        }

        [UnitOfWork]
        public CompositeTranslateResult Save(CompositeTranslateResult translateResult)
        {
            return resultRepository.SetTranslateResult(translateResult);
        }

        [UnitOfWork]
        public CompositeTranslateResult SaveAndUpdateFrequency(CompositeTranslateResult translateResult)
        {
            return resultRepository.SetTranslateResultAndUpdateFrequency(translateResult);
        }

        [UnitOfWork]
        public async Task<CompositeTranslateResult> SaveAndUpdateFrequencyAsync(CompositeTranslateResult translateResult)
        {
            return await resultRepository.SetTranslateResultAndUpdateFrequencyAsync(translateResult);
        }

        [UnitOfWork]
        public async Task<CompositeTranslateResult> SaveAsync(CompositeTranslateResult translateResult)
        {
            return await resultRepository.SetTranslateResultAsync(translateResult);
        }
    }
}
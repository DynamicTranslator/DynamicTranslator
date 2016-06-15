using System.Threading.Tasks;

using DynamicTranslator.DbReeze.DBReezeNoSQL.Repository.TranslateResultRepository;
using DynamicTranslator.Orchestrators.Model;

namespace DynamicTranslator.Application.Result
{
    public class ResultService : IResultService
    {
        private readonly ITranslateResultRepository resultRepository;

        public ResultService(ITranslateResultRepository resultRepository)
        {
            this.resultRepository = resultRepository;
        }

        public CompositeTranslateResult Get(string key)
        {
            return resultRepository.GetTranslateResult(key);
        }

        public async Task<CompositeTranslateResult> GetAsync(string key)
        {
            return await resultRepository.GetTranslateResultAsync(key);
        }

        public CompositeTranslateResult Save(string key, CompositeTranslateResult translateResult)
        {
            return resultRepository.SetTranslateResult(translateResult);
        }

        public CompositeTranslateResult SaveAndUpdateFrequency(string key, CompositeTranslateResult translateResult)
        {
            return resultRepository.SetTranslateResultAndUpdateFrequency(translateResult);
        }

        public async Task<CompositeTranslateResult> SaveAndUpdateFrequencyAsync(string key, CompositeTranslateResult translateResult)
        {
            return await resultRepository.SetTranslateResultAndUpdateFrequencyAsync(translateResult);
        }

        public async Task<CompositeTranslateResult> SaveAsync(string key, CompositeTranslateResult translateResult)
        {
            return await resultRepository.SetTranslateResultAsync(translateResult);
        }
    }
}
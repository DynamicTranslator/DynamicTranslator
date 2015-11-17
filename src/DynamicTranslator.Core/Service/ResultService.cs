namespace DynamicTranslator.Core.Service
{
    #region using

    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DBReezeNoSQL.Repository;
    using Dependency.Markers;
    using Orchestrators;

    #endregion

    public class ResultService : IResultService, ITransientDependency
    {
        private readonly ITranslateResultRepository resultRepository;

        public ResultService(ITranslateResultRepository resultRepository)
        {
            this.resultRepository = resultRepository;
        }

        public ICollection<TranslateResult> Save(string key, ICollection<TranslateResult> translateResult)
        {
            return resultRepository.SetTranslateResult(key, translateResult);
        }

        public async Task<ICollection<TranslateResult>> SaveAsync(string key, ICollection<TranslateResult> translateResult)
        {
            return await resultRepository.SetTranslateResultAsync(key, translateResult);
        }

        public ICollection<TranslateResult> Get(string key)
        {
            return resultRepository.GetTranslateResult(key);
        }

        public async Task<ICollection<TranslateResult>> GetAsync(string key)
        {
            return await resultRepository.GetTranslateResultAsync(key);
        }
    }
}
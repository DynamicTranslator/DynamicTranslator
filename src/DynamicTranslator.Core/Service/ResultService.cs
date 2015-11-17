namespace DynamicTranslator.Core.Service
{
    #region using

    using System.Collections.Generic;
    using DBReezeNoSQL.Repository;
    using Dependency.Markers;
    using Orchestrators;

    #endregion

    public class ResultService : IResultService , ITransientDependency
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

        public ICollection<TranslateResult> Get(string key)
        {
            return resultRepository.GetTranslateResult(key);
        }
    }
}
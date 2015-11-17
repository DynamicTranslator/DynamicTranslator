namespace DynamicTranslator.Core.DBReezeNoSQL.Repository
{
    #region using

    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Orchestrators;

    #endregion

    public class TranslateResultRepository : DBReezeRepositoryBase<CompositeTranslateResult, string>, ITranslateResultRepository
    {
        public TranslateResultRepository(ITransactionProvider transactionProvider) : base(transactionProvider)
        {
        }

        public CompositeTranslateResult GetTranslateResult(string key)
        {
            return Get(key);
        }

        public async Task<CompositeTranslateResult> GetTranslateResultAsync(string key)
        {
            return await GetAsync(key);
        }

        public CompositeTranslateResult SetTranslateResult(string key, CompositeTranslateResult result)
        {
            return Insert(result, key);
        }

        public async Task<CompositeTranslateResult> SetTranslateResultAsync(string key, CompositeTranslateResult result)
        {
            return await InsertAsync(result, key);
        }
    }
}
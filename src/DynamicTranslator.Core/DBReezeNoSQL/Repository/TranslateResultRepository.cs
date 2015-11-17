namespace DynamicTranslator.Core.DBReezeNoSQL.Repository
{
    #region using

    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Orchestrators;

    #endregion

    public class TranslateResultRepository : DBReezeRepositoryBase<ICollection<TranslateResult>, string>, ITranslateResultRepository
    {
        public TranslateResultRepository(ITransactionProvider transactionProvider) : base(transactionProvider)
        {
        }

        public ICollection<TranslateResult> GetTranslateResult(string key)
        {
            return Get(key);
        }

        public async Task<ICollection<TranslateResult>> GetTranslateResultAsync(string key)
        {
            return await GetAsync(key);
        }

        public ICollection<TranslateResult> SetTranslateResult(string key, ICollection<TranslateResult> result)
        {
            return Insert(result, key);
        }

        public async Task<ICollection<TranslateResult>> SetTranslateResultAsync(string key, ICollection<TranslateResult> result)
        {
            return await InsertAsync(result, key);
        }
    }
}
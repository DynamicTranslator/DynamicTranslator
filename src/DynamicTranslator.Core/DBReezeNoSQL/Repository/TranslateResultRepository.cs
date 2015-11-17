namespace DynamicTranslator.Core.DBReezeNoSQL.Repository
{
    #region using

    using System.Collections.Generic;
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

        public ICollection<TranslateResult> SetTranslateResult(string key, ICollection<TranslateResult> result)
        {
            return Insert(result, key);
        }
    }
}
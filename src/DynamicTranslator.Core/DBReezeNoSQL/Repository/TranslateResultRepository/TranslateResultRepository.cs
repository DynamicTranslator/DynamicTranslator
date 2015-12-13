namespace DynamicTranslator.Core.DBReezeNoSQL.Repository.TranslateResultRepository
{
    #region using

    using System;
    using System.Threading.Tasks;
    using Orchestrators.Model;

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

        public CompositeTranslateResult SetTranslateResultAndUpdateFrequency(string key, CompositeTranslateResult result)
        {
            var translateResult = Get(key);

            if (translateResult != null)
            {
                translateResult
                    .SetResults(result.Results)
                    .SetCreateDate(DateTime.Now)
                    .IncreaseFrequency();
            }
            else
            {
                translateResult = result;
            }

            return Insert(translateResult, key);
        }

        public async Task<CompositeTranslateResult> SetTranslateResultAndUpdateFrequencyAsync(string key, CompositeTranslateResult result)
        {
            var translateResult = await GetAsync(key);

            if (translateResult != null)
            {
                translateResult
                    .SetResults(result.Results)
                    .SetCreateDate(DateTime.Now)
                    .IncreaseFrequency();
            }
            else
            {
                translateResult = result;
            }

            return await InsertAsync(translateResult, key);
        }

        public async Task<CompositeTranslateResult> SetTranslateResultAsync(string key, CompositeTranslateResult result)
        {
            return await InsertAsync(result, key);
        }
    }
}
using System;
using System.Threading.Tasks;

using DynamicTranslator.Domain.Model;
using DynamicTranslator.Domain.Repository;

namespace DynamicTranslator.DbReeze.DBReezeNoSQL.Repository.TranslateResultRepository
{
    public class TranslateResultRepository : DBReezeRepositoryBase<CompositeTranslateResult, string>, ITranslateResultRepository
    {
        public TranslateResultRepository(ITransactionProvider transactionProvider) : base(transactionProvider) {}

        public CompositeTranslateResult GetTranslateResult(string key)
        {
            return Get(key);
        }

        public Task<CompositeTranslateResult> GetTranslateResultAsync(string key)
        {
            return GetAsync(key);
        }

        public CompositeTranslateResult SetTranslateResult(CompositeTranslateResult result)
        {
            return Insert(result);
        }

        public CompositeTranslateResult SetTranslateResultAndUpdateFrequency(CompositeTranslateResult result)
        {
            var translateResult = FirstOrDefault(result.Id);

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

            return Insert(translateResult);
        }

        public async Task<CompositeTranslateResult> SetTranslateResultAndUpdateFrequencyAsync(CompositeTranslateResult result)
        {
            var translateResult = await FirstOrDefaultAsync(result.Id);

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

            return await InsertAsync(translateResult);
        }

        public Task<CompositeTranslateResult> SetTranslateResultAsync(CompositeTranslateResult result)
        {
            return InsertAsync(result);
        }
    }
}
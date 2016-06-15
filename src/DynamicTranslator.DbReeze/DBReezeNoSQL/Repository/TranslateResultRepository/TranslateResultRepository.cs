using System;
using System.Threading.Tasks;

using DynamicTranslator.Orchestrators.Model;

namespace DynamicTranslator.DbReeze.DBReezeNoSQL.Repository.TranslateResultRepository
{
    public class TranslateResultRepository : DBReezeRepositoryBase<CompositeTranslateResult, string>, ITranslateResultRepository
    {
        public CompositeTranslateResult GetTranslateResult(string key)
        {
            return Get(key);
        }

        public async Task<CompositeTranslateResult> GetTranslateResultAsync(string key)
        {
            return await GetAsync(key);
        }

        public CompositeTranslateResult SetTranslateResult(CompositeTranslateResult result)
        {
            return Insert(result);
        }

        public CompositeTranslateResult SetTranslateResultAndUpdateFrequency(CompositeTranslateResult result)
        {
            var translateResult = Get(result.Id);

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
            var translateResult = await GetAsync(result.Id);

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

        public async Task<CompositeTranslateResult> SetTranslateResultAsync(CompositeTranslateResult result)
        {
            return await InsertAsync(result);
        }
    }
}
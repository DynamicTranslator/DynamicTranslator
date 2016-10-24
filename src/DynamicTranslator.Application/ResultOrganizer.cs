using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Abp.Dependency;

using DynamicTranslator.Application.Orchestrators;
using DynamicTranslator.Application.Result;
using DynamicTranslator.Domain.Model;

namespace DynamicTranslator.Application
{
    public class ResultOrganizer : IResultOrganizer, ITransientDependency
    {
        private readonly IResultService _resultService;

        public ResultOrganizer(IResultService resultService)
        {
            _resultService = resultService;
        }

        public Task<Maybe<string>> OrganizeResult(ICollection<TranslateResult> findedMeans, string currentString)
        {
            var mean = new StringBuilder();
            foreach (TranslateResult result in findedMeans.Where(result => result.IsSuccess))
            {
                mean.AppendLine(result.ResultMessage.DefaultIfEmpty(string.Empty).First());
            }

            if (!string.IsNullOrEmpty(mean.ToString()))
            {
                List<string> means = mean.ToString().Split('\r')
                                         .Select(x => x.Trim().ToLower())
                                         .Where(s => (s != string.Empty) && (s != currentString.Trim()) && (s != "Translation"))
                                         .Distinct()
                                         .ToList();

                mean.Clear();
                means.ForEach(m => mean.AppendLine("* " + m.ToLower()));
                _resultService.SaveOrUpdateAsync(new CompositeTranslateResult(currentString, 1, findedMeans, DateTime.Now));
                return Task.FromResult(new Maybe<string>(mean.ToString()));
            }

            return Task.FromResult(new Maybe<string>());
        }
    }
}

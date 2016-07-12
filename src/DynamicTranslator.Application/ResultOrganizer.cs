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
        private readonly IResultService resultService;

        public ResultOrganizer(IResultService resultService)
        {
            this.resultService = resultService;
        }

        public Task<Maybe<string>> OrganizeResult(ICollection<TranslateResult> findedMeans, string currentString)
        {
            var mean = new StringBuilder();
            foreach (var result in findedMeans.Where(result => result.IsSucess))
            {
                mean.AppendLine(result.ResultMessage.DefaultIfEmpty(string.Empty).First());
            }

            if (!string.IsNullOrEmpty(mean.ToString()))
            {
                var means = mean.ToString().Split('\r')
                                .Select(x => x.Trim().ToLower())
                                .Where(s => s != string.Empty && s != currentString.Trim() && s != "Translation")
                                .Distinct()
                                .ToList();

                mean.Clear();
                means.ForEach(m => mean.AppendLine("* " + m.ToLower()));
                resultService.SaveAndUpdateFrequencyAsync(new CompositeTranslateResult(currentString, 1, findedMeans, DateTime.Now));
                return Task.FromResult(new Maybe<string>(mean.ToString()));
            }

            return Task.FromResult(new Maybe<string>());
        }
    }
}
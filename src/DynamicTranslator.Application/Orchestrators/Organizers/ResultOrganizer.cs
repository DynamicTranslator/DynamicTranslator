using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Abp.Dependency;

using DynamicTranslator.Application.Model;
using DynamicTranslator.Constants;

namespace DynamicTranslator.Application.Orchestrators.Organizers
{
    public class ResultOrganizer : IResultOrganizer, ITransientDependency
    {
        public Task<Maybe<string>> OrganizeResult(ICollection<TranslateResult> findedMeans, string currentString, out Maybe<string> failedResults)
        {
            Maybe<string> succeededResults = Organize(findedMeans, currentString, true);

            failedResults = Organize(findedMeans, currentString, false);

            return Task.FromResult(succeededResults);
        }

        private Maybe<string> Organize(ICollection<TranslateResult> findedMeans, string currentString, bool isSucceeded)
        {
            var mean = new StringBuilder();
            IEnumerable<TranslateResult> results = findedMeans.Where(result => result.IsSuccess == isSucceeded);

            foreach (TranslateResult result in results)
            {
                mean.AppendLine(result.ResultMessage.DefaultIfEmpty(string.Empty).First());
            }

            if (!string.IsNullOrEmpty(mean.ToString()))
            {
                List<string> means = mean.ToString()
                                         .Split('\r')
                                         .Select(x => x.Trim().ToLower())
                                         .Where(s => s != string.Empty && s != currentString.Trim() && s != "Translation")
                                         .Distinct()
                                         .ToList();

                mean.Clear();
                means.ForEach(m => mean.AppendLine($"{Titles.Asterix} {m.ToLower()}"));

                return new Maybe<string>(mean.ToString());
            }

            return new Maybe<string>();
        }
    }
}

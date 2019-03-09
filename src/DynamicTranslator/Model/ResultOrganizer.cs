using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicTranslator.Model
{
    public class ResultOrganizer
    {
        public string OrganizeResult(ICollection<TranslateResult> foundMeans, string currentString,
            out string failedResults)
        {
            string succeededResults = Organize(foundMeans, currentString, true);

            failedResults = Organize(foundMeans, currentString, false);

            return succeededResults;
        }

        private string Organize(ICollection<TranslateResult> foundedMeans, string currentString, bool isSucceeded)
        {
            var mean = new StringBuilder();
            IEnumerable<TranslateResult> results = foundedMeans.Where(result => result.IsSuccess == isSucceeded);

            foreach (TranslateResult result in results)
            {
                mean.AppendLine(result.ResultMessage);
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

                return (mean.ToString());
            }

            return string.Empty;
        }
    }
}
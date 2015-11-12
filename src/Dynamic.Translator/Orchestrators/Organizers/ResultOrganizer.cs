namespace Dynamic.Translator.Orchestrators.Organizers
{
    #region using

    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Core;
    using Core.Orchestrators;

    #endregion

    public class ResultOrganizer : IResultOrganizer
    {
        public async Task<Maybe<string>> OrganizeResult(ICollection<TranslateResult> findedMeans, string currentString)
        {
            return await Task.Run(() =>
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
                    return new Maybe<string>(mean.ToString());
                }

                return new Maybe<string>();
            });
        }
    }
}
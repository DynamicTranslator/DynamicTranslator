namespace Dynamic.Translator.Orchestrators.Organizers
{
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using Core;
    using Core.Dependency.Markers;
    using Core.Orchestrators;
    using Core.ViewModel.Constants;
    using HtmlAgilityPack;

    public class TurengMeanOrganizer : IMeanOrganizer, ITransientDependency
    {
        public TranslatorType TranslatorType => TranslatorType.TURENG;

        public async Task<Maybe<string>> OrganizeMean(string text)
        {
            if (text == null) return new Maybe<string>();

            var result = text;
            var output = new StringBuilder();
            var doc = new HtmlDocument();
            var decoded = WebUtility.HtmlDecode(result);
            doc.LoadHtml(decoded);
            if (!result.Contains("table") || doc.DocumentNode.SelectSingleNode("//table") == null)
                return new Maybe<string>();

            foreach (var table in doc.DocumentNode.SelectNodes("//table"))
            {
                foreach (var row in table.SelectNodes("tr").AsParallel())
                {
                    var space = false;
                    var i = 0;
                    foreach (var cell in row.SelectNodes("th|td").Descendants("a").AsParallel())
                    {
                        var word = cell.InnerHtml.ToString(CultureInfo.CurrentCulture);
                        space = true;
                        i++;
                        if (i <= 1) continue;
                        if (output.ToString().Contains(word))
                        {
                            space = false;
                            continue;
                        }
                        output.Append(cell.Id + " " + word);
                    }
                    if (!space) continue;
                    output.AppendLine();
                }
                break;
            }

            return new Maybe<string>(output.ToString().ToLower().Trim());
        }
    }
}
#region using

using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using DynamicTranslator.Orchestrators.Model;
using DynamicTranslator.ViewModel.Constants;

using HtmlAgilityPack;

#endregion

namespace DynamicTranslator.Wpf.Orchestrators.Organizers
{
    public class TurengMeanOrganizer : AbstractMeanOrganizer
    {
        public override TranslatorType TranslatorType => TranslatorType.Tureng;

        public override async Task<Maybe<string>> OrganizeMean(string text, string fromLanguageExtension)
        {
            return await Task.Run(() =>
            {
                if (text == null) return new Maybe<string>();

                var result = text;
                var output = new StringBuilder();
                var doc = new HtmlDocument();
                var decoded = WebUtility.HtmlDecode(result);
                doc.LoadHtml(decoded);

                if (!result.Contains("table") || doc.DocumentNode.SelectSingleNode("//table") == null)
                    return new Maybe<string>();

                (from x in doc.DocumentNode.Descendants()
                 where x.Name == "table"
                 from y in x.Descendants().AsParallel()
                 where y.Name == "tr"
                 from z in y.Descendants().AsParallel()
                 where (z.Name == "th" || z.Name == "td") && z.GetAttributeValue("lang", string.Empty) == (fromLanguageExtension == "tr" ? "en" : "tr")
                 from t in z.Descendants().AsParallel()
                 where t.Name == "a"
                 select t.InnerHtml)
                    .AsParallel()
                    .ToList()
                    .ForEach(mean => output.AppendLine(mean));

                return new Maybe<string>(output.ToString().ToLower().Trim());
            });
        }
    }
}
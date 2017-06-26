using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using DynamicTranslator.Application.Orchestrators.Organizers;
using DynamicTranslator.Constants;
using DynamicTranslator.Extensions;

using HtmlAgilityPack;

namespace DynamicTranslator.Application.WordReference.Orchestration
{
    public class WordReferenceMeanOrganizer : AbstractMeanOrganizer
    {
        public override TranslatorType TranslatorType => TranslatorType.WordReference;

        public override Task<Maybe<string>> OrganizeMean(string text, string fromLanguageExtension)
        {
            if (text == null)
            {
                return Task.FromResult(new Maybe<string>());
            }

            string result = text;
            var output = new StringBuilder();
            var doc = new HtmlDocument();
            string decoded = WebUtility.HtmlDecode(result);
            doc.LoadHtml(decoded);

            if (!result.Contains("table") || doc.DocumentNode.SelectSingleNode("//table") == null)
            {
                return Task.FromResult(new Maybe<string>());
            }

            (from x in doc.DocumentNode.Descendants()
             where x.Name == "table"
             from y in x.Descendants().AsParallel()
             where y.Name == "tr"
             from z in y.Descendants().AsParallel()
             where z.Name == "th" || z.Name == "td"
             from t in z.Descendants().AsParallel()
             where t.GetAttributeValue("class", "").Contains("ToWrd")
             select t.InnerHtml)
                .Skip(1)
                .AsParallel()
                .ToList()
                .ForEach(mean => output.AppendLine(mean.StripTagsRegexCompiled()));

            return Task.FromResult(new Maybe<string>(output.ToString().ToLower().Trim()));
        }
    }
}

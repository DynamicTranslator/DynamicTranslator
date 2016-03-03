#region using

using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DynamicTranslator.Core.Extensions;
using DynamicTranslator.Core.Orchestrators.Model;
using DynamicTranslator.Core.ViewModel.Constants;
using HtmlAgilityPack;

#endregion

namespace DynamicTranslator.Orchestrators.Organizers
{
    public class ZarganMeanOrganizer : AbstractMeanOrganizer
    {
        public override TranslatorType TranslatorType => TranslatorType.Zargan;

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

                (from x in doc.DocumentNode.Descendants()
                 where x.Name == "div" && x.GetAttributeValue("class", string.Empty) == "numberedList"
                 select x.InnerHtml)
                    .AsParallel()
                    .ToList()
                    .ForEach(mean => output.AppendLine(mean.StripTagsCharArray()));

                return new Maybe<string>(output.ToString().ToLower().Trim());
            });
        }
    }
}
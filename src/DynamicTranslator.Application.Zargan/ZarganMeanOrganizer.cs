using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using DynamicTranslator.Constants;
using DynamicTranslator.Extensions;

using HtmlAgilityPack;

namespace DynamicTranslator.Application.Zargan
{
    public class ZarganMeanOrganizer : AbstractMeanOrganizer
    {
        public override TranslatorType TranslatorType => TranslatorType.Zargan;

        public override Task<Maybe<string>> OrganizeMean(string text, string fromLanguageExtension)
        {
            if (text == null) return Task.FromResult(new Maybe<string>());

            var result = text;
            var output = new StringBuilder();
            var doc = new HtmlDocument();
            var decoded = WebUtility.HtmlDecode(result);
            doc.LoadHtml(decoded);

            var nodesToDelete = new List<HtmlNode>();
            if (doc.DocumentNode.SelectSingleNode("//div[@class='read-more-content']") != null)
                nodesToDelete.AddRange(doc.DocumentNode.SelectNodes("//div[@class='read-more-content']").ToList());

            if (doc.DocumentNode.SelectSingleNode("//span[@class='red']") != null)
                nodesToDelete.AddRange(doc.DocumentNode.SelectNodes("//span[@class='red']").ToList());

            if (doc.DocumentNode.SelectSingleNode("//a[@class='soundButton']") != null)
                nodesToDelete.AddRange(doc.DocumentNode.SelectNodes("//a[@class='soundButton']").ToList());

            if (doc.DocumentNode.SelectSingleNode("//i") != null)
                nodesToDelete.AddRange(doc.DocumentNode.SelectNodes("//i").ToList());

            if (doc.DocumentNode.SelectSingleNode("//b") != null)
                nodesToDelete.AddRange(doc.DocumentNode.SelectNodes("//b").ToList());

            nodesToDelete.AsParallel().ToList().ForEach(node => node.Remove());

            if (fromLanguageExtension != "tr")
            {
                (from x in doc.DocumentNode.Descendants()
                 where x.GetAttributeValue("id", string.Empty) == "section-1"
                 from y in x.Descendants().AsParallel()
                 where y.Name == "ol"
                 from z in y.Descendants().AsParallel()
                 where z.Name == "li"
                 select z.InnerHtml)
                    .AsParallel()
                    .ToList()
                    .ForEach(mean => output.AppendLine(mean.StripTagsCharArray()));
            }
            else
            {
                (from x in doc.DocumentNode.Descendants()
                 where x.GetAttributeValue("id", string.Empty) == "section-2"
                 from y in x.Descendants().AsParallel()
                 where y.Name == "ol"
                 from z in y.Descendants().AsParallel()
                 where z.Name == "li"
                 from g in z.Descendants().AsParallel()
                 where g.Name == "a"
                 select g.InnerHtml).AsParallel()
                                    .ToList()
                                    .ForEach(mean => output.AppendLine(mean.StripTagsCharArray()));
            }

            return Task.FromResult(new Maybe<string>(output.ToString().ToLower().Trim()));
        }
    }
}
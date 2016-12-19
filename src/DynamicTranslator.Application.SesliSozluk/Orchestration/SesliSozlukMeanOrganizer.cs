using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Abp.Dependency;

using DynamicTranslator.Application.Orchestrators.Organizers;
using DynamicTranslator.Constants;
using DynamicTranslator.Extensions;

using HtmlAgilityPack;

namespace DynamicTranslator.Application.SesliSozluk.Orchestration
{
    public class SesliSozlukMeanOrganizer : AbstractMeanOrganizer, IMeanOrganizer, ITransientDependency
    {
        public override TranslatorType TranslatorType => TranslatorType.SesliSozluk;

        public override Task<Maybe<string>> OrganizeMean(string text, string fromLanguageExtension)
        {
            var output = new StringBuilder();

            var document = new HtmlDocument();
            document.LoadHtml(text);

            (from x in document.DocumentNode.Descendants()
             where x.Name == "pre"
             from y in x.Descendants()
             where y.Name == "ol"
             from z in y.Descendants()
             where z.Name == "li"
             select z.InnerHtml)
                .AsParallel()
                .ToList()
                .ForEach(mean => output.AppendLine(mean));

            if (string.IsNullOrEmpty(output.ToString()))
            {
                (from x in document.DocumentNode.Descendants()
                 where x.Name == "pre"
                 from y in x.Descendants()
                 where y.Name == "span"
                 select y.InnerHtml)
                    .AsParallel()
                    .ToList()
                    .ForEach(mean => output.AppendLine(mean.StripTagsCharArray()));
            }

            return Task.FromResult(new Maybe<string>(output.ToString()));
        }
    }
}

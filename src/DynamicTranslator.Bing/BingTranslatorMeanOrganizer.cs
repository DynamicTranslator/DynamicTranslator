using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DynamicTranslator.Application;
using DynamicTranslator.Constants;
using DynamicTranslator.Extensions;

namespace DynamicTranslator.Bing
{
    public class BingTranslatorMeanOrganizer : AbstractMeanOrganizer
    {
        public override TranslatorType TranslatorType => TranslatorType.Bing;

        public override Task<Maybe<string>> OrganizeMean(string text, string fromLanguageExtension)
        {
            var means = new StringBuilder();

            var response = text.DeserializeAs<BingTranslatorResponse>();
            if (response.Translations.Any())
            {
                if (response.Translations.ContainsKey("Bing"))
                {
                    means.AppendLine(response.Translations["Bing"]);
                }
            }

            return Task.FromResult(new Maybe<string>(means.ToString()));
        }
    }
}
#region using

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicTranslator.Core.Orchestrators.Model;
using DynamicTranslator.Core.Orchestrators.Model.Bing;
using DynamicTranslator.Core.ViewModel.Constants;
using Newtonsoft.Json;

#endregion

namespace DynamicTranslator.Orchestrators.Organizers
{
    public class BingTranslatorMeanOrganizer : AbstractMeanOrganizer
    {
        public override TranslatorType TranslatorType => TranslatorType.Bing;

        public override async Task<Maybe<string>> OrganizeMean(string text, string fromLanguageExtension)
        {
            return await Task.Run(() =>
            {
                var means = new StringBuilder();

                var response = JsonConvert.DeserializeObject<BingTranslatorResponse>(text);
                if (response.Translations.Any())
                {
                    if (response.Translations.ContainsKey("Bing"))
                        means.AppendLine(response.Translations["Bing"]);
                }

                return new Maybe<string>(means.ToString());
            });
        }
    }
}

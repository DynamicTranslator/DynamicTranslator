using System.Collections.Generic;
using System.Threading.Tasks;

using DynamicTranslator.Extensions;
using DynamicTranslator.Orchestrators.Model;
using DynamicTranslator.ViewModel.Constants;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DynamicTranslator.Wpf.Orchestrators.Organizers
{
    public class GoogleTranslateMeanOrganizer : AbstractMeanOrganizer
    {
        public override TranslatorType TranslatorType => TranslatorType.Google;

        public override async Task<Maybe<string>> OrganizeMean(string text, string fromLanguageExtension)
        {
            return await Task.Run(() =>
            {
                var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(text);
                var arrayTree = result["sentences"] as JArray;
                var output = arrayTree.GetFirstValueInArrayGraph<string>();
                return new Maybe<string>(output);
            });
        }
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;

using DynamicTranslator.Constants;
using DynamicTranslator.Extensions;
using DynamicTranslator.Orchestrators.Model;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DynamicTranslator.Wpf.Orchestrators.Organizers
{
    public class GoogleTranslateMeanOrganizer : AbstractMeanOrganizer
    {
        public override TranslatorType TranslatorType => TranslatorType.Google;

        public override Task<Maybe<string>> OrganizeMean(string text, string fromLanguageExtension)
        {
            var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(text);
            var arrayTree = result["sentences"] as JArray;
            var output = arrayTree.GetFirstValueInArrayGraph<string>();
            return Task.FromResult(new Maybe<string>(output));
        }
    }
}
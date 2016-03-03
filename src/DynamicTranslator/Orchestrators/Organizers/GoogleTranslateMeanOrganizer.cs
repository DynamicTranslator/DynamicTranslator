#region using

using System.Collections.Generic;
using System.Threading.Tasks;
using DynamicTranslator.Core.Extensions;
using DynamicTranslator.Core.Orchestrators.Model;
using DynamicTranslator.Core.ViewModel.Constants;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

namespace DynamicTranslator.Orchestrators.Organizers
{
    public class GoogleTranslateMeanOrganizer : AbstractMeanOrganizer
    {
        public override TranslatorType TranslatorType => TranslatorType.GOOGLE;

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

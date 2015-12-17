namespace DynamicTranslator.Orchestrators.Organizers
{
    #region using

    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Extensions;
    using Core.Orchestrators.Model;
    using Core.Orchestrators.Organizer;
    using Core.ViewModel.Constants;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    #endregion

    public class GoogleTranslateMeanOrganizer : IMeanOrganizer
    {
        public TranslatorType TranslatorType => TranslatorType.GOOGLE;

        public async Task<Maybe<string>> OrganizeMean(string text)
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
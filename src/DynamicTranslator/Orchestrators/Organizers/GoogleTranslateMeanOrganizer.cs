namespace DynamicTranslator.Orchestrators.Organizers
{
    #region using

    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;
    using Core.Orchestrators;
    using Core.ViewModel.Constants;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    #endregion

    public class GoogleTranslateMeanOrganizer : IMeanOrganizer
    {
        public async Task<Maybe<string>> OrganizeMean(string text)
        {
            return await Task.Run(() =>
            {
                var arrayTree = JsonConvert.DeserializeObject(text) as JArray;
                var output = arrayTree.GetFirstValueInArrayGraph<string>();

                return new Maybe<string>(output);
            }).ConfigureAwait(false);
        }

        public TranslatorType TranslatorType => TranslatorType.GOOGLE;
    }
}
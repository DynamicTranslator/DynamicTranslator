namespace Dynamic.Translator.Orchestrators.Organizers
{
    using System.Threading.Tasks;
    using Core;
    using Core.Orchestrators;
    using Core.ViewModel.Constants;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class GoogleTranslateMeanOrganizer : IMeanOrganizer
    {
        public async Task<Maybe<string>> OrganizeMean(string text)
        {
            return await Task.Run(() =>
            {
                var obj = JsonConvert.DeserializeObject(text) as JArray;
                var output = obj?.First.First.First.Value<string>();

                return new Maybe<string>(output);
            });
        }

        public TranslatorType TranslatorType => TranslatorType.GOOGLE;
    }
}
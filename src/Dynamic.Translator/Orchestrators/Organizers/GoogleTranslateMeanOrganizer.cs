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
                var output = GetFirstElementSafely(obj);

                return new Maybe<string>(output);
            });
        }

        public TranslatorType TranslatorType => TranslatorType.GOOGLE;

        private string GetFirstElementSafely(JArray obj)
        {
            if (obj.HasValues)
            {
                if (obj.First.HasValues)
                {
                    if (obj.First.First.HasValues)
                    {
                        if (obj.First.First.First.HasValues)
                            return obj.First.First.First.Value<string>();
                    }
                }
            }

            return string.Empty;
        }
    }
}
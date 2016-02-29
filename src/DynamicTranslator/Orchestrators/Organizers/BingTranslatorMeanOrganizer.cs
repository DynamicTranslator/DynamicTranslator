#region using

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicTranslator.Core.Orchestrators.Model;
using DynamicTranslator.Core.Orchestrators.Model.Bing;
using DynamicTranslator.Core.Orchestrators.Organizer;
using DynamicTranslator.Core.ViewModel.Constants;
using Newtonsoft.Json;

#endregion

namespace DynamicTranslator.Orchestrators.Organizers
{
    public class BingTranslatorMeanOrganizer : IMeanOrganizer
    {
        public TranslatorType TranslatorType => TranslatorType.BING;

        public async Task<Maybe<string>> OrganizeMean(string text)
        {
            return await Task.Run(
                () =>
                {
                    var response = JsonConvert.DeserializeObject<BingTranslatorResponse>(text);

                    var means = new StringBuilder();

                    if (response.Translations.Any())
                    {
                        if (response.Translations.ContainsKey("Bing"))
                            means.AppendLine(response.Translations["Bing"]);

                        //if (response.WordByWord.Any())
                        //{
                        //    var innerMeans = response.WordByWord.Select(x => x.Description.Split(',')).ToList();
                        //    means.AppendLine(FillMean(innerMeans).ToString());
                        //}
                    }

                    return new Maybe<string>(means.ToString());
                });
        }

        private StringBuilder FillMean(List<string[]> rawMeans)
        {
            StringBuilder localBuilder = new StringBuilder();
            rawMeans
                .AsParallel()
                .ToList()
                .ForEach(
                    x =>
                    {
                        x.AsParallel()
                         .ToList()
                         .ForEach(
                             s => { localBuilder.AppendLine(s.Replace("...", string.Empty)); });
                    });
            return localBuilder;
        }
    }
}

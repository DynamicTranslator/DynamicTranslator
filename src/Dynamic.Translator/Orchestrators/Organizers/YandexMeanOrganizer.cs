namespace Dynamic.Translator.Orchestrators.Organizers
{
    using System.Threading.Tasks;
    using System.Xml;
    using Core;
    using Core.Orchestrators;
    using Core.ViewModel.Constants;

    public class YandexMeanOrganizer : IMeanOrganizer
    {
        public async Task<Maybe<string>> OrganizeMean(string text)
        {
            return await Task.Run(() =>
            {
                if (text == null) return new Maybe<string>();

                var doc = new XmlDocument();
                doc.LoadXml(text);
                var node = doc.SelectSingleNode("//Translation/text");
                var output = node?.InnerText ?? "!!! An error occured";

                return new Maybe<string>(output.ToLower().Trim());
            });
        }

        public TranslatorType TranslatorType => TranslatorType.YANDEX;
    }
}
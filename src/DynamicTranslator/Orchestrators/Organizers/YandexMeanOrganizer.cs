namespace DynamicTranslator.Orchestrators.Organizers
{
    #region using

    using System.Threading.Tasks;
    using System.Xml;
    using Core;
    using Core.Orchestrators;
    using Core.ViewModel.Constants;

    #endregion

    public class YandexMeanOrganizer : IMeanOrganizer
    {
        public Task<Maybe<string>> OrganizeMean(string text)
        {
            return Task.Run(() =>
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
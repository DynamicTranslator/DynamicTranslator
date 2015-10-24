namespace Dynamic.Translator.Orchestrators.Organizers
{
    using System;
    using System.Threading.Tasks;
    using System.Xml;
    using Core;
    using Core.Dependency.Markers;
    using Core.Orchestrators;
    using Core.ViewModel.Constants;

    public class YandexMeanOrganizer : IMeanOrganizer, ITransientDependency
    {
        public TranslatorType TranslatorType => TranslatorType.YANDEX;

        public async Task<Maybe<string>> OrganizeMean(string text)
        {
            try
            {
                if (text == null) return new Maybe<string>();

                var doc = new XmlDocument();
                doc.LoadXml(text);
                var node = doc.SelectSingleNode("//Translation/text");
                var output = node?.InnerText ?? "!!! An error occured";

                return new Maybe<string>(output.ToLower().Trim());
            }
            catch (Exception)
            {
                //ingore
            }

            return new Maybe<string>();
        }
    }
}
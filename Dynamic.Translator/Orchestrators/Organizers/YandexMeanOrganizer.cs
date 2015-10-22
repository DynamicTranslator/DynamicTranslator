namespace Dynamic.Tureng.Translator.Orchestrators.Organizers
{
    using System;
    using System.Threading.Tasks;
    using System.Xml;
    using Dynamic.Translator.Core;
    using Dynamic.Translator.Core.Dependency.Markers;
    using Dynamic.Translator.Core.Orchestrators;
    using Dynamic.Translator.Core.ViewModel.Constants;

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
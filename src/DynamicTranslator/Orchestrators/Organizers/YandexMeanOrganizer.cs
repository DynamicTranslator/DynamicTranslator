﻿namespace DynamicTranslator.Orchestrators.Organizers
{
    #region using

    using System.Threading.Tasks;
    using System.Xml;
    using Core.Orchestrators.Model;
    using Core.Orchestrators.Organizer;
    using Core.ViewModel.Constants;

    #endregion

    public class YandexMeanOrganizer : IMeanOrganizer
    {
        public bool IsTranslationActive => true;

        public TranslatorType TranslatorType => TranslatorType.YANDEX;

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
    }
}
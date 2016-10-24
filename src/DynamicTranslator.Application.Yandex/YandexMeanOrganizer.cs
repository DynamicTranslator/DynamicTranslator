using System.Threading.Tasks;
using System.Xml;

using Abp.Collections.Extensions;
using Abp.Dependency;

using DynamicTranslator.Constants;
using DynamicTranslator.Extensions;

namespace DynamicTranslator.Application.Yandex
{
    public class YandexMeanOrganizer : AbstractMeanOrganizer, IMeanOrganizer, ITransientDependency
    {
        public override TranslatorType TranslatorType => TranslatorType.Yandex;

        public override Task<Maybe<string>> OrganizeMean(string text, string fromLanguageExtension)
        {
            string output;

            if (text == null)
            {
                return Task.FromResult(new Maybe<string>());
            }

            if (text.IsXml())
            {
                var doc = new XmlDocument();
                doc.LoadXml(text);
                XmlNode node = doc.SelectSingleNode("//Translation/text");
                output = node?.InnerText ?? "!!! An error occurred";
            }
            else
            {
                output = text.DeserializeAs<YandexDetectResponse>().Text.JoinAsString(",");
            }

            return Task.FromResult(new Maybe<string>(output.ToLower().Trim()));
        }
    }
}

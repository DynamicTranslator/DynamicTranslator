using System.Threading.Tasks;

using DynamicTranslator.Constants;

namespace DynamicTranslator.Application
{
    public abstract class AbstractMeanOrganizer : IMeanOrganizer
    {
        public Task<Maybe<string>> OrganizeMean(string text)
        {
            return OrganizeMean(text, string.Empty);
        }

        public abstract Task<Maybe<string>> OrganizeMean(string text, string fromLanguageExtension);

        public abstract TranslatorType TranslatorType { get; }
    }
}
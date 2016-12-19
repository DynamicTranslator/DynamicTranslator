using System.Threading.Tasks;

using DynamicTranslator.Constants;

namespace DynamicTranslator.Application.Orchestrators.Organizers
{
    public abstract class AbstractMeanOrganizer
    {
        public abstract TranslatorType TranslatorType { get; }

        public Task<Maybe<string>> OrganizeMean(string text)
        {
            return OrganizeMean(text, string.Empty);
        }

        public abstract Task<Maybe<string>> OrganizeMean(string text, string fromLanguageExtension);
    }
}

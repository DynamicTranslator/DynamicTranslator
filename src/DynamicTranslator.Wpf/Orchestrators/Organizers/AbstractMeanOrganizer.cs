using System.Threading.Tasks;

using DynamicTranslator.Application;
using DynamicTranslator.Application.Orchestrators;
using DynamicTranslator.Constants;

namespace DynamicTranslator.Wpf.Orchestrators.Organizers
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
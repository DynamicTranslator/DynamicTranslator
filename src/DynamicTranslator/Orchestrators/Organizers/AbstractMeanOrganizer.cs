#region using

using System.Threading.Tasks;

using DynamicTranslator.Core.Orchestrators.Model;
using DynamicTranslator.Core.Orchestrators.Organizer;
using DynamicTranslator.Core.ViewModel.Constants;

#endregion

namespace DynamicTranslator.Orchestrators.Organizers
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
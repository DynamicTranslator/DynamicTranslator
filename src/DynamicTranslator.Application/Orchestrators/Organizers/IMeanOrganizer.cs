using System.Threading.Tasks;

namespace DynamicTranslator.Application.Orchestrators.Organizers
{
    public interface IMeanOrganizer : IMustHaveTranslatorType
    {
        Task<Maybe<string>> OrganizeMean(string text);

        Task<Maybe<string>> OrganizeMean(string text, string fromLanguageExtension);
    }
}

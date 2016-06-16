using System.Threading.Tasks;

namespace DynamicTranslator.Application.Orchestrators
{
    public interface IMeanOrganizer : IOrchestrator
    {
        Task<Maybe<string>> OrganizeMean(string text);

        Task<Maybe<string>> OrganizeMean(string text, string fromLanguageExtension);
    }
}
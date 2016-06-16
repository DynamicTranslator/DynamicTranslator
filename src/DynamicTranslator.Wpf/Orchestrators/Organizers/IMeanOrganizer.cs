using System.Threading.Tasks;

using DynamicTranslator.Application.Orchestrators;

namespace DynamicTranslator.Wpf.Orchestrators.Organizers
{
    public interface IMeanOrganizer : IOrchestrator
    {
        Task<Maybe<string>> OrganizeMean(string text);

        Task<Maybe<string>> OrganizeMean(string text, string fromLanguageExtension);
    }
}
using System.Threading.Tasks;

using DynamicTranslator.Core.Orchestrators.Model;

namespace DynamicTranslator.Core.Orchestrators.Organizer
{
    #region using

    

    #endregion

    public interface IMeanOrganizer : IOrchestrator
    {
        Task<Maybe<string>> OrganizeMean(string text);

        Task<Maybe<string>> OrganizeMean(string text, string fromLanguageExtension);
    }
}
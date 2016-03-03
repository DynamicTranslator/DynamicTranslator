namespace DynamicTranslator.Core.Orchestrators.Organizer
{
    #region using

    using System.Threading.Tasks;
    using Model;

    #endregion

    public interface IMeanOrganizer : IOrchestrator
    {
        Task<Maybe<string>> OrganizeMean(string text);
        Task<Maybe<string>> OrganizeMean(string text, string fromLanguageExtension);
    }
}
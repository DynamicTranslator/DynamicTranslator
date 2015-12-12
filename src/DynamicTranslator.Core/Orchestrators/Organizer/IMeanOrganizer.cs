namespace DynamicTranslator.Core.Orchestrators.Organizer
{
    #region using

    using System.Threading.Tasks;
    using Dependency.Markers;
    using Model;
    using ViewModel.Constants;

    #endregion

    public interface IMeanOrganizer : IOrchestrator
    {
        Task<Maybe<string>> OrganizeMean(string text);
    }
}
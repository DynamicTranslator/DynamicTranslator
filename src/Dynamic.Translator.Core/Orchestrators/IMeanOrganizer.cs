namespace Dynamic.Translator.Core.Orchestrators
{
    #region using

    using System.Threading.Tasks;
    using Dependency.Markers;
    using ViewModel.Constants;

    #endregion

    public interface IMeanOrganizer : ITransientDependency
    {
        TranslatorType TranslatorType { get; }

        Task<Maybe<string>> OrganizeMean(string text);
    }
}
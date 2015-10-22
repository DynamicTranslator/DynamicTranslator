namespace Dynamic.Translator.Core.Orchestrators
{
    using System.Threading.Tasks;
    using Dependency.Markers;
    using ViewModel.Constants;

    public interface IMeanOrganizer
    {
        TranslatorType TranslatorType { get; }
        Task<Maybe<string>> OrganizeMean(string text);
    }
}
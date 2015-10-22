namespace Dynamic.Translator.Core.Orchestrators
{
    using System.Threading.Tasks;
    using Dependency.Markers;

    public interface IMeanOrganizer : ITransientDependency
    {
        Task<Maybe<string>> OrganizeMean(string text);
    }
}
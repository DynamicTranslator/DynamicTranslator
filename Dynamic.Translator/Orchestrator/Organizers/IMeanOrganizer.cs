namespace Dynamic.Tureng.Translator.Orchestrator.Organizers
{
    using System.Threading.Tasks;

    public interface IMeanOrganizer
    {
        Task<Maybe<string>> OrganizeMean(string text);
    }
}
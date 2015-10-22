namespace Dynamic.Translator.Core.Orchestrators
{
    using System.Collections.Generic;

    public interface IMeanOrganizerFactory
    {
        ICollection<IMeanOrganizer> GetMeanOrganizers();
    }
}
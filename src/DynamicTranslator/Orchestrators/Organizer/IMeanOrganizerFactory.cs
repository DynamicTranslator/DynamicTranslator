using System.Collections.Generic;

namespace DynamicTranslator.Orchestrators.Organizer
{
    public interface IMeanOrganizerFactory
    {
        ICollection<IMeanOrganizer> GetMeanOrganizers();
    }
}
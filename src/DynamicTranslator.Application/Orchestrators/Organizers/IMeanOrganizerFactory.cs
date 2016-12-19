using System.Collections.Generic;

namespace DynamicTranslator.Application.Orchestrators.Organizers
{
    public interface IMeanOrganizerFactory
    {
        ICollection<IMeanOrganizer> GetMeanOrganizers();
    }
}

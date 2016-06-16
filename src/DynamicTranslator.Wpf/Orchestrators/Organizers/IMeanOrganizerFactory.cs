using System.Collections.Generic;

namespace DynamicTranslator.Wpf.Orchestrators.Organizers
{
    public interface IMeanOrganizerFactory
    {
        ICollection<IMeanOrganizer> GetMeanOrganizers();
    }
}
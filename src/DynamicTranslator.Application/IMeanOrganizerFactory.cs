using System.Collections.Generic;

namespace DynamicTranslator.Application
{
    public interface IMeanOrganizerFactory
    {
        ICollection<IMeanOrganizer> GetMeanOrganizers();
    }
}
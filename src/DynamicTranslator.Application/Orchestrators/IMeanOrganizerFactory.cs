using System.Collections.Generic;

namespace DynamicTranslator.Application.Orchestrators
{
    public interface IMeanOrganizerFactory
    {
        ICollection<IMeanOrganizer> GetMeanOrganizers();
    }
}
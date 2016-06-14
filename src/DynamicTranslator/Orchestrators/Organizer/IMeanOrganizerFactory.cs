using System.Collections.Generic;

namespace DynamicTranslator.Orchestrators.Organizer
{
    #region using

    

    #endregion

    public interface IMeanOrganizerFactory
    {
        ICollection<IMeanOrganizer> GetMeanOrganizers();
    }
}
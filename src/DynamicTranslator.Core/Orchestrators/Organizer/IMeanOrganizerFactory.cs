using System.Collections.Generic;

namespace DynamicTranslator.Core.Orchestrators.Organizer
{
    #region using

    

    #endregion

    public interface IMeanOrganizerFactory
    {
        ICollection<IMeanOrganizer> GetMeanOrganizers();
    }
}
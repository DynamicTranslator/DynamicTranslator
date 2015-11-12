namespace Dynamic.Translator.Core.Orchestrators
{
    #region using

    using System.Collections.Generic;

    #endregion

    public interface IMeanOrganizerFactory
    {
        ICollection<IMeanOrganizer> GetMeanOrganizers();
    }
}
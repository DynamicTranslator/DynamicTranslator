using System.Collections.Generic;

namespace DynamicTranslator.Orchestrators.Finder
{
    #region using

    

    #endregion

    public interface IMeanFinderFactory
    {
        ICollection<IMeanFinder> GetFinders();
    }
}
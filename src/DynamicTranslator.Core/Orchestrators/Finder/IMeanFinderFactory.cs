using System.Collections.Generic;

namespace DynamicTranslator.Core.Orchestrators.Finder
{
    #region using

    

    #endregion

    public interface IMeanFinderFactory
    {
        ICollection<IMeanFinder> GetFinders();
    }
}
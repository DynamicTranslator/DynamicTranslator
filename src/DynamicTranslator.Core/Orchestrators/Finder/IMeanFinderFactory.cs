namespace DynamicTranslator.Core.Orchestrators.Finder
{
    #region using

    using System.Collections.Generic;

    #endregion

    public interface IMeanFinderFactory
    {
        ICollection<IMeanFinder> GetFinders();
    }
}
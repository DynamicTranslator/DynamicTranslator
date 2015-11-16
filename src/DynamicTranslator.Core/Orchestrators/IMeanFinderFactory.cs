namespace DynamicTranslator.Core.Orchestrators
{
    #region using

    using System.Collections.Generic;

    #endregion

    public interface IMeanFinderFactory
    {
        ICollection<IMeanFinder> GetFinders();
    }
}
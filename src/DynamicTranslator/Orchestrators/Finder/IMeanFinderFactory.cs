using System.Collections.Generic;

namespace DynamicTranslator.Orchestrators.Finder
{
    public interface IMeanFinderFactory
    {
        ICollection<IMeanFinder> GetFinders();
    }
}
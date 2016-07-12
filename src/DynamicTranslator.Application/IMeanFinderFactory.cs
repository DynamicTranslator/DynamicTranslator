using System.Collections.Generic;

namespace DynamicTranslator.Application
{
    public interface IMeanFinderFactory
    {
        ICollection<IMeanFinder> GetFinders();
    }
}
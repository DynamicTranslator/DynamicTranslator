using System.Collections.Generic;

namespace DynamicTranslator.Wpf.Orchestrators.Finders
{
    public interface IMeanFinderFactory
    {
        ICollection<IMeanFinder> GetFinders();
    }
}
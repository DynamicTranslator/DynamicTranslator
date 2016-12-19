using System.Collections.Generic;

namespace DynamicTranslator.Application.Orchestrators.Finders
{
    public interface IMeanFinderFactory
    {
        ICollection<IMeanFinder> GetFinders();
    }
}

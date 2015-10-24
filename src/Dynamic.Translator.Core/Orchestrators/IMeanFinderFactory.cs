namespace Dynamic.Translator.Core.Orchestrators
{
    using System.Collections.Generic;

    public interface IMeanFinderFactory
    {
        ICollection<IMeanFinder> GetFinders();
    }
}
namespace Dynamic.Translator.Core.Orchestrators
{
    using System.Threading.Tasks;
    using Dependency.Markers;

    public interface IMeanFinder: ITransientDependency
    {
        Task<TranslateResult> Find(string text);
    }
}
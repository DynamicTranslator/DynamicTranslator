namespace Dynamic.Translator.Core.Orchestrators
{
    using System;
    using System.Threading.Tasks;
    using Dependency.Markers;

    public interface IMeanFinder : ITransientDependency
    {
        Task<Maybe<string>> Find(string text);

        event EventHandler<WhenNotificationAddEventArgs> WhenNotificationAddEventHandler;
    }
}
namespace Dynamic.Tureng.Translator.Orchestrator.Queue
{
    using System.Collections.Generic;

    public interface IQueue : IEnumerable<string>
    {
        void Delete(string text);
    }
}
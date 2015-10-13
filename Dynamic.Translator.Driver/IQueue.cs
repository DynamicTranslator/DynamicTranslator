namespace Dynamic.Translator.Driver
{
    using System.Collections.Generic;
    using System.IO;

    public interface IQueue : IEnumerable<Stream>
    {
        void Delete(Stream stream);
    }
}
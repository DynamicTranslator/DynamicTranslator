namespace Dynamic.Translator.Driver
{
    using System.Collections.Generic;

    public interface IQuickening
    {
        IEnumerable<IMessage> Quicken(dynamic envelope);
    }
}
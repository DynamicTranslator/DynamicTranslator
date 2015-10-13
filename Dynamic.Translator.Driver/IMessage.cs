namespace Dynamic.Translator.Driver
{
    using System;

    public interface IMessage
    {
        Envelope Envelop();

        Guid Id { get; }
    }
}
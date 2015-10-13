namespace Dynamic.Translator.Driver
{
    public interface IChannel<in T> where T : IMessage
    {
        void Send(T message);
    }
}
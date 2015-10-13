namespace Dynamic.Translator.Driver
{
    public interface IEventHandler<T>
    {
        void Handle(T @event);
    }
}
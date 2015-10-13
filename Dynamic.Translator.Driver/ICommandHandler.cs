namespace Dynamic.Translator.Driver
{
    public interface ICommandHandler<T>
    {
        void Execute(T command);
    }
}
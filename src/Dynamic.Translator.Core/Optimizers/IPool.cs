namespace Dynamic.Translator.Core.Optimizers
{
    public interface IPool<T>
    {
        T GetOrCreate(T obj);
    }
}
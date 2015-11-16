namespace DynamicTranslator.Core.Optimizers
{
    public interface IPool<T>
    {
        T GetOrCreate(T obj);
    }
}
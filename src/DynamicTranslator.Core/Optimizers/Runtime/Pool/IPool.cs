namespace DynamicTranslator.Core.Optimizers.Runtime.Pool
{
    public interface IPool<T>
    {
        T GetOrCreate(T obj);
    }
}
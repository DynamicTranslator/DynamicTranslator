namespace DynamicTranslator.Core.Optimizers.Runtime.Pool
{
    #region using

    using System.Collections.Concurrent;
    using Dependency.Markers;

    #endregion

    public class Pool<T> : IPool<T>, ITransientDependency
    {
        private readonly ConcurrentDictionary<T, T> pool = new ConcurrentDictionary<T, T>();

        public T GetOrCreate(T obj)
        {
            T result;

            if (!pool.TryGetValue(obj, out result))
            {
                pool[obj] = obj;
                result = obj;
            }

            return result;
        }
    }
}
namespace DynamicTranslator.Core.Optimizers.Runtime.Pool
{
    #region using

    using System.Collections.Concurrent;
    using DynamicTranslator.Core.Dependency.Markers;

    #endregion

    /// <summary>
    ///     The pool.
    /// </summary>
    /// <typeparam name="T">
    ///     T is any object
    /// </typeparam>
    public class Pool<T> : IPool<T>, ITransientDependency
    {
        /// <summary>
        ///     The pool.
        /// </summary>
        private readonly ConcurrentDictionary<T, T> pool = new ConcurrentDictionary<T, T>();

        /// <summary>
        ///     The get or create.
        /// </summary>
        /// <param name="obj">
        ///     The object.
        /// </param>
        /// <returns>
        ///     The <see cref="T" />.
        /// </returns>
        public T GetOrCreate(T obj)
        {
            T result;

            if (!this.pool.TryGetValue(obj, out result))
            {
                this.pool[obj] = obj;
                result = obj;
            }

            return result;
        }
    }
}
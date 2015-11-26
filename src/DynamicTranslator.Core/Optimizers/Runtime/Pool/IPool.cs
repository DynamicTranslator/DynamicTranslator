namespace DynamicTranslator.Core.Optimizers.Runtime.Pool
{
    /// <summary>
    /// The Pool interface.
    /// </summary>
    /// <typeparam name="T">Any item to be pooled
    /// </typeparam>
    public interface IPool<T>
    {
        /// <summary>
        /// The get or create.
        /// </summary>
        /// <param name="obj">
        /// The any object.
        /// </param>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        T GetOrCreate(T obj);
    }
}
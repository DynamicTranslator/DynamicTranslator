namespace Dynamic.Translator.Core.Optimizers.Runtime
{
    #region using

    using System;
    using System.Collections.Generic;

    #endregion

    public interface ICacheManager : IDisposable
    {
        /// <summary>
        ///     Gets all caches.
        /// </summary>
        /// <returns>List of caches</returns>
        IReadOnlyList<ICache> GetAllCacheEnvironments();

        /// <summary>
        ///     Gets (or creates) a cache.
        /// </summary>
        /// <param name="name">Unique name of the cache</param>
        /// <returns>The cache reference</returns>
        ICache GetCacheEnvironment(string name);
    }
}
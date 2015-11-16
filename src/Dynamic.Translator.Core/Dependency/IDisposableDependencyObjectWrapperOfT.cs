namespace DynamicTranslator.Core.Dependency
{
    #region using

    using System;

    #endregion

    public interface IDisposableDependencyObjectWrapper<out T> : IDisposable
    {
        /// <summary>
        ///     The resolved object.
        /// </summary>
        T Object { get; }
    }
}
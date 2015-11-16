namespace DynamicTranslator.Core.Dependency
{
    #region using

    using Manager;

    #endregion

    internal class DisposableDependencyObjectWrapperOfT : DisposableDependencyObjectWrapper<object>, IDisposableDependencyObjectWrapper
    {
        public DisposableDependencyObjectWrapperOfT(IIocResolver iocResolver, object obj)
            : base(iocResolver, obj)
        {
        }
    }

    internal class DisposableDependencyObjectWrapper<T> : IDisposableDependencyObjectWrapper<T>
    {
        private readonly IIocResolver _iocResolver;

        public DisposableDependencyObjectWrapper(IIocResolver iocResolver, T obj)
        {
            _iocResolver = iocResolver;
            Object = obj;
        }

        public void Dispose()
        {
            _iocResolver.Release(Object);
        }

        public T Object { get; }
    }
}
namespace Dynamic.Translator.Core.Dependency
{
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

        public T Object { get; }

        public void Dispose()
        {
            _iocResolver.Release(Object);
        }
    }
}
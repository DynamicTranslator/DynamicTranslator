namespace Dynamic.Translator.Core.Dependency
{
    using Manager;

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
            this._iocResolver = iocResolver;
            this.Object = obj;
        }

        public T Object { get; }

        public void Dispose()
        {
            this._iocResolver.Release(this.Object);
        }
    }
}
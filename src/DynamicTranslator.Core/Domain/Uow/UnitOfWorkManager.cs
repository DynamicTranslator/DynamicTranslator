namespace DynamicTranslator.Core.Domain.Uow
{
    #region using

    using System.Transactions;
    using Dependency.Manager;
    using Dependency.Markers;

    #endregion

    /// <summary>
    ///     Unit of work manager.
    /// </summary>
    internal class UnitOfWorkManager : IUnitOfWorkManager, ITransientDependency
    {
        private readonly ICurrentUnitOfWorkProvider _currentUnitOfWorkProvider;
        private readonly IUnitOfWorkDefaultOptions _defaultOptions;
        private readonly IIocResolver _iocResolver;

        public UnitOfWorkManager(
            IIocResolver iocResolver,
            ICurrentUnitOfWorkProvider currentUnitOfWorkProvider,
            IUnitOfWorkDefaultOptions defaultOptions)
        {
            _iocResolver = iocResolver;
            _currentUnitOfWorkProvider = currentUnitOfWorkProvider;
            _defaultOptions = defaultOptions;
        }

        public IActiveUnitOfWork Current => _currentUnitOfWorkProvider.Current;

        public IUnitOfWorkCompleteHandle Begin()
        {
            return Begin(new UnitOfWorkOptions());
        }

        public IUnitOfWorkCompleteHandle Begin(TransactionScopeOption scope)
        {
            return Begin(new UnitOfWorkOptions {Scope = scope});
        }

        public IUnitOfWorkCompleteHandle Begin(UnitOfWorkOptions options)
        {
            options.FillDefaultsForNonProvidedOptions(_defaultOptions);

            if (options.Scope == TransactionScopeOption.Required && _currentUnitOfWorkProvider.Current != null)
            {
                return new InnerUnitOfWorkCompleteHandle();
            }

            var uow = _iocResolver.Resolve<IUnitOfWork>();

            uow.Completed += (sender, args) => { _currentUnitOfWorkProvider.Current = null; };

            uow.Failed += (sender, args) => { _currentUnitOfWorkProvider.Current = null; };

            uow.Disposed += (sender, args) => { _iocResolver.Release(uow); };

            uow.Begin(options);

            _currentUnitOfWorkProvider.Current = uow;

            return uow;
        }
    }
}
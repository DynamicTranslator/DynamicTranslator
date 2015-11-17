namespace DynamicTranslator.Core.DBReezeNoSQL.Uow
{
    #region using

    using DBreeze.Transactions;
    using Dependency.Markers;
    using Domain.Uow;

    #endregion

    public class UowTransactionProvider : ITransactionProvider, ITransientDependency
    {
        private readonly ICurrentUnitOfWorkProvider currentUnitOfWorkProvider;

        public UowTransactionProvider(ICurrentUnitOfWorkProvider currentUnitOfWorkProvider)
        {
            this.currentUnitOfWorkProvider = currentUnitOfWorkProvider;
        }

        public Transaction Transaction => currentUnitOfWorkProvider.Current.GetTransaction();
    }
}
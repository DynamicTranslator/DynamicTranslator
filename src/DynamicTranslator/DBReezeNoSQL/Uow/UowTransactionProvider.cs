using DBreeze.Transactions;

using DynamicTranslator.Dependency.Markers;
using DynamicTranslator.Domain.Uow;

namespace DynamicTranslator.DBReezeNoSQL.Uow
{
    #region using

    

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
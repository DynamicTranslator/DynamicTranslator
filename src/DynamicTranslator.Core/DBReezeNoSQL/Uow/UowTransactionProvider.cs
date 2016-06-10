using DBreeze.Transactions;

using DynamicTranslator.Core.Dependency.Markers;
using DynamicTranslator.Core.Domain.Uow;

namespace DynamicTranslator.Core.DBReezeNoSQL.Uow
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
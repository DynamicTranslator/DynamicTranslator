using Abp.Dependency;
using Abp.Domain.Uow;

using DBreeze.Transactions;

namespace DynamicTranslator.DbReeze.DBReezeNoSQL.Uow
{
    public class UowTransactionProvider : ITransactionProvider, ITransientDependency
    {
        public UowTransactionProvider(ICurrentUnitOfWorkProvider currentUnitOfWorkProvider)
        {
            this.currentUnitOfWorkProvider = currentUnitOfWorkProvider;
        }

        private readonly ICurrentUnitOfWorkProvider currentUnitOfWorkProvider;

        public Transaction Transaction => currentUnitOfWorkProvider.Current.GetTransaction();
    }
}
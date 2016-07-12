using Abp.Dependency;
using Abp.Domain.Uow;

using DBreeze.Transactions;

namespace DynamicTranslator.DbReeze.DBReezeNoSQL.Uow
{
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
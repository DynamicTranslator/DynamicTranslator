using Abp.Dependency;
using Abp.Domain.Uow;

using DBreeze.Transactions;

namespace DynamicTranslator.DbReeze.DBReezeNoSQL.Uow
{
    public class DbReezeUowTransactionProvider : ITransactionProvider, ITransientDependency
    {
        private readonly ICurrentUnitOfWorkProvider currentUnitOfWorkProvider;

        public DbReezeUowTransactionProvider(ICurrentUnitOfWorkProvider currentUnitOfWorkProvider)
        {
            this.currentUnitOfWorkProvider = currentUnitOfWorkProvider;
        }

        public Transaction Transaction => currentUnitOfWorkProvider.Current.GetTransaction();
    }
}
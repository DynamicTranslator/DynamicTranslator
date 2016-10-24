using Abp.Dependency;
using Abp.Domain.Uow;

using DBreeze.Transactions;

namespace DynamicTranslator.Domain.DbReeze.DBReezeNoSQL.Uow
{
    public class DbReezeUowTransactionProvider : ITransactionProvider, ITransientDependency
    {
        private readonly ICurrentUnitOfWorkProvider _currentUnitOfWorkProvider;

        public DbReezeUowTransactionProvider(ICurrentUnitOfWorkProvider currentUnitOfWorkProvider)
        {
            _currentUnitOfWorkProvider = currentUnitOfWorkProvider;
        }

        public Transaction Transaction => _currentUnitOfWorkProvider.Current.GetTransaction();
    }
}

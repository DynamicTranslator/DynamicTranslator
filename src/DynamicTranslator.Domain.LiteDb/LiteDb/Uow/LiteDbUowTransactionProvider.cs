using Abp.Dependency;
using Abp.Domain.Uow;

using LiteDB;

namespace DynamicTranslator.Domain.LiteDb.LiteDb.Uow
{
    public class LiteDbUowTransactionProvider : ITransactionProvider, ITransientDependency
    {
        private readonly ICurrentUnitOfWorkProvider _currentUnitOfWorkProvider;

        public LiteDbUowTransactionProvider(ICurrentUnitOfWorkProvider currentUnitOfWorkProvider)
        {
            _currentUnitOfWorkProvider = currentUnitOfWorkProvider;
        }

        public LiteTransaction Transaction => _currentUnitOfWorkProvider.Current.GetTransaction();
    }
}

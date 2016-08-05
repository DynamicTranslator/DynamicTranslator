using Abp.Dependency;
using Abp.Domain.Uow;

using LiteDB;

namespace DynamicTranslator.LiteDb.LiteDb.Uow
{
    public class LiteDbUowTransactionProvider : ITransactionProvider, ITransientDependency
    {
        private readonly ICurrentUnitOfWorkProvider currentUnitOfWorkProvider;

        public LiteDbUowTransactionProvider(ICurrentUnitOfWorkProvider currentUnitOfWorkProvider)
        {
            this.currentUnitOfWorkProvider = currentUnitOfWorkProvider;
        }

        public LiteTransaction Transaction => currentUnitOfWorkProvider.Current.GetTransaction();
    }
}
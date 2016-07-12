using System.Threading.Tasks;

using Abp.Dependency;
using Abp.Domain.Uow;

using DBreeze;
using DBreeze.Transactions;

namespace DynamicTranslator.DbReeze.DBReezeNoSQL.Uow
{
    public class DbReezeUnitOfWork : UnitOfWorkBase, ITransientDependency
    {
        public DBreezeEngine DBreezeEngine { get; set; }

        public Transaction Transaction { get; private set; }

        public DbReezeUnitOfWork(IConnectionStringResolver connectionStringResolver, IUnitOfWorkDefaultOptions defaultOptions)
            : base(connectionStringResolver, defaultOptions) {}

        public override void SaveChanges()
        {
            Transaction.Commit();
        }

        public override Task SaveChangesAsync()
        {
            SaveChanges();
            return Task.FromResult(0);
        }

        protected override void BeginUow()
        {
            Transaction = DBreezeEngine.GetTransaction();
        }

        protected override void CompleteUow()
        {
            Transaction.Commit();
        }

        protected override Task CompleteUowAsync()
        {
            CompleteUow();
            return Task.FromResult(0);
        }

        protected override void DisposeUow()
        {
            if (Transaction != null)
            {
                Transaction.Dispose();
                Transaction = null;
            }
        }
    }
}
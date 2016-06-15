using System.Threading.Tasks;

using Abp.Dependency;
using Abp.Domain.Uow;

using DBreeze;
using DBreeze.Transactions;

namespace DynamicTranslator.DbReeze.DBReezeNoSQL.Uow
{
    public class DbReezeUnitOfWork : UnitOfWorkBase, ITransientDependency
    {
        public DbReezeUnitOfWork(IConnectionStringResolver connectionStringResolver, IUnitOfWorkDefaultOptions defaultOptions)
            : base(connectionStringResolver, defaultOptions) {}

        public Transaction Transaction { get; private set; }

        public DBreezeEngine DBreezeEngine { get; set; }

        public override void SaveChanges()
        {
            Transaction.Commit();
        }

        public override Task SaveChangesAsync()
        {
            Transaction.Commit();
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
            Transaction.Commit();
            return Task.FromResult(0);
        }

        protected override void DisposeUow()
        {
            if (Transaction != null)
            {
                Transaction.Dispose();
                Transaction = null;
            }

            DBreezeEngine.Dispose();
        }
    }
}
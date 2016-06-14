using System;
using System.Threading.Tasks;

using Abp.Domain.Uow;

using DBreeze;
using DBreeze.Transactions;

namespace DynamicTranslator.DBReezeNoSQL.Uow
{
    public class DbReezeUnitOfWork : UnitOfWorkBase
    {
        private readonly DBreezeEngine dBreezeEngine;

        public DbReezeUnitOfWork(IUnitOfWorkDefaultOptions defaultOptions, IConnectionStringResolver connectionStringResolver, DBreezeEngine dBreezeEngine)
            : base(connectionStringResolver, defaultOptions)
        {
            this.dBreezeEngine = dBreezeEngine;
        }

        public Transaction Transaction { get; private set; }

        public override void SaveChanges()
        {
            throw new NotImplementedException();
        }

        public override Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        protected override void BeginUow()
        {
            Transaction = dBreezeEngine.GetTransaction();
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
            Transaction.Dispose();
        }
    }
}
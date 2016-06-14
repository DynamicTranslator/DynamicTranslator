using System;
using System.Threading.Tasks;

using DBreeze;
using DBreeze.Transactions;

using DynamicTranslator.Domain.Uow;

namespace DynamicTranslator.DBReezeNoSQL.Uow
{
    #region using

    

    #endregion

    public class DbReezeUnitOfWork : UnitOfWorkBase
    {
        private readonly DBreezeEngine dBreezeEngine;

        public DbReezeUnitOfWork(IUnitOfWorkDefaultOptions defaultOptions, DBreezeEngine dBreezeEngine) : base(defaultOptions)
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
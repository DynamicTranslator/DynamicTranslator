using System.Linq;

using Abp.Domain.Entities;
using Abp.Domain.Repositories;

using DBreeze.Transactions;

using DynamicTranslator.DbReeze.DBReezeNoSQL.Extensions;
using DynamicTranslator.Helper;

namespace DynamicTranslator.DbReeze.DBReezeNoSQL.Repository
{
    public class DBReezeRepositoryBase<TEntity, TKey> : AbpRepositoryBase<TEntity, TKey> where TEntity : class, IEntity<TKey>
    {
        public Transaction Transaction => transactionProvider.Transaction;

        private readonly ITransactionProvider transactionProvider;

        public DBReezeRepositoryBase(ITransactionProvider transactionProvider)
        {
            this.transactionProvider = transactionProvider;
        }

        public override void Delete(TEntity entity) {}

        public override void Delete(TKey id)
        {
            Transaction.RemoveKey(typeof(TEntity).Name, id);
        }

        public override IQueryable<TEntity> GetAll()
        {
            return Transaction.SelectForward<TKey, TEntity>(typeof(TEntity).Name).AsQueryable();
        }

        public override TEntity Insert(TEntity entity)
        {
            Transaction.Insert(typeof(TEntity).Name, entity.Id, ObjectHelper.ObjectToByteArray(entity));
            return entity;
        }

        public override TEntity Update(TEntity entity)
        {
            return Insert(entity);
        }
    }
}
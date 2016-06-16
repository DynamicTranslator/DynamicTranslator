using System.Collections.Generic;
using System.Linq;

using Abp.Domain.Entities;
using Abp.Domain.Repositories;

using DBreeze.Transactions;

namespace DynamicTranslator.DbReeze.DBReezeNoSQL.Repository
{
    public class DBReezeRepositoryBase<TEntity, TKey> : AbpRepositoryBase<TEntity, TKey> where TEntity : class, IEntity<TKey>
    {
        private readonly ITransactionProvider transactionProvider;

        public DBReezeRepositoryBase(ITransactionProvider transactionProvider)
        {
            this.transactionProvider = transactionProvider;
        }

        public Transaction Transaction => transactionProvider.Transaction;

        public override void Delete(TEntity entity) {}

        public override void Delete(TKey id)
        {
            Transaction.RemoveKey(typeof(TEntity).Name, id);
        }

        public override IQueryable<TEntity> GetAll()
        {
            var a = Transaction.SelectForward<TKey, TEntity>(typeof(TEntity).Name).AsQueryable();

            return new EnumerableQuery<TEntity>(new List<TEntity>());
        }

        public override TEntity Insert(TEntity entity)
        {
            Transaction.Insert(typeof(TEntity).Name, entity.Id, entity);
            return entity;
        }

        public override TEntity Update(TEntity entity)
        {
            return Insert(entity);
        }
    }
}
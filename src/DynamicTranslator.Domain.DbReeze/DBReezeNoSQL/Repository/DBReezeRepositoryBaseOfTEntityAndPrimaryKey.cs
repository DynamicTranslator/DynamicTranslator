using System.Linq;

using Abp.Domain.Entities;
using Abp.Domain.Repositories;

using DBreeze.Transactions;

using DynamicTranslator.Domain.DbReeze.DBReezeNoSQL.Extensions;
using DynamicTranslator.Helper;

namespace DynamicTranslator.Domain.DbReeze.DBReezeNoSQL.Repository
{
    public class DBReezeRepositoryBase<TEntity, TKey> : AbpRepositoryBase<TEntity, TKey> where TEntity : class, IEntity<TKey>
    {
        private readonly ITransactionProvider _transactionProvider;

        public DBReezeRepositoryBase(ITransactionProvider transactionProvider)
        {
            _transactionProvider = transactionProvider;
        }

        public Transaction Transaction => _transactionProvider.Transaction;

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

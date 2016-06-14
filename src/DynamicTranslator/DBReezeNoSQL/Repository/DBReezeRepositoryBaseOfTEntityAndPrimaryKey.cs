using DBreeze.Transactions;

using DynamicTranslator.DBReezeNoSQL.Extensions;
using DynamicTranslator.Domain.Repository;
using DynamicTranslator.Helper;

namespace DynamicTranslator.DBReezeNoSQL.Repository
{
    #region using

    

    #endregion

    public class DBReezeRepositoryBase<TEntity, TKey> : RepositoryBase<TEntity, TKey> where TEntity : class
    {
        private readonly ITransactionProvider transactionProvider;

        public DBReezeRepositoryBase(ITransactionProvider transactionProvider)
        {
            this.transactionProvider = transactionProvider;
        }

        protected Transaction Transaction => transactionProvider.Transaction;

        public override TEntity Get(TKey id)
        {
            return Transaction.Select<TKey, byte[]>(typeof(TEntity).Name, id).GetSafely<TEntity, TKey>();
        }

        public override TEntity Insert(TEntity entity, TKey key)
        {
            Transaction.Insert(typeof(TEntity).Name, key, ObjectHelper.ObjectToByteArray(entity));
            return entity;
        }
    }
}
namespace DynamicTranslator.Core.DBReezeNoSQL.Repository
{
    #region using

    using DBreeze.Transactions;
    using Domain.Repository;
    using Helper;

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
            return (TEntity) ObjectHelper.ByteArrayToObject(Transaction.Select<TKey, byte[]>(nameof(TEntity), id).Value);
        }

        public override TEntity Insert(TEntity entity, TKey key)
        {
            Transaction.Insert(nameof(TEntity), key, ObjectHelper.ObjectToByteArray(entity));
            return entity;
        }
    }
}
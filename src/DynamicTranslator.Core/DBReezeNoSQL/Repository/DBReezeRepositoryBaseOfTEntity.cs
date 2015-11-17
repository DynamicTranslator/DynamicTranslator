namespace DynamicTranslator.Core.DBReezeNoSQL.Repository
{
    public class DBReezeRepositoryBase<TEntity> : DBReezeRepositoryBase<TEntity, string> where TEntity : class
    {
        public DBReezeRepositoryBase(ITransactionProvider transactionProvider) : base(transactionProvider)
        {
        }
    }
}
namespace DynamicTranslator.Core.Domain.Repository
{
    #region using

    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    #endregion

    public abstract class RepositoryBase<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey>
        where TEntity : class
    {
        public abstract TEntity Get(TPrimaryKey id);

        public async Task<TEntity> GetAsync(TPrimaryKey id)
        {
            return await Task.FromResult(Get(id));
        }

        public abstract TEntity Insert(TEntity entity, TPrimaryKey key);

        public async Task<TEntity> InsertAsync(TEntity entity, TPrimaryKey key)
        {
            return await Task.FromResult(Insert(entity, key));
        }

        protected static Expression<Func<TEntity, bool>> CreateEqualityExpressionForId(TPrimaryKey id)
        {
            var lambdaParam = Expression.Parameter(typeof (TEntity));

            var lambdaBody = Expression.Equal(
                Expression.PropertyOrField(lambdaParam, "Id"),
                Expression.Constant(id, typeof (TPrimaryKey))
                );

            return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
        }
    }
}
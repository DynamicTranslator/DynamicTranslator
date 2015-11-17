namespace DynamicTranslator.Core.Domain.Repository
{
    #region using

    

    #endregion

    public interface IRepository<TEntity, in TPrimaryKey> : IRepository where TEntity : class
    {
        TEntity Get(TPrimaryKey id);

        TEntity Insert(TEntity entity, TPrimaryKey key);
    }
}
using System.Linq;

using Abp.Domain.Entities;
using Abp.Domain.Repositories;

using LiteDB;

namespace DynamicTranslator.LiteDb.LiteDb.Repository
{
    public class LiteDbRepositoryBase<TEntity, TKey> : AbpRepositoryBase<TEntity, TKey> where TEntity : class, IEntity<TKey>, new()
    {
        public LiteDatabase Database { get; }

        public LiteDbRepositoryBase(LiteDatabase database)
        {
            Database = database;
        }

        public override void Delete(TEntity entity)
        {
            Delete(entity.Id);
        }

        public override void Delete(TKey id)
        {
            Database.GetCollection<TEntity>(typeof(TEntity).Name)
                    .Delete(LiteDB.Query.EQ(nameof(id), new BsonValue(id)));
        }

        public override IQueryable<TEntity> GetAll()
        {
            return Database.GetCollection<TEntity>(typeof(TEntity).Name).FindAll().AsQueryable();
        }

        public override TEntity Insert(TEntity entity)
        {
            Database.GetCollection<TEntity>(typeof(TEntity).Name)
                    .Insert(entity);

            return entity;
        }

        public override TEntity Update(TEntity entity)
        {
            Database.GetCollection<TEntity>(typeof(TEntity).Name)
                    .Update(entity);

            return entity;
        }
    }
}
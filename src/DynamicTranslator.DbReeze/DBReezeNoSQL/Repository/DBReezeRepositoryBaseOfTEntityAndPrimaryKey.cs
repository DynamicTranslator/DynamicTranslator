using System;
using System.Linq;

using Abp.Domain.Entities;
using Abp.Domain.Repositories;

namespace DynamicTranslator.DbReeze.DBReezeNoSQL.Repository
{
    public class DBReezeRepositoryBase<TEntity, TKey> : AbpRepositoryBase<TEntity, TKey> where TEntity : class, IEntity<TKey>
    {
        public override void Delete(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public override void Delete(TKey id)
        {
            throw new NotImplementedException();
        }

        public override IQueryable<TEntity> GetAll()
        {
            throw new NotImplementedException();
        }

        public override TEntity Insert(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public override TEntity Update(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
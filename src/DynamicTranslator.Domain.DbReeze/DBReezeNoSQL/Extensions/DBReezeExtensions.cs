using System.Collections.Generic;
using System.Linq;

using DBreeze.DataTypes;

using DynamicTranslator.Helper;

namespace DynamicTranslator.Domain.DbReeze.DBReezeNoSQL.Extensions
{
    internal static class DBReezeExtensions
    {
        internal static IQueryable<TEntity> AsQueryable<TKey, TEntity>(this IEnumerable<Row<TKey, TEntity>> rows)
        {
            ICollection<TEntity> values = new List<TEntity>();

            foreach (var row in rows)
            {
                values.Add((TEntity)ObjectHelper.ByteArrayToObject(row.GetValuePart(0)));
            }

            return values.AsQueryable();
        }

        internal static TEntity GetSafely<TEntity, TKey>(this Row<TKey, byte[]> returnedRow)
        {
            if (returnedRow.Exists)
            {
                return (TEntity)ObjectHelper.ByteArrayToObject(returnedRow.Value);
            }

            return default(TEntity);
        }
    }
}
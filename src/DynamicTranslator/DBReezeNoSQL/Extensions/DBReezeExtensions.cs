using DBreeze.DataTypes;

using DynamicTranslator.Helper;

namespace DynamicTranslator.DBReezeNoSQL.Extensions
{
    #region using

    

    #endregion

    internal static class DBReezeExtensions
    {
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
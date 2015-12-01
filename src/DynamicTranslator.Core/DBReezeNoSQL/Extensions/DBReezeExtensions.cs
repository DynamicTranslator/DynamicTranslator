namespace DynamicTranslator.Core.DBReezeNoSQL.Extensions
{
    #region using

    using DBreeze.DataTypes;
    using Helper;

    #endregion

    internal static class DBReezeExtensions
    {
        internal static TEntity GetSafely<TEntity, TKey>(this Row<TKey, byte[]> returnedRow)
        {
            if (returnedRow.Exists)
            {
                return (TEntity) ObjectHelper.ByteArrayToObject(returnedRow.Value);
            }

            return default(TEntity);
        }
    }
}
using System;

using Abp.Domain.Uow;

using LiteDB;

namespace DynamicTranslator.Domain.LiteDb.LiteDb.Uow
{
    public static class LiteDbDatabaseProvider
    {
        public static LiteTransaction GetTransaction(this IActiveUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
            {
                throw new ArgumentNullException(nameof(unitOfWork));
            }

            if (!(unitOfWork is LiteDbUnitOfWork))
            {
                throw new ArgumentException("unitOfWork is not type of " + typeof(LiteDbUnitOfWork).FullName, nameof(unitOfWork));
            }

            return ((LiteDbUnitOfWork)unitOfWork).Transaction;
        }
    }
}

using System;

using Abp.Domain.Uow;

using DBreeze.Transactions;

namespace DynamicTranslator.DbReeze.DBReezeNoSQL.Uow
{
    public static class DbReezeDatabaseProvider
    {
        public static Transaction GetTransaction(this IActiveUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
            {
                throw new ArgumentNullException(nameof(unitOfWork));
            }

            if (!(unitOfWork is DbReezeUnitOfWork))
            {
                throw new ArgumentException("unitOfWork is not type of " + typeof(DbReezeUnitOfWork).FullName, nameof(unitOfWork));
            }

            return ((DbReezeUnitOfWork)unitOfWork).Transaction;
        }
    }
}
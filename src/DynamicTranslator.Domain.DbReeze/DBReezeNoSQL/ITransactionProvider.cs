using DBreeze.Transactions;

namespace DynamicTranslator.Domain.DbReeze.DBReezeNoSQL
{
    public interface ITransactionProvider
    {
        Transaction Transaction { get; }
    }
}
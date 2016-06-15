using DBreeze.Transactions;

namespace DynamicTranslator.DbReeze.DBReezeNoSQL
{
    public interface ITransactionProvider
    {
        Transaction Transaction { get; }
    }
}
using DBreeze.Transactions;

namespace DynamicTranslator.DBReezeNoSQL
{
    public interface ITransactionProvider
    {
        Transaction Transaction { get; }
    }
}
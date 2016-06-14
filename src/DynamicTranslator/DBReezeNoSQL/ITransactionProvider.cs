using DBreeze.Transactions;

namespace DynamicTranslator.DBReezeNoSQL
{
    #region using

    

    #endregion

    public interface ITransactionProvider
    {
        Transaction Transaction { get; }
    }
}
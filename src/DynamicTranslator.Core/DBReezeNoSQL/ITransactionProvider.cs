using DBreeze.Transactions;

namespace DynamicTranslator.Core.DBReezeNoSQL
{
    #region using

    

    #endregion

    public interface ITransactionProvider
    {
        Transaction Transaction { get; }
    }
}
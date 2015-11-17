namespace DynamicTranslator.Core.DBReezeNoSQL
{
    #region using

    using DBreeze.Transactions;

    #endregion

    public interface ITransactionProvider
    {
        Transaction Transaction { get; }
    }
}
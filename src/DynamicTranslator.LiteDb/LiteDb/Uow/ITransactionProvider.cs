using LiteDB;

namespace DynamicTranslator.LiteDb.LiteDb.Uow
{
    public interface ITransactionProvider
    {
        LiteTransaction Transaction { get; }
    }
}
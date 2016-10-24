using LiteDB;

namespace DynamicTranslator.Domain.LiteDb.LiteDb.Uow
{
    public interface ITransactionProvider
    {
        LiteTransaction Transaction { get; }
    }
}

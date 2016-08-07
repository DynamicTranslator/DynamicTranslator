using DynamicTranslator.Domain.Model;
using DynamicTranslator.Domain.Repository;

using LiteDB;

namespace DynamicTranslator.LiteDb.LiteDb.Repository
{
    public class TranslateResultRepository : LiteDbRepositoryBase<CompositeTranslateResult, string>, ITranslateResultRepository
    {
        public TranslateResultRepository(LiteDatabase database) : base(database) {}
    }
}
using DynamicTranslator.Domain.Model;
using DynamicTranslator.Domain.Repository;

using LiteDB;

namespace DynamicTranslator.Domain.LiteDb.LiteDb.Repository
{
    public class TranslateResultRepository : LiteDbRepositoryBase<CompositeTranslateResult, string>, ITranslateResultRepository
    {
        public TranslateResultRepository(LiteDatabase database) : base(database) {}
    }
}
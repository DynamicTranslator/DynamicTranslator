using DynamicTranslator.Domain.Model;

using LiteDB;

namespace DynamicTranslator.Domain.LiteDb.LiteDb.Mapper
{
    public static class LiteDbMap
    {
        public static void Initialize()
        {
            var mapper = BsonMapper.Global;
            mapper.TrimWhitespace = true;
            mapper.SerializeNullValues = true;
            mapper.EmptyStringToNull = true;

            mapper.Entity<CompositeTranslateResult>()
                  .Id(x => x.Id)
                  .Field(x => x.CreateDate, nameof(CompositeTranslateResult.CreateDate))
                  .Field(x => x.Frequency, nameof(CompositeTranslateResult.Frequency))
                  .Field(x => x.SearchText, nameof(CompositeTranslateResult.SearchText))
                  .Index(x => x.Id, true);
        }
    }
}
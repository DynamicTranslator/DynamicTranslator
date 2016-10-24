using Abp.Domain.Repositories;

using DynamicTranslator.Domain.Model;

namespace DynamicTranslator.Domain.Repository
{
    public interface ITranslateResultRepository : IRepository<CompositeTranslateResult, string> {}
}

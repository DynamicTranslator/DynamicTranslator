using System.Threading.Tasks;

using DynamicTranslator.Domain.Uow;
using DynamicTranslator.Orchestrators.Model;

namespace DynamicTranslator.Service.Result
{
    #region using

    

    #endregion

    public interface IResultService : IApplicationService
    {
        [UnitOfWork]
        CompositeTranslateResult Get(string key);

        [UnitOfWork]
        Task<CompositeTranslateResult> GetAsync(string key);

        [UnitOfWork]
        CompositeTranslateResult Save(string key, CompositeTranslateResult translateResult);

        [UnitOfWork]
        CompositeTranslateResult SaveAndUpdateFrequency(string key, CompositeTranslateResult translateResult);

        [UnitOfWork]
        Task<CompositeTranslateResult> SaveAndUpdateFrequencyAsync(string key, CompositeTranslateResult translateResult);

        [UnitOfWork]
        Task<CompositeTranslateResult> SaveAsync(string key, CompositeTranslateResult translateResult);
    }
}
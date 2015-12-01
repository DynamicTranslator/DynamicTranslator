namespace DynamicTranslator.Core.Service.Result
{
    #region using

    using System.Threading.Tasks;
    using Domain.Uow;
    using Orchestrators;
    using Orchestrators.Model;

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
using DynamicTranslator.Core.Dependency.Markers;

namespace DynamicTranslator.Core.Domain.Uow
{
    #region using

    

    #endregion

    /// <summary>
    ///     Defines a unit of work.
    ///     Use <see cref="IUnitOfWorkManager.Begin()" /> to start a new unit of work.
    /// </summary>
    public interface IUnitOfWork : IActiveUnitOfWork, IUnitOfWorkCompleteHandle, ITransientDependency
    {
        /// <summary>
        ///     Unique id of this UOW.
        /// </summary>
        string Id { get; }

        /// <summary>
        ///     Reference to the outer UOW if exists.
        /// </summary>
        IUnitOfWork Outer { get; set; }

        /// <summary>
        ///     Begins the unit of work with given options.
        /// </summary>
        /// <param name="options">Unit of work options</param>
        void Begin(UnitOfWorkOptions options);
    }
}
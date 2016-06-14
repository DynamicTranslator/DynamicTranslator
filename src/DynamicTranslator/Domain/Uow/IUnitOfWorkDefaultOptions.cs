using System;
using System.Transactions;

using IsolationLevel = System.Data.IsolationLevel;

namespace DynamicTranslator.Domain.Uow
{
    #region using

    

    #endregion

    /// <summary>
    ///     Used to get/set default options for a unit of work.
    /// </summary>
    public interface IUnitOfWorkDefaultOptions
    {
        /// <summary>
        ///     Gets/sets isolation level of transaction.
        ///     This is used if <see cref="IsTransactional" /> is true.
        /// </summary>
        IsolationLevel? IsolationLevel { get; set; }

        /// <summary>
        ///     Should unit of works be transactional.
        ///     Default: true.
        /// </summary>
        bool IsTransactional { get; set; }

        /// <summary>
        ///     Scope option.
        /// </summary>
        TransactionScopeOption Scope { get; set; }

        /// <summary>
        ///     Gets/sets a timeout value for unit of works.
        /// </summary>
        TimeSpan? Timeout { get; set; }
    }
}
namespace DynamicTranslator.Core.Domain.Uow
{
    #region using

    using System;
    using System.Transactions;
    using IsolationLevel = System.Data.IsolationLevel;

    #endregion

    /// <summary>
    ///     Unit of work options.
    /// </summary>
    public class UnitOfWorkOptions
    {
        /// <summary>
        ///     This option should be set to <see cref="TransactionScopeAsyncFlowOption.Enabled" />
        ///     if unit of work is used in an async scope.
        /// </summary>
        public TransactionScopeAsyncFlowOption? AsyncFlowOption { get; set; }

        /// <summary>
        ///     If this UOW is transactional, this option indicated the isolation level of the transaction.
        ///     Uses default value if not supplied.
        /// </summary>
        public IsolationLevel? IsolationLevel { get; set; }

        /// <summary>
        ///     Is this UOW transactional?
        ///     Uses default value if not supplied.
        /// </summary>
        public bool? IsTransactional { get; set; }

        /// <summary>
        ///     Scope option.
        /// </summary>
        public TransactionScopeOption? Scope { get; set; }

        /// <summary>
        ///     Timeout of UOW As milliseconds.
        ///     Uses default value if not supplied.
        /// </summary>
        public TimeSpan? Timeout { get; set; }

        internal void FillDefaultsForNonProvidedOptions(IUnitOfWorkDefaultOptions defaultOptions)
        {
            //TODO: Do not change options object..?

            if (!IsTransactional.HasValue)
            {
                IsTransactional = defaultOptions.IsTransactional;
            }

            if (!Scope.HasValue)
            {
                Scope = defaultOptions.Scope;
            }

            if (!Timeout.HasValue && defaultOptions.Timeout.HasValue)
            {
                Timeout = defaultOptions.Timeout.Value;
            }

            if (!IsolationLevel.HasValue && defaultOptions.IsolationLevel.HasValue)
            {
                IsolationLevel = defaultOptions.IsolationLevel.Value;
            }
        }
    }
}
using System;
using System.Transactions;

using DynamicTranslator.Dependency.Markers;

using IsolationLevel = System.Data.IsolationLevel;

namespace DynamicTranslator.Domain.Uow
{
    internal class UnitOfWorkDefaultOptions : IUnitOfWorkDefaultOptions, ISingletonDependency
    {
        public UnitOfWorkDefaultOptions()
        {
            IsTransactional = true;
            Scope = TransactionScopeOption.Required;
        }

        /// <inheritdoc />
        public IsolationLevel? IsolationLevel { get; set; }

        /// <inheritdoc />
        public bool IsTransactional { get; set; }

        public TransactionScopeOption Scope { get; set; }

        /// <inheritdoc />
        public TimeSpan? Timeout { get; set; }
    }
}
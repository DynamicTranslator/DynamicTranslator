using System;
using System.Threading.Tasks;

using DynamicTranslator.Exception;
using DynamicTranslator.Extensions;

namespace DynamicTranslator.Domain.Uow
{
    #region using

    

    #endregion

    /// <summary>
    ///     Base for all Unit Of Work classes.
    /// </summary>
    public abstract class UnitOfWorkBase : IUnitOfWork
    {
        /// <summary>
        ///     A reference to the exception if this unit of work failed.
        /// </summary>
        private System.Exception _exception;

        /// <summary>
        ///     Is <see cref="Begin" /> method called before?
        /// </summary>
        private bool _isBeginCalledBefore;

        /// <summary>
        ///     Is <see cref="Complete" /> method called before?
        /// </summary>
        private bool _isCompleteCalledBefore;

        /// <summary>
        ///     Is this unit of work successfully completed.
        /// </summary>
        private bool _succeed;

        /// <summary>
        ///     Constructor.
        /// </summary>
        protected UnitOfWorkBase(IUnitOfWorkDefaultOptions defaultOptions)
        {
            DefaultOptions = defaultOptions;

            Id = Guid.NewGuid().ToString("N");
        }

        /// <summary>
        ///     Gets default UOW options.
        /// </summary>
        protected IUnitOfWorkDefaultOptions DefaultOptions { get; private set; }

        /// <inheritdoc />
        public event EventHandler Completed;

        /// <inheritdoc />
        public event EventHandler Disposed;

        /// <inheritdoc />
        public event EventHandler<UnitOfWorkFailedEventArgs> Failed;

        /// <inheritdoc />
        public void Begin(UnitOfWorkOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            PreventMultipleBegin();
            Options = options; //TODO: Do not set options like that, instead make a copy?

            BeginUow();
        }

        public void Complete()
        {
            PreventMultipleComplete();
            try
            {
                CompleteUow();
                _succeed = true;
                OnCompleted();
            }
            catch (System.Exception ex)
            {
                _exception = ex;
                throw;
            }
        }

        /// <inheritdoc />
        public async Task CompleteAsync()
        {
            PreventMultipleComplete();
            try
            {
                await CompleteUowAsync();
                _succeed = true;
                OnCompleted();
            }
            catch (System.Exception ex)
            {
                _exception = ex;
                throw;
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            IsDisposed = true;

            if (!_succeed)
            {
                OnFailed(_exception);
            }

            DisposeUow();
            OnDisposed();
        }

        /// <inheritdoc />
        public abstract void SaveChanges();

        /// <inheritdoc />
        public abstract Task SaveChangesAsync();

        /// <summary>
        ///     Reference to current session.
        /// </summary>
        public string Id { get; }

        /// <summary>
        ///     Gets a value indicates that this unit of work is disposed or not.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <inheritdoc />
        public UnitOfWorkOptions Options { get; private set; }

        public IUnitOfWork Outer { get; set; }

        /// <summary>
        ///     Concrete Unit of work classes should implement this
        ///     method in order to disable a filter.
        ///     Should not call base method since it throws <see cref="NotImplementedException" />.
        /// </summary>
        /// <param name="filterName">Filter name</param>
        protected virtual void ApplyDisableFilter(string filterName)
        {
            throw new NotImplementedException("DisableFilter is not implemented for " + GetType().FullName);
        }

        /// <summary>
        ///     Concrete Unit of work classes should implement this
        ///     method in order to enable a filter.
        ///     Should not call base method since it throws <see cref="NotImplementedException" />.
        /// </summary>
        /// <param name="filterName">Filter name</param>
        protected virtual void ApplyEnableFilter(string filterName)
        {
            throw new NotImplementedException("EnableFilter is not implemented for " + GetType().FullName);
        }

        /// <summary>
        ///     Concrete Unit of work classes should implement this
        ///     method in order to set a parameter's value.
        ///     Should not call base method since it throws <see cref="NotImplementedException" />.
        /// </summary>
        /// <param name="filterName">Filter name</param>
        protected virtual void ApplyFilterParameterValue(string filterName, string parameterName, object value)
        {
            throw new NotImplementedException("SetFilterParameterValue is not implemented for " + GetType().FullName);
        }

        /// <summary>
        ///     Should be implemented by derived classes to start UOW.
        /// </summary>
        protected abstract void BeginUow();

        /// <summary>
        ///     Should be implemented by derived classes to complete UOW.
        /// </summary>
        protected abstract void CompleteUow();

        /// <summary>
        ///     Should be implemented by derived classes to complete UOW.
        /// </summary>
        protected abstract Task CompleteUowAsync();

        /// <summary>
        ///     Should be implemented by derived classes to dispose UOW.
        /// </summary>
        protected abstract void DisposeUow();

        /// <summary>
        ///     Called to trigger <see cref="Completed" /> event.
        /// </summary>
        protected virtual void OnCompleted()
        {
            Completed.InvokeSafely(this);
        }

        /// <summary>
        ///     Called to trigger <see cref="Disposed" /> event.
        /// </summary>
        protected virtual void OnDisposed()
        {
            Disposed.InvokeSafely(this);
        }

        /// <summary>
        ///     Called to trigger <see cref="Failed" /> event.
        /// </summary>
        /// <param name="exception">Exception that cause failure</param>
        protected virtual void OnFailed(System.Exception exception)
        {
            Failed.InvokeSafely(this, new UnitOfWorkFailedEventArgs(exception));
        }

        private void PreventMultipleBegin()
        {
            if (_isBeginCalledBefore)
            {
                throw new BusinessException("This unit of work has started before. Can not call Start method more than once.");
            }

            _isBeginCalledBefore = true;
        }

        private void PreventMultipleComplete()
        {
            if (_isCompleteCalledBefore)
            {
                throw new BusinessException("Complete is called before!");
            }

            _isCompleteCalledBefore = true;
        }
    }
}
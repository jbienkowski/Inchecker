using Inchecker.Common.Interfaces;
using System;

namespace Inchecker.Common.Base
{
    /// <summary>
    /// Provides base implementation for view models that support closing.
    /// </summary>
    public abstract class ClosableViewModel : BaseViewModel, IClosableViewModel
    {
        /// <summary>
        /// Raises when the view should be closed.
        /// </summary>
        public event EventHandler CloseRequested;

        /// <summary>
        /// Raises a CloseRequested event.
        /// </summary>
        protected virtual void OnCloseRequested()
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Requests to close the view.
        /// </summary>
        public virtual void RequestClose()
        {
            // Requests to close.
            OnCloseRequested();
        }
    }

    /// <summary>
    /// Provides base implementation for view models that support closing with event arguments.
    /// </summary>
    public abstract class ClosableViewModel<T> : BaseViewModel, IClosableViewModel
        where T : EventArgs
    {
        /// <summary>
        /// Raises when the view should be closed.
        /// </summary>
        public event EventHandler<T> CloseRequested;

        /// <summary>
        /// Raises a CloseRequested event.
        /// </summary>
        /// <param name="args">The event arguments.</param>
        protected virtual void OnCloseRequested(T args)
        {
            CloseRequested?.Invoke(this, args);

            SimpleCloseRequested?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Requests to close the view.
        /// </summary>
        public abstract void RequestClose();


        #region IClosableViewModel Implementation

        event EventHandler SimpleCloseRequested;

        event EventHandler IClosableViewModel.CloseRequested
        {
            add { SimpleCloseRequested += value; }
            remove { SimpleCloseRequested -= value; }
        }

        void IClosableViewModel.RequestClose()
        {
            RequestClose();
        }

        #endregion
    }
}

using System;

namespace Inchecker.Common.Interfaces
{
    /// <summary>
    /// Represents a view model that supports closing.
    /// </summary>
    public interface IClosableViewModel
    {
        /// <summary>
        /// Raises when the view should be closed.
        /// </summary>
        /// <remarks>
        /// This event is raised in the UI thread that is bound to the view model.
        /// </remarks>
        event EventHandler CloseRequested;

        /// <summary>
        /// Requests to close the view.
        /// </summary>
        /// <remarks>
        /// This method must be called in the UI thread that is bound to the view model.
        /// </remarks>
        void RequestClose();
    }
}

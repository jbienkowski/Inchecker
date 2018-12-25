using System.Windows;

namespace Inchecker.Common.UICommon
{
    /// <summary>
    /// Provides owner window information.
    /// </summary>
    public class OwnerWindowInfo
    {
        #region Null Singleton

        static readonly OwnerWindowInfo _null = new OwnerWindowInfo(ownerWindow: null);

        /// <summary>
        /// Represents owner window information with a null owner window.
        /// </summary>
        public static OwnerWindowInfo Null { get { return _null; } }

        #endregion

        /// <summary>
        /// Initializes a new window.
        /// </summary>
        /// <param name="ownerWindow">The owner window or null if no owner window.</param>
        public OwnerWindowInfo(Window ownerWindow)
        {
            OwnerWindow = ownerWindow;
        }

        /// <summary>
        /// The owner window or null if no owner window.
        /// </summary>
        public Window OwnerWindow { get; }
    }
}

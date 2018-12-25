using Inchecker.Common.Interfaces;
using Inchecker.Common.UICommon;
using System;
using System.Linq;
using System.Windows;

namespace Inchecker.Common
{
    public static class Extensions
    {
        /// <summary>
        /// Shows a window with the specified view model.
        /// </summary>
        /// <param name="window">The window to show.</param>
        /// <param name="viewModel">The view model to use.</param>
        /// <param name="isDialog">Whether the ShowDialog method is used.</param>
        /// <param name="owner">The owner window information. If it is null, the default is used.</param>
        public static void ShowWithViewModel(Window window, object viewModel, bool isDialog, OwnerWindowInfo owner = null)
        {
            if (window == null)
                throw new ArgumentNullException(nameof(window));

            // Initializes the window.
            window.Owner = owner != null
                         ? owner.OwnerWindow
                         : Application.Current.Windows.Cast<Window>().FirstOrDefault(p => p.IsActive);
            window.DataContext = viewModel;

            // Handles closing by the view model.
            var closableViewModel = viewModel as IClosableViewModel;
            if (closableViewModel != null)
            {
                // Handles events for closing.
                bool isCloseRequested = false;
                window.Closing += (_, args) =>
                {
                    // The close must be controlled by the view model.
                    if (isCloseRequested == false)
                    {
                        args.Cancel = true;
                        window.Dispatcher.BeginInvoke(new Action(() => closableViewModel.RequestClose()));
                    }
                };

                EventHandler closeRequestedHandler = (_, __) =>
                {
                    isCloseRequested = true;
                    window.Close();
                };

                window.Closed += (_, __) => closableViewModel.CloseRequested -= closeRequestedHandler;
                closableViewModel.CloseRequested += closeRequestedHandler;
            }

            // Handles disposing resources.
            var disposableViewModel = viewModel as IDisposable;
            if (disposableViewModel != null)
                window.Closed += (_, __) => disposableViewModel.Dispose();

            // Shows the window.
            if (isDialog)
                window.ShowDialog();
            else
                window.Show();
        }

        /// <summary>
        /// Shows a dialog with the specified view model.
        /// </summary>
        /// <param name="window">The window to show.</param>
        /// <param name="viewModel">The view model to use.</param>
        /// <param name="owner">The owner window information. If it is null, the default is used.</param>
        public static void ShowDialogWithViewModel(this Window window, object viewModel, OwnerWindowInfo owner = null)
        {
            ShowWithViewModel
            (
                window: window,
                viewModel: viewModel,
                owner: owner,
                isDialog: true
            );
        }
    }
}

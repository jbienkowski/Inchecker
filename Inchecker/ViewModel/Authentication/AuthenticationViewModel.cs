using GalaSoft.MvvmLight.CommandWpf;
using Inchecker.Common.Base;
using Inchecker.Common.Enums;
using Inchecker.ViewModel.EntityWrappers;
using System;
using System.Windows.Input;

namespace Inchecker.ViewModel.Authentication
{
    public class AuthenticationViewModel : ClosableViewModel, IDisposable
    {
        #region Commands
        public ICommand KeypadButtonPressedCommand { get; private set; }
        #endregion Commands

        #region Fields
        public bool UserAuthenticated = false;
        private string pinEntered = string.Empty;
        private PersonViewModel _personToAuthenticate;
        #endregion Fields

        public AuthenticationViewModel(AuthenticationModes authMode, PersonViewModel person)
        {
            _personToAuthenticate = person;

            KeypadButtonPressedCommand = new RelayCommand<string>((p) =>
            {
                lock (this)
                {
                    pinEntered += p;
                    PinCheck(_personToAuthenticate.Pin);
                }
            });
        }

        private void PinCheck(string pin)
        {
            if (pin.Length == pinEntered.Length)
            {
                if (pin == pinEntered)
                    UserAuthenticated = true;

                base.RequestClose();
            }
            else if (!pin.StartsWith(pinEntered))
            {
                base.RequestClose();
            }
        }

        public void Dispose()
        {
            
        }
    }
}

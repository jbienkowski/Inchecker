using GalaSoft.MvvmLight.CommandWpf;
using Inchecker.Common.Base;
using Inchecker.ViewModel.Config;
using Inchecker.ViewModel.Inchecker;
using Inchecker.View.Config;
using Inchecker.View.Inchecker;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Inchecker.View.Log;
using Inchecker.ViewModel.Log;
using Inchecker.ViewModel.EntityWrappers;
using Inchecker.ViewModel.Authentication;
using Inchecker.View.Authentication;
using Inchecker.Common;
using Inchecker.Entities;
using System.Linq;

namespace Inchecker.ViewModel.MainWindow
{
    public class MainWindowViewModel : ClosableViewModel
    {
        #region Properties
        private UserControl _currentPage;
        public UserControl CurrentPage
        {
            get { return _currentPage; }
            set { SetProperty(ref _currentPage, value, () => CurrentPage); }
        }

        private UserControl _incheckerPage;
        private UserControl IncheckerPage
        {
            get
            {
                if (_incheckerPage == null)
                {
                    _incheckerPage = new IncheckerView();
                    _incheckerPage.DataContext = new IncheckerViewModel();
                }
                return _incheckerPage;
            }
            set { SetProperty(ref _incheckerPage, value, () => IncheckerPage); }
        }

        private UserControl _logPage;
        private UserControl LogPage
        {
            get
            {
                if (_logPage == null)
                {
                    _logPage = new LogView();
                    _logPage.DataContext = new LogViewModel();
                }
                return _logPage;
            }
            set { SetProperty(ref _logPage, value, () => LogPage); }
        }

        private UserControl _configPage;
        private UserControl ConfigPage
        {
            get
            {
                if (_configPage == null)
                {
                    _configPage = new ConfigView();
                    _configPage.DataContext = new ConfigViewModel();
                }
                return _configPage;
            }
            set { SetProperty(ref _configPage, value, () => ConfigPage); }
        }

        private PersonViewModel _loggedPerson;
        public PersonViewModel LoggedPerson
        {
            get { return _loggedPerson; }
            set { SetProperty(ref _loggedPerson, value, () => LoggedPerson); }
        }

        public bool AdvancedFunctionsEnabled
        {
            get
            {
                return (LoggedPerson != null && LoggedPerson.Role == Common.Enums.Roles.Admin);
            }
        }
        #endregion Properties

        #region Fields
        #endregion Fields

        #region Commands
        public ICommand IncheckerPageCommand { get; private set; }
        public ICommand LogPageCommand { get; private set; }
        public ICommand ConfigPageCommand { get; private set; }
        public ICommand CloseCommand { get; private set; }
        public ICommand LoginCommand { get; private set; }
        public ICommand LogoutCommand { get; private set; }
        #endregion Commands

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindowViewModel()
        {
            LoginCommand = new RelayCommand(LoginCommandExecute, LoginCommandCanExecute);
            LogoutCommand = new RelayCommand(LogoutCommandExecute, LogoutCommandCanExecute);

            IncheckerPageCommand = new RelayCommand(() =>
            {
                if (CurrentPage == IncheckerPage)
                    return;
                CurrentPage = IncheckerPage;
            });

            LogPageCommand = new RelayCommand(() =>
            {
                if (CurrentPage == LogPage)
                    return;
                CurrentPage = LogPage;
            });

            ConfigPageCommand = new RelayCommand(() =>
            {
                if (CurrentPage == ConfigPage)
                    return;
                CurrentPage = ConfigPage;
            });

            CloseCommand = new RelayCommand(() =>
            {
                Application.Current.Shutdown();
            });

            CurrentPage = IncheckerPage;
        }

        private void LoginCommandExecute()
        {
            PersonViewModel admin;

            using (var ctx = new IncheckerDbCtx())
            {
                admin = new PersonViewModel(ctx.Persons.First(n => n.Role == Common.Enums.Roles.Admin));
            }

            if (string.IsNullOrEmpty(admin.Pin))
            {
                LoggedPerson = admin;
            }
            else
            {
                using (var dataCtx = new AuthenticationViewModel(Common.Enums.AuthenticationModes.Admin, admin))
                {
                    var auth = new AuthenticationView();
                    auth.ShowDialogWithViewModel(dataCtx);

                    if (dataCtx.UserAuthenticated == true)
                    {
                        LoggedPerson = admin;
                    }
                    else
                    {
                        LoggedPerson = null;
                    }
                }
            }

            RaisePropertyChanged(nameof(AdvancedFunctionsEnabled));
        }

        private bool LoginCommandCanExecute()
        {
            return LoggedPerson == null;
        }

        private void LogoutCommandExecute()
        {
            LoggedPerson = null;
            CurrentPage = IncheckerPage;
            RaisePropertyChanged(nameof(AdvancedFunctionsEnabled));
        }

        private bool LogoutCommandCanExecute()
        {
            return LoggedPerson != null;
        }
    }
}

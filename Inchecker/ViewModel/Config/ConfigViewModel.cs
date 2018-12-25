using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Inchecker.Common.Base;
using Inchecker.Common.Settings;
using Inchecker.Common.Static;
using Inchecker.Entities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System;
using System.Text.RegularExpressions;
using Inchecker.ViewModel.EntityWrappers;

namespace Inchecker.ViewModel.Config
{
    public class ConfigViewModel : BaseViewModel, IDataErrorInfo
    {
        #region Properties
        private ObservableCollection<PersonViewModel> _persons = new ObservableCollection<PersonViewModel>();
        public ObservableCollection<PersonViewModel> Persons
        {
            get { return _persons; }
            set { SetProperty(ref _persons, value, () => Persons); }
        }

        private PersonViewModel _selectedPerson;
        public PersonViewModel SelectedPerson
        {
            get { return _selectedPerson; }
            set
            {
                SetProperty(ref _selectedPerson, value, () => SelectedPerson);
                RaisePropertyChanged(nameof(ControlsEnabled));
            }
        }

        public bool ControlsEnabled
        {
            get { return SelectedPerson != null; }
        }

        private string _logRetention;
        public string LogRetention
        {
            get { return _logRetention ; }
            set
            {
                SetProperty(ref _logRetention, value, () => LogRetention);
            }
        }
        #endregion Properties

        #region Fields
        #endregion Fields

        #region Commands
        public ICommand SaveGeneralSettingsCommand { get; private set; }
        public ICommand AddPersonCommand { get; private set; }
        public ICommand RemovePersonCommand { get; private set; }
        public ICommand CommitCommand { get; private set; }
        #endregion Commands

        /// <summary>
        /// Constructor
        /// </summary>
        public ConfigViewModel()
        {
            SaveGeneralSettingsCommand = new RelayCommand(SaveGeneralSettingsCommandExecute, SaveGeneralSettingsCommandCanExecute);
            AddPersonCommand = new RelayCommand(AddPersonCommandExecute);
            RemovePersonCommand = new RelayCommand(RemovePersonCommandExecute, RemovePersonCommandCanExecute);
            CommitCommand = new RelayCommand(CommitCommandExecute, CommitCommandCanExecute);

            LogRetention = Convert.ToString(SettingManager.LogRetention);
            RefreshPersons();
        }

        private void SaveGeneralSettingsCommandExecute()
        {
            if (SaveGeneralSettingsCommandCanExecute())
            {
                SettingManager.LogRetention = Convert.ToInt32(LogRetention);
            }
        }

        private bool SaveGeneralSettingsCommandCanExecute()
        {
            return PropertiesWithError.Count == 0;
        }

        private void AddPersonCommandExecute()
        {
            var newPerson = new PersonViewModel();
            Persons.Add(newPerson);
            SelectedPerson = newPerson;
        }

        private bool CommitCommandCanExecute()
        {
            return !Persons.Any(n => n.PropertiesWithError.Count != 0);
        }

        private void CommitCommandExecute()
        {
            if (!CommitCommandCanExecute())
                return;

            List<int> keep = new List<int>();

            using (var ctx = new IncheckerDbCtx())
            {
                foreach (var p1 in Persons)
                {
                    var pdb = ctx.Persons.FirstOrDefault(n => n.Id == p1.Id);
                    if (pdb == null)
                    {
                        ctx.Persons.Add(new Person() {
                            FirstName = p1.FirstName
                            , LastName = p1.LastName
                            , FloorNr = p1.FloorNr
                            , RoomNr = p1.RoomNr
                            , Pin = p1.Pin
                            , Role = p1.Role
                            , CheckinTime = null });
                    }
                    else
                    {
                        keep.Add(p1.Id);

                        pdb.FirstName = p1.FirstName;
                        pdb.LastName = p1.LastName;
                        pdb.FloorNr = p1.FloorNr;
                        pdb.RoomNr = p1.RoomNr;
                        pdb.Pin = p1.Pin;
                        pdb.Role = p1.Role;

                        foreach (var incheck in ctx.Inchecks.Where(n => n.PersonId == p1.Id))
                        {
                            incheck.FirstName = p1.FirstName;
                            incheck.LastName = p1.LastName;
                        }
                    }
                }

                foreach (var p2 in ctx.Persons.Where(n => n.Id != 0 && !keep.Contains(n.Id)))
                {
                    ctx.Persons.Remove(p2);
                    
                    foreach (var incheck in ctx.Inchecks.Where(n => n.PersonId == p2.Id))
                    {
                        ctx.Inchecks.Remove(incheck);
                    }
                }

                ctx.SaveChanges();
                RefreshPersons();
                Messenger.Default.Send(new NotificationMessage(MessengerNotificationMessages.RefreshPersonsInOut));
            }
        }

        private bool RemovePersonCommandCanExecute()
        {
            return SelectedPerson != null && SelectedPerson.Role != Common.Enums.Roles.Admin;
        }

        private void RemovePersonCommandExecute()
        {
            if (!RemovePersonCommandCanExecute())
                return;

            Persons.Remove(SelectedPerson);
        }

        private void RefreshPersons()
        {
            Persons.Clear();

            using (var ctx = new IncheckerDbCtx())
            {
                foreach (var person in ctx.Persons)
                {
                    Persons.Add(new PersonViewModel(person));
                }
            }
        }

        public string Error
        {
            get
            {
                return string.Empty;
            }
        }

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case nameof(LogRetention):
                        if (!Regex.IsMatch(LogRetention.ToString(), @"^\d{1,4}$"))
                        {
                            TryAddPropertyWithError(nameof(LogRetention));
                            return "Log retention must be between 0 and 9999 days!";
                        }
                        else { TryRemovePropertyWithError(nameof(LogRetention)); }
                        break;
                    default:
                        return null;
                }

                return null;
            }
        }
    }
}

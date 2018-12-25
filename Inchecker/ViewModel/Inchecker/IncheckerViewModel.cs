using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Inchecker.Common;
using Inchecker.Common.Base;
using Inchecker.Common.Static;
using Inchecker.Entities;
using Inchecker.View.Authentication;
using Inchecker.ViewModel.Authentication;
using Inchecker.ViewModel.EntityWrappers;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Inchecker.ViewModel.Inchecker
{
    public class IncheckerViewModel : BaseViewModel
    {
        private ObservableCollection<Person> _personsIn = new ObservableCollection<Person>();
        public ObservableCollection<Person> PersonsIn
        {
            get { return _personsIn; }
            set { SetProperty(ref _personsIn, value, () => PersonsIn); }
        }

        private ObservableCollection<Person> _personsOut = new ObservableCollection<Person>();
        public ObservableCollection<Person> PersonsOut
        {
            get { return _personsOut; }
            set { SetProperty(ref _personsOut, value, () => PersonsOut); }
        }

        #region Commands
        public ICommand CheckInCommand { get; private set; }
        public ICommand CheckOutCommand { get; private set; }
        #endregion Commands

        /// <summary>
        /// Constructor
        /// </summary>
        public IncheckerViewModel()
        {
            CheckInCommand = new RelayCommand<int>(CheckInCommandExecute);
            CheckOutCommand = new RelayCommand<int>(CheckOutCommandExecute);

            Messenger.Default.Register<NotificationMessage>(this, (message) =>
            {
                if (message.Notification == MessengerNotificationMessages.RefreshPersonsInOut)
                    RefreshPersonsInOut();
            });
            
            RefreshPersonsInOut();
        }

        private void CheckInCommandExecute(int id)
        {
            using (var ctx = new IncheckerDbCtx())
            {
                var person = ctx.Persons.FirstOrDefault(n => n.Id == id);
                if (person == null)
                    return;

                if (string.IsNullOrEmpty(person.Pin))
                {
                    person.CheckinTime = DateTime.Now;
                    ctx.SaveChanges();
                }
                else
                {
                    using (var dataCtx = new AuthenticationViewModel(Common.Enums.AuthenticationModes.CheckInOut
                    , new PersonViewModel(person)))
                    {
                        var auth = new AuthenticationView();
                        auth.ShowDialogWithViewModel(dataCtx);

                        if (dataCtx.UserAuthenticated == true)
                        {
                            person.CheckinTime = DateTime.Now;
                            ctx.SaveChanges();
                        }
                    }
                }
            }

            RefreshPersonsInOut();
        }

        private void CheckOutCommandExecute(int id)
        {
            using (var ctx = new IncheckerDbCtx())
            {
                var person = ctx.Persons.FirstOrDefault(n => n.Id == id);
                if (person == null)
                    return;

                if (string.IsNullOrEmpty(person.Pin))
                {
                    ctx.Inchecks.Add(new Incheck()
                    {
                        PersonId = person.Id
                        , FirstName = person.FirstName
                        , LastName = person.LastName
                        , CheckinTime = person.CheckinTime
                        , CheckoutTime = DateTime.Now
                    });

                    person.CheckinTime = null;
                    ctx.SaveChanges();
                }
                else
                {
                    using (var dataCtx = new AuthenticationViewModel(Common.Enums.AuthenticationModes.CheckInOut
                    , new PersonViewModel(person)))
                    {
                        var auth = new AuthenticationView();
                        auth.ShowDialogWithViewModel(dataCtx);

                        if (dataCtx.UserAuthenticated == true)
                        {
                            ctx.Inchecks.Add(new Incheck()
                            {
                                PersonId = person.Id
                                , FirstName = person.FirstName
                                , LastName = person.LastName
                                , CheckinTime = person.CheckinTime
                                , CheckoutTime = DateTime.Now
                            });

                            person.CheckinTime = null;
                            ctx.SaveChanges();
                        }
                    }
                }
            }

            RefreshPersonsInOut();

            Messenger.Default.Send<Incheck>(null);
        }

        private void RefreshPersonsInOut()
        {
            PersonsIn.Clear();
            PersonsOut.Clear();
            using (var ctx = new IncheckerDbCtx())
            {
                foreach (var person in ctx.Persons.Where(n => n.CheckinTime == null))
                {
                    PersonsOut.Add(person);
                }

                foreach (var person in ctx.Persons.Where(n => n.CheckinTime != null))
                {
                    PersonsIn.Add(person);
                }
            }
        }
    }
}

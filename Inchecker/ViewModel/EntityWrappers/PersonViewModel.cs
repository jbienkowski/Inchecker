using Inchecker.Common.Base;
using Inchecker.Common.Enums;
using Inchecker.Entities;
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Inchecker.ViewModel.EntityWrappers
{
    public class PersonViewModel : BaseViewModel, IDataErrorInfo
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set
            {
                SetProperty(ref _id, value, () => Id);
            }
        }

        private string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                SetProperty(ref _firstName, value, () => FirstName);
            }
        }

        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set
            {
                SetProperty(ref _lastName, value, () => LastName);
            }
        }

        private string _floorNr;
        public string FloorNr
        {
            get { return _floorNr; }
            set
            {
                SetProperty(ref _floorNr, value, () => FloorNr);
            }
        }

        private string _roomNr;
        public string RoomNr
        {
            get { return _roomNr; }
            set
            {
                SetProperty(ref _roomNr, value, () => RoomNr);
            }
        }

        private string _pin;
        public string Pin
        {
            get { return _pin; }
            set
            {
                SetProperty(ref _pin, value, () => Pin);
            }
        }

        private Roles _role;
        public Roles Role
        {
            get { return _role; }
            set
            {
                SetProperty(ref _role, value, () => Role);
            }
        }

        private DateTime? _checkinTime;
        public DateTime? CheckinTime
        {
            get { return _checkinTime; }
            set
            {
                SetProperty(ref _checkinTime, value, () => CheckinTime);
            }
        }

        public PersonViewModel()
        {
            Id = 0;
            FirstName = "FirstName";
            LastName = "LastName";
            FloorNr = "0";
            RoomNr = "0";
            Pin = "1234";
            CheckinTime = null;
            Role = Roles.User;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="person"></param>
        public PersonViewModel(Person person)
        {
            Id = person.Id;
            FirstName = person.FirstName;
            LastName = person.LastName;
            FloorNr = person.FloorNr;
            RoomNr = person.RoomNr;
            Pin = person.Pin;
            CheckinTime = person.CheckinTime;
            Role = person.Role;
        }

        public override string ToString()
        {
            return $"{FirstName} {LastName}";
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
                    case nameof(Pin):
                        if (!Regex.IsMatch(Pin, @"^\d{0,10}$"))
                        {
                            TryAddPropertyWithError(nameof(Pin));
                            return "Pin is expected to be a sequence of max 10 digits!";
                        }
                        else { TryRemovePropertyWithError(nameof(Pin)); }
                        break;
                    case nameof(FirstName):
                        if (!Regex.IsMatch(FirstName, @"^[\w ]{1,50}$"))
                        {
                            TryAddPropertyWithError(nameof(FirstName));
                            return "Length must be between 1 and 50 characters!";
                        }
                        else { TryRemovePropertyWithError(nameof(FirstName)); }
                        break;
                    case nameof(LastName):
                        if (!Regex.IsMatch(LastName, @"^[\w ]{1,50}$"))
                        {
                            TryAddPropertyWithError(nameof(LastName));
                            return "Length must be between 1 and 50 characters!";
                        }
                        else { TryRemovePropertyWithError(nameof(LastName)); }
                        break;
                    case nameof(FloorNr):
                        if (!Regex.IsMatch(FloorNr, @"^[\w ]{0,50}$"))
                        {
                            TryAddPropertyWithError(nameof(FloorNr));
                            return "Length must be smaller than 50 characters!";
                        }
                        else { TryRemovePropertyWithError(nameof(FloorNr)); }
                        break;
                    case nameof(RoomNr):
                        if (!Regex.IsMatch(RoomNr, @"^[\w ]{0,50}$"))
                        {
                            TryAddPropertyWithError(nameof(RoomNr));
                            return "Length must be smaller than 50 characters!";
                        }
                        else { TryRemovePropertyWithError(nameof(RoomNr)); }
                        break;
                    default:
                        return null;
                }
                return null;
            }
        }
    }
}

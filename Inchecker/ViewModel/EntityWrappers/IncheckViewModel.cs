using Inchecker.Common.Base;
using Inchecker.Entities;
using System;

namespace Inchecker.ViewModel.EntityWrappers
{
    public class IncheckViewModel : BaseViewModel
    {
        #region Properties
        private int _id;
        public int Id
        {
            get { return _id; }
            set
            {
                SetProperty(ref _id, value, () => Id);
            }
        }

        private int _personId;
        public int PersonId
        {
            get { return _personId; }
            set
            {
                SetProperty(ref _personId, value, () => PersonId);
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

        private DateTime? _checkin;
        public DateTime? Checkin
        {
            get { return _checkin; }
            set
            {
                SetProperty(ref _checkin, value, () => Checkin);
            }
        }

        private DateTime? _checkout;
        public DateTime? Checkout
        {
            get { return _checkout; }
            set
            {
                SetProperty(ref _checkout, value, () => Checkout);
            }
        }

        private string _timeInOffice;
        public string TimeInOffice
        {
            get { return _timeInOffice; }
            set
            {
                SetProperty(ref _timeInOffice, value, () => TimeInOffice);
            }
        }
        #endregion Properties

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="incheck"></param>
        public IncheckViewModel(Incheck incheck)
        {
            Id = incheck.Id;
            PersonId = incheck.PersonId;
            FirstName = incheck.FirstName;
            LastName = incheck.LastName;
            Checkin = incheck.CheckinTime;
            Checkout = incheck.CheckoutTime;
            TimeInOffice = GetTimeInOffice(Checkin, Checkout);
        }

        private string GetTimeInOffice(DateTime? checkIn, DateTime? checkOut)
        {
            if (checkOut != null && checkIn != null)
            {
                var timeSpan = checkOut - checkIn;
                return $"{timeSpan.Value.Days} days, {timeSpan.Value.Hours} hours, {timeSpan.Value.Minutes} minutes, {timeSpan.Value.Seconds} seconds";
            }
            else
            {
                return string.Empty;
            }
        }
    }
}

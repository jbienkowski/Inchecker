using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Inchecker.Common.Base
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T backingField, T Value, Expression<Func<T>> propertyExpression)
        {
            var changed = !EqualityComparer<T>.Default.Equals(backingField, Value);

            if (changed)
            {
                backingField = Value;
                RaisePropertyChanged(ExtractPropertyName(propertyExpression));
            }

            return changed;
        }

        private static string ExtractPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            var memberExp = propertyExpression.Body as MemberExpression;

            if (memberExp == null)
            {
                throw new ArgumentException("Expression must be a MemberExpression.", "propertyExpression");
            }

            return memberExp.Member.Name;
        }

        private List<string> _propertiesWithError = new List<string>();
        public List<string> PropertiesWithError
        {
            get { return _propertiesWithError; }
            set
            {
                SetProperty(ref _propertiesWithError, value, () => PropertiesWithError);
            }
        }

        public void TryAddPropertyWithError(string propertyName)
        {
            if (!PropertiesWithError.Contains(propertyName))
                PropertiesWithError.Add(propertyName);
        }

        public void TryRemovePropertyWithError(string propertyName)
        {
            if (PropertiesWithError.Contains(propertyName))
                PropertiesWithError.Remove(propertyName);
        }
    }
}

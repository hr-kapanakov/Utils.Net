using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Utils.Net.ViewModels
{
    /// <summary>
    /// ViewModel base class, implements OnPropertyChanged.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        #region PropertyChanged

        /// <summary>
        /// Property changed event handler.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Invokes property changed event for a specified changed property.
        /// </summary>
        /// <param name="propertyName">Changed property.</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            CheckPropertyName(propertyName);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Checks if the passed property name is valid, otherwise throws an exception.
        /// </summary>
        /// <param name="propertyName">Property name to be checked.</param>
        private void CheckPropertyName(string propertyName)
        {
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                throw new Exception($"Could not find property \"{propertyName}\"");
            }
        }

        #endregion

        /// <summary>
        /// Sets a non-automatic property backing field and invokes property changed event.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="backingField">Reference to the backing field.</param>
        /// <param name="value">Value to be set to the backing field.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>Returns true if the backing field value is successfully updated.</returns>
        protected bool SetPropertyBackingField<T>(
            ref T backingField, T value, [CallerMemberName] string propertyName = null)
        {
            if (!typeof(T).IsValueType)
            {
                if (ReferenceEquals(backingField, value))
                {
                    return false;
                }
            }
            else
            {
                if (backingField.Equals(value))
                {
                    return false;
                }
            }

            backingField = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}

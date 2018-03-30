using System;
using Utils.Net.Common;

namespace Utils.Net.ViewModels
{
    /// <summary>
    /// Common ViewModel for a checkable item.
    /// </summary>
    public class CheckableItemViewModel : ViewModelBase
    {
        private string name;
        /// <summary>
        /// Gets or sets the name of the item.
        /// </summary>
        public string Name
        {
            get => name;
            set => SetPropertyBackingField(ref name, value);
        }

        private bool isChecked;
        /// <summary>
        /// Gets or sets a value indicating whether the item is checked.
        /// </summary>
        public bool IsChecked
        {
            get => isChecked;
            set
            {
                if (SetPropertyBackingField(ref isChecked, value))
                {
                    IsCheckedChanged?.Invoke(this, new EventArgs<bool>(isChecked));
                }
            }
        }

        /// <summary>
        /// Occurs when the <see cref="IsChecked"/> property get changed.
        /// </summary>
        public event EventHandler<EventArgs<bool>> IsCheckedChanged;


        /// <summary>
        /// Initializes a new instance of the <see cref="CheckableItemViewModel"/> class.
        /// </summary>
        /// <param name="name">Name of the item.</param>
        /// <param name="isChecked">Indicate whether the item is checked; default false.</param>
        public CheckableItemViewModel(string name, bool isChecked = false)
        {
            this.name = name;
            this.isChecked = isChecked;
        }


        /// <summary>
        /// Overrides the default ToString method. 
        /// </summary>
        /// <returns>String representation of the object.</returns>
        public override string ToString()
        {
            return $"{Name}";
        }
    }


    /// <summary>
    /// Common ViewModel for a checkable item with generic value.
    /// </summary>
    /// <typeparam name="T">Type of the item's value.</typeparam>
    public class CheckableItemViewModel<T> : CheckableItemViewModel
    {
        private T value;
        /// <summary>
        /// Gets or sets the generic value of the item.
        /// </summary>
        public T Value
        {
            get => value;
            set => SetPropertyBackingField(ref this.value, value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckableItemViewModel{T}"/> class.
        /// </summary>
        /// <param name="name">Name of the item.</param>
        /// <param name="value">Value of the item.</param>
        /// <param name="isChecked">Indicate whether the item is checked; default false.</param>
        public CheckableItemViewModel(string name, T value, bool isChecked = false)
            : base(name, isChecked)
        { 
            this.value = value;
        }
        

        /// <summary>
        /// Overrides the default ToString method.
        /// </summary>
        /// <returns>String representation of the object.</returns>
        public override string ToString()
        {
            return $"{Name}: {Value}";
        }
    }
}

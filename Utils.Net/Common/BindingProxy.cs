using System.Windows;

namespace Utils.Net.Common
{
    /// <summary>
    /// Represent a <see cref="Freezable"/> object which allow to bind to <see cref="Data"/> property.
    /// </summary>
    public class BindingProxy : Freezable
    {
        /// <summary>
        /// Identifies the <see cref="Data"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register(
                nameof(Data), 
                typeof(object), 
                typeof(BindingProxy),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the bindable data.
        /// </summary>
        public object Data
        {
            get => GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        /// <summary>
        /// Cast <see cref="Data"/> to the set type.
        /// </summary>
        /// <typeparam name="T">Desired type.</typeparam>
        /// <returns>Casted data.</returns>
        public T As<T>() => (T)Data;


        /// <summary>
        /// Creates a new instance of the <see cref="Freezable"/> derived class.
        /// </summary>
        /// <returns>The new instance.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }
    }
}

using System;

namespace Utils.Net.Common
{
    /// <summary>
    /// Represents a class that provides a value to use for events.
    /// </summary>
    /// <typeparam name="T">Type of the event data value.</typeparam>
    public class EventArgs<T> : EventArgs
    {
        /// <summary>
        /// Gets the value of the event data.
        /// </summary>
        public T Value { get; private set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="EventArgs{T}" /> class.
        /// </summary>
        /// <param name="value">Data of the event.</param>
        public EventArgs(T value)
        {
            Value = value;
        }
    }
}

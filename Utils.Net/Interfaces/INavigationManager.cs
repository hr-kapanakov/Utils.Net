using System;
using System.Windows.Controls;
using Utils.Net.Common;

namespace Utils.Net.Interfaces
{
    /// <summary>
    /// Provides functionality to navigate between different <see cref="Control"/>.
    /// </summary>
    public interface INavigationManager
    {
        /// <summary>
        /// Gets the currently selected <see cref="Control"/>.
        /// </summary>
        Control CurrentControl { get; }

        /// <summary>
        /// Event trigger if the a navigation command has been called.
        /// </summary>
        event EventHandler<EventArgs<Control>> CurrentControlChanged;

        /// <summary>
        /// Gets a value indicating whether it's possible to navigate forward.
        /// </summary>
        bool CanNavigateForward { get; }

        /// <summary>
        /// Gets a value indicating whether it's possible to navigate backward.
        /// </summary>
        bool CanNavigateBackward { get; }

        /// <summary>
        /// Gets or sets the capacity of the backward stack before reducing it.
        /// Default value is 10.
        /// </summary>
        int BackwardStackCapacity { get; set; }

        /// <summary>
        /// Gets or sets the count of the elements to be reduced when backward stack capacity get reached.
        /// Default value is 5.
        /// </summary>
        int BackwardStackReductionCount { get; set; }


        /// <summary>
        /// Navigates to specified control and sets is as <see cref="CurrentControl"/>.
        /// </summary>
        /// <param name="control">Control to navigate to.</param>
        /// <param name="clearForwardStack">[optional] Clears the forward stack.</param>
        /// <returns>True if successfully navigate to the new <see cref="Control"/>; otherwise, false.</returns>
        bool NavigateTo(Control control, bool clearForwardStack = true);

        /// <summary>
        /// Navigates forward to the top element of the forward stack.
        /// </summary>
        /// <returns>True if successfully navigate forward; otherwise, false.</returns>
        bool NavigateForward();

        /// <summary>
        /// Navigates backward to the top element of the backward stack.
        /// </summary>
        /// <param name="clearForwardStack">[optional] Clears the forward stack.</param>
        /// <returns>True if successfully navigate backward; otherwise, false.</returns>
        bool NavigateBackward(bool clearForwardStack = false);
    }
}

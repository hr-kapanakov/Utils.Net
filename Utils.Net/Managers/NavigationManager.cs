using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Utils.Net.Common;

namespace Utils.Net.Managers
{
    /// <summary>
    /// Provides functionality to navigate between different <see cref="Control"/>.
    /// </summary>
    public class NavigationManager
    {
        #region Members

        /// <summary>
        /// private member representing the backward stack
        /// </summary>
        private Stack<Control> backwardStack = new Stack<Control>();

        /// <summary>
        /// private member representing the forward stack
        /// </summary>
        private Stack<Control> forwardStack = new Stack<Control>();

        #endregion

        /// <summary>
        /// Gets the currently selected <see cref="Control"/>.
        /// </summary>
        public Control CurrentControl { get; private set; }

        /// <summary>
        /// Event trigger if the a navigation command has been called.
        /// </summary>
        public event EventHandler<EventArgs<Control>> CurrentControlChanged;

        /// <summary>
        /// Gets or sets the capacity of the backward stack before reducing it.
        /// Default value is 10.
        /// </summary>
        public int BackwardStackCapacity { get; set; } = 10;

        /// <summary>
        /// Gets or sets the count of the elements to be reduced when backward stack capacity get reached.
        /// Default value is 5.
        /// </summary>
        public int BackwardStackReductionCount { get; set; } = 5;


        /// <summary>
        /// Navigates to specified control and sets is as <see cref="CurrentControl"/>.
        /// </summary>
        /// <param name="control">Control to navigate to.</param>
        /// <param name="clearForwardStack">[optional] Clears the forward stack.</param>
        public void NavigateTo(Control control, bool clearForwardStack = true)
        {
            if (control != null && CurrentControl != null)
            {
                if (control.GetType() == CurrentControl.GetType() && 
                    control.DataContext.Equals(CurrentControl.DataContext))
                {
                    return;
                }
            }

            if (clearForwardStack)
            {
                forwardStack.Clear();
                if (CurrentControl != null && CanAddToBackwardStack(control))
                {
                    backwardStack.Push(CurrentControl);

                    // if the backward stack gets bigger than set capacity 
                    // we will clean set count of elements to reduce memory consumption
                    if (backwardStack.Count >= BackwardStackCapacity)
                    {
                        var backStackArray = backwardStack.ToArray();
                        backwardStack.Clear();
                        var startIdx = Math.Max(backStackArray.Length - BackwardStackReductionCount, 0);
                        for (int i = startIdx; i < backStackArray.Length; i++)
                        {
                            backwardStack.Push(backStackArray[i]);
                        }
                    }
                }
            }

            CurrentControl = control;
            OnCurrentControlChanged();
        }

        /// <summary>
        /// Navigates forward to the top element of the forward stack.
        /// </summary>
        public void NavigateForward()
        {
            if (forwardStack.Count == 0)
            {
                return;
            }

            if (CurrentControl != null)
            {
                backwardStack.Push(CurrentControl);
            }

            NavigateTo(forwardStack.Pop(), false);
        }

        /// <summary>
        /// Navigates backward to the top element of the backward stack.
        /// </summary>
        /// <param name="clearForwardStack">[optional] Clears the forward stack.</param>
        public void NavigateBackward(bool clearForwardStack = false)
        {
            if (backwardStack.Count == 0)
            {
                return;
            }

            var currentControl = CurrentControl;

            NavigateTo(backwardStack.Pop(), clearForwardStack);

            if (!clearForwardStack)
            {
                forwardStack.Push(currentControl);
            }
        }


        /// <summary>
        /// Checks if an <see cref="Control"/> can be added to the backward stack.
        /// Returns false if the passed <see cref="Control"/> is equal to <see cref="CurrentControl"/>.
        /// </summary>
        /// <param name="control">Control to be added to the backward stack.</param>
        /// <returns>True if the control can be added to the backward stack.</returns>
        private bool CanAddToBackwardStack(Control control)
        {
            if (backwardStack.Count == 0)
            {
                return true;
            }

            return CurrentControl != control;
        }

        /// <summary>
        /// Handles changes of the controls and sets <see cref="CurrentControl"/>.
        /// </summary>
        private void OnCurrentControlChanged()
        {
            CurrentControlChanged?.Invoke(this, new EventArgs<Control>(CurrentControl));
        }
    }
}

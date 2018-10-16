using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using Utils.Net.Common;
using Utils.Net.Dialogs;
using Utils.Net.Helpers;
using Utils.Net.Interfaces;
using Utils.Net.Interfaces.Tutorial;

namespace Utils.Net.Managers
{
    /// <summary>
    /// Provides functionality to create a interactive tutorial.
    /// </summary>
    public class TutorialManager : IDisposable, ITutorialManager
    {
        #region Attached Dependency Properties

        /// <summary>
        /// Identifies the attached RelativeFontSize dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemIdProperty =
            DependencyProperty.RegisterAttached(
                "ItemId",
                typeof(string),
                typeof(TutorialManager));

        /// <summary>
        /// Get value of the <see cref="ItemIdProperty"/> dependency property.
        /// </summary>
        /// <param name="obj">Dependency object from which the value will be get.</param>
        /// <returns>Value of the <see cref="ItemIdProperty"/> dependency property.</returns>
        public static string GetItemId(DependencyObject obj)
        {
            return (string)obj.GetValue(ItemIdProperty);
        }

        /// <summary>
        /// Set value to the <see cref="ItemIdProperty"/> dependency property.
        /// </summary>
        /// <param name="obj">Dependency object to which the value will be set.</param>
        /// <param name="value">Value which will be set to the <see cref="ItemIdProperty"/> dependency property.</param>
        public static void SetItemId(DependencyObject obj, string value)
        {
            obj.SetValue(ItemIdProperty, value);
        }

        #endregion

        private Window owner;
        private Window Owner
        {
            get => owner;
            set
            {
                if (owner != value)
                {
                    if (owner != null)
                    {
                        owner.LocationChanged -= Owner_LocationChanged;
                        owner.Deactivated -= Owner_IsActiveChanged;
                        owner.Activated -= Owner_IsActiveChanged;
                        owner.IsEnabled = true;
                    }

                    owner = value;

                    if (owner != null)
                    {
                        owner.IsEnabled = false;
                        owner.Activated += Owner_IsActiveChanged;
                        owner.Deactivated += Owner_IsActiveChanged;
                        owner.LocationChanged += Owner_LocationChanged;
                    }
                }
            }
        }

        private Popup popup;
        private Popup border;


        // TODO: tests
        /// <summary>
        /// Gets a <see cref="Dictionary{TKey, TValue}"/> with different steps in the tutorial.<para/>
        /// The key of the dictionary is the <see cref="ItemIdProperty"/> of the target <see cref="FrameworkElement"/>.
        /// If the key is invalid (no element with such ItemId) the <see cref="TutorialDialog"/> wan't highlight target.
        /// </summary>
        public Dictionary<string, ITutorialItem> Items { get; } = new Dictionary<string, ITutorialItem>();


        private int currentItemIdx = -1;

        /// <summary>
        /// Gets the <see cref="TutorialManager.ItemIdProperty"/> of the current target <see cref="FrameworkElement"/>.
        /// </summary>
        public string CurrentItemId => currentItemIdx >= 0 ? Items.ElementAt(currentItemIdx).Key : null;

        /// <summary>
        /// Gets the current <see cref="ITutorialItem"/>.
        /// </summary>
        public ITutorialItem CurrentItem => currentItemIdx >= 0 ? Items.ElementAt(currentItemIdx).Value : null;

        /// <summary>
        /// Gets or sets a value indicating whether "Don't show again" check box should be visible.
        /// </summary>
        public bool ShowDontShowAgainCheckbox { get; set; } = true;

        private bool dontShowAgain;
        /// <summary>
        /// Gets or sets a value indicating whether "Don't show again" check box is checked in the <see cref="TutorialDialog"/>.
        /// </summary>
        public bool DontShowAgain
        {
            get => dontShowAgain;
            set
            {
                if (dontShowAgain != value)
                {
                    dontShowAgain = value;
                    DontShowAgainChanged?.Invoke(this, dontShowAgain.ToEventArgs());
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the tutorial is already started.
        /// </summary>
        public bool IsStarted => currentItemIdx >= 0;


        /// <summary>
        /// Occurs when the tutorial starts.
        /// </summary>
        public event EventHandler Started;

        /// <summary>
        /// Occurs when the current item of the tutorial get changed.
        /// </summary>
        public event EventHandler<EventArgs<ITutorialItem>> CurrentItemChanged;

        /// <summary>
        /// Occurs when the tutorial stops.
        /// </summary>
        public event EventHandler Stopped;

        /// <summary>
        /// Occurs when the <see cref="DontShowAgain"/> property get changed.
        /// </summary>
        public event EventHandler<EventArgs<bool>> DontShowAgainChanged;


        /// <summary>
        /// Start the tutorial.
        /// </summary>
        /// <returns>True if the tutorial get started successfuly; false if <see cref="Items"/> is empty.</returns>
        public bool Start()
        {
            if (Items.Count == 0)
            {
                return false;
            }

            Started?.Invoke(this, EventArgs.Empty);
            currentItemIdx = 0;
            ShowCurrentItem();
            CurrentItemChanged?.Invoke(this, CurrentItem.ToEventArgs());
            return true;
        }

        /// <summary>
        /// Go to previous step of the tutorial. Stop the tutorial if the begining is reached.
        /// </summary>
        /// <returns>True if successful; false if there isn't previous step.</returns>
        public bool Previous()
        {
            if (currentItemIdx < 0)
            {
                return false;
            }

            SetPopupsIsOpen(false); // close popups

            currentItemIdx--;
            if (currentItemIdx >= 0)
            {
                CurrentItem.BeforeShow?.Invoke(CurrentItem);
                ShowCurrentItem();
            }
            else // if we reached the begining
            {
                Stop();
                return false;
            }

            CurrentItemChanged?.Invoke(this, CurrentItem.ToEventArgs());
            return true;
        }

        /// <summary>
        /// Go to next step of the tutorial. Stop the tutorial if the end is reached.
        /// </summary>
        /// <returns>True if successful; false if there isn't next step.</returns>
        public bool Next()
        {
            if (currentItemIdx < 0)
            {
                return false;
            }

            SetPopupsIsOpen(false); // close popups

            currentItemIdx++;
            if (currentItemIdx < Items.Count)
            {
                CurrentItem.BeforeShow?.Invoke(CurrentItem);
                ShowCurrentItem();
            }
            else // if we reached the end
            {
                Stop();
                return false;
            }

            CurrentItemChanged?.Invoke(this, CurrentItem.ToEventArgs());
            return true;
        }

        /// <summary>
        /// Stop the tutorial.
        /// </summary>
        public void Stop()
        {
            SetPopupsIsOpen(false);
            Owner = null;

            currentItemIdx = -1;
            CurrentItemChanged?.Invoke(this, CurrentItem.ToEventArgs());
            Stopped?.Invoke(this, EventArgs.Empty);
        }


        private void SetPopupsIsOpen(bool isOpen)
        {
            if (popup != null)
            {
                popup.IsOpen = isOpen;
            }

            if (border != null)
            {
                border.IsOpen = isOpen;
            }
        }

        private void ShowCurrentItem()
        {
            var target = GetTarget(out var window);
            Owner = window;

            if (target != null && !target.IsLoaded)
            {
                target.Loaded += Target_Loaded;
                return;
            }

            var nextButtonText = "Next";
            var previousButtonText = "Previous";
            var isCloseButtonVisible = true;
            if (currentItemIdx == 0) // first item
            {
                nextButtonText = "Begin";
                previousButtonText = "Close";
                isCloseButtonVisible = false;
            }
            if (currentItemIdx == Items.Count - 1) // last item
            {
                nextButtonText = "Close";
                isCloseButtonVisible = false;
            }

            if (target == null)
            {
                // show in the center if there is no target
                popup = TutorialDialog.ShowPopup(
                    this,
                    CurrentItem.Title,
                    CurrentItem.Text,
                    PlacementMode.Center,
                    Application.Current.MainWindow,
                    previousButtonText,
                    nextButtonText,
                    isCloseButtonVisible,
                    ShowDontShowAgainCheckbox);

                border = null;
            }
            else
            {
                // show first the border, because the popup may be over it
                if (CurrentItem.BorderColor.HasValue)
                {
                    border = TutorialDialog.ShowBorder(target, CurrentItem.BorderColor.Value, new Thickness(2));
                }

                popup = TutorialDialog.ShowPopup(
                    this,
                    CurrentItem.Title,
                    CurrentItem.Text,
                    CurrentItem.PlacementMode,
                    target,
                    previousButtonText,
                    nextButtonText,
                    isCloseButtonVisible,
                    ShowDontShowAgainCheckbox);
            }
            SetPopupsIsOpen(Owner.IsActive);
        }

        private FrameworkElement GetTarget(out Window window)
        {
            FrameworkElement target = null;
            foreach (var w in Application.Current.Windows.OfType<Window>())
            {
                target = w.FindVisualDescendant<FrameworkElement>(t => GetItemId(t) == CurrentItemId);
                if (target != null)
                {
                    window = w;
                    return target;
                }
            }

            window = Application.Current.MainWindow;
            return null;
        }

        private void Target_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement target)
            {
                target.Loaded -= Target_Loaded;
            }
            ShowCurrentItem();
        }


        private void Owner_IsActiveChanged(object sender, EventArgs e)
        {
            SetPopupsIsOpen(Owner.IsActive);
        }

        private void Owner_LocationChanged(object sender, EventArgs e)
        {
            // refresh popups positions
            if (popup != null)
            {
                var placement = popup.Placement;
                popup.Placement = PlacementMode.Custom;
                popup.Placement = placement;
            }

            if (border != null)
            {
                var placement = border.Placement;
                border.Placement = PlacementMode.Custom;
                border.Placement = placement;
            }
        }


        #region IDisposable Support

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Stop();
            }
        }

        #endregion
    }
}

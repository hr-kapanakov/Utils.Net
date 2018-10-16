using System;
using System.Collections.Generic;
using System.Windows;
using Utils.Net.Common;
using Utils.Net.Dialogs;
using Utils.Net.Interfaces.Tutorial;
using Utils.Net.Managers;

namespace Utils.Net.Interfaces
{
    /// <summary>
    /// Provides functionality to create a interactive tutorial.
    /// </summary>
    public interface ITutorialManager
    {
        /// <summary>
        /// Gets a <see cref="Dictionary{TKey, TValue}"/> with different steps in the tutorial.<para/>
        /// The key of the dictionary is the <see cref="TutorialManager.ItemIdProperty"/> of the target <see cref="FrameworkElement"/>.
        /// If the key is invalid (no element with such ItemId) the <see cref="TutorialDialog"/> wan't highlight target.
        /// </summary>
        Dictionary<string, ITutorialItem> Items { get; }

        /// <summary>
        /// Gets the <see cref="TutorialManager.ItemIdProperty"/> of the current target <see cref="FrameworkElement"/>.
        /// </summary>
        string CurrentItemId { get; }

        /// <summary>
        /// Gets the current <see cref="ITutorialItem"/>.
        /// </summary>
        ITutorialItem CurrentItem { get; }

        /// <summary>
        /// Gets or sets a value indicating whether "Don't show again" check box should be visible.
        /// </summary>
        bool ShowDontShowAgainCheckbox { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether "Don't show again" button is checked in the <see cref="TutorialDialog"/>.
        /// </summary>
        bool DontShowAgain { get; set; }

        /// <summary>
        /// Gets a value indicating whether the tutorial is already started.
        /// </summary>
        bool IsStarted { get; }


        /// <summary>
        /// Occurs when the tutorial starts.
        /// </summary>
        event EventHandler Started;

        /// <summary>
        /// Occurs when the current item of the tutorial get changed.
        /// </summary>
        event EventHandler<EventArgs<ITutorialItem>> CurrentItemChanged;

        /// <summary>
        /// Occurs when the tutorial stops.
        /// </summary>
        event EventHandler Stopped;

        /// <summary>
        /// Occurs when the <see cref="DontShowAgain"/> property get changed.
        /// </summary>
        event EventHandler<EventArgs<bool>> DontShowAgainChanged;


        /// <summary>
        /// Start the tutorial.
        /// </summary>
        /// <returns>True if the tutorial get started successfuly; false if <see cref="Items"/> is empty.</returns>
        bool Start();

        /// <summary>
        /// Go to previous step of the tutorial. Stop the tutorial if the begining is reached.
        /// </summary>
        /// <returns>True if successful; false if there isn't previous step.</returns>
        bool Previous();

        /// <summary>
        /// Go to next step of the tutorial. Stop the tutorial if the end is reached.
        /// </summary>
        /// <returns>True if successful; false if there isn't next step.</returns>
        bool Next();

        /// <summary>
        /// Stop the tutorial.
        /// </summary>
        void Stop();

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        void Dispose();
    }
}

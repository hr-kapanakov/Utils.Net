using System;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Utils.Net.Interfaces.Tutorial
{
    /// <summary>
    /// Interface of an item of tutorial.
    /// </summary>
    public interface ITutorialItem
    {
        /// <summary>
        /// Gets the title of the item.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Gets the text of the item.
        /// </summary>
        string Text { get; }

        /// <summary>
        /// Gets the color of the <see cref="System.Windows.Controls.Border"/>. If null there will be no border.
        /// </summary>
        Color? BorderColor { get; }

        /// <summary>
        /// Gets the placement position of the <see cref="Dialogs.TutorialDialog"/>.
        /// </summary>
        PlacementMode PlacementMode { get; }

        /// <summary>
        /// Gets the action that should be executed before showing the item.
        /// </summary>
        Action<ITutorialItem> BeforeShow { get; }
    }
}

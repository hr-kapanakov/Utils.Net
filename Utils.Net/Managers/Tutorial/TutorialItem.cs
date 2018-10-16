using System;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Utils.Net.Interfaces.Tutorial;

namespace Utils.Net.Managers.Tutorial
{
    /// <summary>
    /// Class representing an item of tutorial.
    /// </summary>
    public class TutorialItem : ITutorialItem
    {
        /// <summary>
        /// Gets the title of the item.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Gets the text of the item.
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Gets the color of the <see cref="System.Windows.Controls.Border"/>. If null there will be no border.
        /// </summary>
        public Color? BorderColor { get; }

        /// <summary>
        /// Gets the placement position of the <see cref="Dialogs.TutorialDialog"/>.
        /// </summary>
        public PlacementMode PlacementMode { get; }

        /// <summary>
        /// Gets the action that should be executed before showing the item.
        /// </summary>
        public Action<ITutorialItem> BeforeShow { get; }


        /// <summary>
        /// Initializes a new instance of the <see cref="TutorialItem" /> class.
        /// </summary>
        /// <param name="title">Title of the item.</param>
        /// <param name="text">Text of the item. Use '*' and '_' for bold and italic respectively (double to escape).</param>
        /// <param name="placementMode">The placement position of the <see cref="Dialogs.TutorialDialog"/>; default bottom.</param>
        /// <param name="beforeShow">Action to be executed before showing the item.</param>
        public TutorialItem(string title, string text, PlacementMode placementMode = PlacementMode.Bottom, Action<ITutorialItem> beforeShow = null) :
            this(title, text, Colors.Red, placementMode, beforeShow)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TutorialItem" /> class.
        /// </summary>
        /// <param name="title">Title of the item.</param>
        /// <param name="text">Text of the item. Use '*' and '_' for bold and italic respectively (double to escape).</param>
        /// <param name="borderColor">The color of the <see cref="System.Windows.Controls.Border"/>. If null there will be no border.</param>
        /// <param name="placementMode">The placement position of the <see cref="Dialogs.TutorialDialog"/>; default bottom.</param>
        /// <param name="beforeShow">Action to be executed before showing the item.</param>
        public TutorialItem(string title, string text, Color? borderColor, PlacementMode placementMode = PlacementMode.Bottom, Action<ITutorialItem> beforeShow = null)
        {
            Title = title;
            Text = text;
            BorderColor = borderColor;
            PlacementMode = placementMode;
            BeforeShow = beforeShow;
        }
    }
}

using System.ComponentModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Utils.Net.Adorners
{
    /// <summary>
    /// Class which provides methods for beautifying grid views column sorting.
    /// </summary>
    public class SortAdorner : Adorner
    {
        private static readonly Geometry AscGeometry =
                Geometry.Parse("M 0 4 L 3.5 0 L 7 4 Z");

        private static readonly Geometry DescGeometry =
                Geometry.Parse("M 0 0 L 3.5 4 L 7 0 Z");

        /// <summary>
        /// Gets the sorting direction.
        /// </summary>
        public ListSortDirection Direction { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SortAdorner"/> class.
        /// </summary>
        /// <param name="element">Adorned <see cref="UIElement"/>.</param>
        /// <param name="dir">Direction of sorting.</param>
        /// <remarks>Beautifies sorting of a column.</remarks>
        public SortAdorner(UIElement element, ListSortDirection dir)
            : base(element)
        {
            Direction = dir;
        }

        /// <summary>
        /// Renders the arrows which show the sort order.
        /// </summary>
        /// <param name="drawingContext">Drawing context.</param>
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (AdornedElement.RenderSize.Width < 20)
            {
                return;
            }

            var transform = new TranslateTransform(
                AdornedElement.RenderSize.Width - 15,
                (AdornedElement.RenderSize.Height - 5) / 2);
            drawingContext.PushTransform(transform);

            var geometry = AscGeometry;
            if (Direction == ListSortDirection.Descending)
            {
                geometry = DescGeometry;
            }

            drawingContext.DrawGeometry(Brushes.Black, null, geometry);

            drawingContext.Pop();
        }
    }
}

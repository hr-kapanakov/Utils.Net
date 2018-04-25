using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Utils.Net.Controls
{
    // https://uhimaniavwp.codeplex.com/SourceControl/latest#VirtualizingWrapPanel/VirtualizingWrapPanel.cs
    #region VirtualizingWrapPanel

    /// <summary>
    /// Virtualize child elements <see cref="System.Windows.Controls.WrapPanel"/>.
    /// </summary>
    public class VirtualizingWrapPanel : VirtualizingPanel, IScrollInfo
    {
        #region ItemSize

        #region ItemWidth

        /// <summary>
        /// <see cref="ItemWidth"/> identifier of the dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemWidthProperty =
            DependencyProperty.Register(
                nameof(ItemWidth),
                typeof(double),
                typeof(VirtualizingWrapPanel),
                new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure),
                new ValidateValueCallback(IsWidthHeightValid));

        /// <summary>
        /// Gets or sets the width of all the items contained in VirtualizingWrapPanel.
        /// </summary>
        [TypeConverter(typeof(LengthConverter)), Category("Common")]
        public double ItemWidth
        {
            get => (double)GetValue(ItemWidthProperty);
            set => SetValue(ItemWidthProperty, value);
        }

        #endregion

        #region ItemHeight

        /// <summary>
        /// <see cref="ItemHeight"/> identifier of the dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemHeightProperty =
            DependencyProperty.Register(
                nameof(ItemHeight),
                typeof(double),
                typeof(VirtualizingWrapPanel),
                new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure),
                new ValidateValueCallback(IsWidthHeightValid));

        /// <summary>
        /// Gets or sets the height of all the items contained in VirtualizingWrapPanel.
        /// </summary>
        [TypeConverter(typeof(LengthConverter)), Category("Common")]
        public double ItemHeight
        {
            get => (double)GetValue(ItemHeightProperty);
            set => SetValue(ItemHeightProperty, value);
        }

        #endregion

        #region IsWidthHeightValid

        /// <summary>
        /// <see cref="ItemWidth"/>, <see cref="ItemHeight"/> if the value set for
        /// callback to verify validity.
        /// </summary>
        /// <param name="value">The value set in the property.</param>
        /// <returns>True if the value is valid, false if it is invalid.</returns>
        private static bool IsWidthHeightValid(object value)
        {
            var d = (double)value;
            return double.IsNaN(d) || ((d >= 0) && !double.IsPositiveInfinity(d));
        }

        #endregion

        #endregion

        #region Orientation

        /// <summary>
        /// <see cref="Orientation"/> identifier of the dependency property.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty =
            WrapPanel.OrientationProperty.AddOwner(
                typeof(VirtualizingWrapPanel),
                new FrameworkPropertyMetadata(
                    Orientation.Horizontal, 
                    FrameworkPropertyMetadataOptions.AffectsMeasure,
                    new PropertyChangedCallback(OnOrientationChanged)));

        /// <summary>
        /// Gets or sets a value that specifies the direction in which child content is placed.
        /// </summary>
        [Category("Common")]
        public Orientation Orientation
        {
            get => (Orientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        /// <summary>
        /// <see cref="Orientation"/> callback called when the dependency property changes.
        /// </summary>
        /// <param name="d">Property value changed<see cref="System.Windows.DependencyObject"/>.</param>
        /// <param name="e">Event data issued by events tracking changes to the valid values of this property.</param>
        private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var panel = d as VirtualizingWrapPanel;
            panel.offset = default(Point);
            panel.InvalidateMeasure();
        }

        #endregion

        #region MeasureOverride, ArrangeOverride

        /// <summary>
        /// A dictionary that stores the position and size of the item at the specified index.
        /// </summary>
        private Dictionary<int, Rect> containerLayouts = new Dictionary<int, Rect>();

        /// <summary>
        /// Measure the size of the layout required for the child element and determine the size of the panel.
        /// </summary>
        /// <param name="availableSize">Available sizes that can be given to child elements.</param>
        /// <returns>Size required by this panel during layout.</returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            containerLayouts.Clear();

            var isAutoWidth = double.IsNaN(ItemWidth);
            var isAutoHeight = double.IsNaN(ItemHeight);
            var childAvailable = new Size(
                isAutoWidth ? double.PositiveInfinity : ItemWidth, 
                isAutoHeight ? double.PositiveInfinity : ItemHeight);
            var isHorizontal = Orientation == Orientation.Horizontal;

            var childrenCount = InternalChildren.Count;

            var itemsControl = ItemsControl.GetItemsOwner(this);
            if (itemsControl != null)
            {
                childrenCount = itemsControl.Items.Count;
            }

            var generator = new ChildGenerator(this);

            var x = 0.0;
            var y = 0.0;
            var lineSize = default(Size);
            var maxSize = default(Size);

            for (int i = 0; i < childrenCount; i++)
            {
                var childSize = ContainerSizeForIndex(i);

                // Adjust x, y with provisional size for intersection with viewport
                var isWrapped = isHorizontal ?
                    lineSize.Width + childSize.Width > availableSize.Width :
                    lineSize.Height + childSize.Height > availableSize.Height;
                if (isWrapped)
                {
                    x = isHorizontal ? 0 : x + lineSize.Width;
                    y = isHorizontal ? y + lineSize.Height : 0;
                }

                // If the child element is within the view port, 
                // child element is generated and the size is remeasured
                var itemRect = new Rect(x, y, childSize.Width, childSize.Height);
                var viewportRect = new Rect(offset, availableSize);
                if (itemRect.IntersectsWith(viewportRect))
                {
                    var child = generator.GetOrCreateChild(i);
                    child.Measure(childAvailable);
                    childSize = ContainerSizeForIndex(i);
                }

                // Remember the determined size
                containerLayouts[i] = new Rect(x, y, childSize.Width, childSize.Height);

                // Calculate lineSize and maxSize
                isWrapped = isHorizontal ?
                    lineSize.Width + childSize.Width > availableSize.Width :
                    lineSize.Height + childSize.Height > availableSize.Height;
                if (isWrapped)
                {
                    maxSize.Width = isHorizontal ? 
                        Math.Max(lineSize.Width, maxSize.Width) : 
                        maxSize.Width + lineSize.Width;
                    maxSize.Height = isHorizontal ? 
                        maxSize.Height + lineSize.Height : 
                        Math.Max(lineSize.Height, maxSize.Height);
                    lineSize = childSize;

                    isWrapped = isHorizontal ?
                        childSize.Width > availableSize.Width :
                        childSize.Height > availableSize.Height;
                    if (isWrapped)
                    {
                        maxSize.Width = isHorizontal ? 
                            Math.Max(childSize.Width, maxSize.Width) : 
                            maxSize.Width + childSize.Width;
                        maxSize.Height = isHorizontal ? 
                            maxSize.Height + childSize.Height : 
                            Math.Max(childSize.Height, maxSize.Height);
                        lineSize = default(Size);
                    }
                }
                else
                {
                    lineSize.Width = isHorizontal ? 
                        lineSize.Width + childSize.Width : 
                        Math.Max(childSize.Width, lineSize.Width);
                    lineSize.Height = isHorizontal ? 
                        Math.Max(childSize.Height, lineSize.Height) : 
                        lineSize.Height + childSize.Height;
                }

                x = isHorizontal ? lineSize.Width : maxSize.Width;
                y = isHorizontal ? maxSize.Height : lineSize.Height;
            }

            maxSize.Width = isHorizontal ? 
                Math.Max(lineSize.Width, maxSize.Width) : 
                maxSize.Width + lineSize.Width;
            maxSize.Height = isHorizontal ? 
                maxSize.Height + lineSize.Height : 
                Math.Max(lineSize.Height, maxSize.Height);

            extent = maxSize;
            viewport = availableSize;

            generator.CleanupChildren();
            generator.Dispose();

            if (ScrollOwner != null)
            {
                ScrollOwner.InvalidateScrollInfo();
            }

            return maxSize;
        }

        #region ChildGenerator

        /// <summary>
        /// <see cref="VirtualizingWrapPanel"/> manage items of.
        /// </summary>
        private class ChildGenerator : IDisposable
        {
            #region fields

            /// <summary>
            /// The target of generating the item <see cref="VirtualizingWrapPanel"/>.
            /// </summary>
            private VirtualizingWrapPanel owner;

            /// <summary>
            /// <see cref="owner"/> of <see cref="System.Windows.Controls.ItemContainerGenerator"/>.
            /// </summary>
            private IItemContainerGenerator generator;

            /// <summary>
            /// <see cref="generator"/> An object that tracks the lifetime of the generation process.
            /// </summary>
            private IDisposable generatorTracker;

            /// <summary>
            /// The index of the first element in the display range.
            /// </summary>
            private int firstGeneratedIndex;

            /// <summary>
            /// The index of the last element in the display range.
            /// </summary>
            private int lastGeneratedIndex;

            /// <summary>
            /// The next element to be generated next 
            /// <see cref="System.Windows.Controls.Panel.InternalChildren"/> the index in.
            /// </summary>
            private int currentGenerateIndex;

            #endregion

            #region _ctor

            /// <summary>
            /// Initializes a new instance of the <see cref="ChildGenerator" /> class.
            /// </summary>
            /// <param name="owner"> the target of generating the item<see cref="VirtualizingWrapPanel"/>.</param>
            public ChildGenerator(VirtualizingWrapPanel owner)
            {
                this.owner = owner;

                // ItemContainerGenerator it gets null unless you access InternalChildren before getting it
                var childrenCount = owner.InternalChildren.Count;
                generator = owner.ItemContainerGenerator;
            }

            /// <summary>
            /// Finalizes an instance of the <see cref="ChildGenerator" /> class.
            /// </summary>
            ~ChildGenerator()
            {
                Dispose();
            }

            /// <summary>
            /// End generation of items.
            /// </summary>
            public void Dispose()
            {
                if (generatorTracker != null)
                {
                    generatorTracker.Dispose();
                }
            }

            #endregion

            #region GetOrCreateChild

            /// <summary>
            /// Start generating items.
            /// </summary>
            /// <param name="index">The index of the item.</param>
            private void BeginGenerate(int index)
            {
                firstGeneratedIndex = index;
                var startPos = generator.GeneratorPositionFromIndex(index);
                currentGenerateIndex = (startPos.Offset == 0) ? startPos.Index : startPos.Index + 1;
                generatorTracker = generator.StartAt(startPos, GeneratorDirection.Forward, true);
            }

            /// <summary>
            /// Generate items as necessary and acquire items with the specified index.
            /// </summary>
            /// <param name="index">The index of the item to retrieve.</param>
            /// <returns>Item at the specified index.</returns>
            public UIElement GetOrCreateChild(int index)
            {
                if (generator == null)
                {
                    return owner.InternalChildren[index];
                }

                if (generatorTracker == null)
                {
                    BeginGenerate(index);
                }

                var child = generator.GenerateNext(out bool newlyRealized) as UIElement;
                if (newlyRealized)
                {
                    if (currentGenerateIndex >= owner.InternalChildren.Count)
                    {
                        owner.AddInternalChild(child);
                    }
                    else
                    {
                        owner.InsertInternalChild(currentGenerateIndex, child);
                    }

                    generator.PrepareItemContainer(child);
                }

                lastGeneratedIndex = index;
                currentGenerateIndex++;

                return child;
            }

            #endregion

            #region CleanupChildren

            /// <summary>
            /// Delete items outside the display range.
            /// </summary>
            public void CleanupChildren()
            {
                if (generator == null)
                {
                    return;
                }

                var children = owner.InternalChildren;

                for (int i = children.Count - 1; i >= 0; i--)
                {
                    var childPos = new GeneratorPosition(i, 0);
                    var index = generator.IndexFromGeneratorPosition(childPos);
                    if (index < firstGeneratedIndex || index > lastGeneratedIndex)
                    {
                        generator.Remove(childPos, 1);
                        owner.RemoveInternalChildRange(i, 1);
                    }
                }
            }

            #endregion
        }

        #endregion

        /// <summary>
        /// Place the child elements and determine the size of the panel.
        /// </summary>
        /// <param name="finalSize">The area at the end of the parent used to place the panel itself and child elements.</param>
        /// <returns>Actual size to use.</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            foreach (UIElement child in InternalChildren)
            {
                var index = (ItemContainerGenerator is ItemContainerGenerator gen) ? 
                    gen.IndexFromContainer(child) : 
                    InternalChildren.IndexOf(child);
                if (containerLayouts.ContainsKey(index))
                {
                    var layout = containerLayouts[index];
                    layout.Offset(offset.X * -1, offset.Y * -1);
                    child.Arrange(layout);
                }
            }

            return finalSize;
        }

        #endregion

        #region ContainerSizeForIndex

        /// <summary>
        /// Size of the element laid out just before.
        /// </summary>
        /// <remarks>
        /// <see cref="System.Windows.DataTemplate"/> on use, assuming that the sizes of all elements match,
        /// Used to estimate element size.
        /// </remarks>
        private Size prevSize = new Size(16, 16);

        /// <summary>
        /// Estimate the size of the item for the specified index without actually creating the item.
        /// </summary>
        /// <param name="index">The index of the item.</param>
        /// <returns>The estimated size of the item for the specified index.</returns>
        private Size ContainerSizeForIndex(int index)
        {
            var getSize = new Func<int, Size>(idx =>
            {
                UIElement item = null;
                var itemsOwner = ItemsControl.GetItemsOwner(this);
                var generator = ItemContainerGenerator as ItemContainerGenerator;

                if (itemsOwner == null || generator == null)
                {
                    // VirtualizingWrapPanel When used alone, returns its own item
                    if (InternalChildren.Count > idx)
                    {
                        item = InternalChildren[idx];
                    }
                }
                else
                {
                    // If the generator does not generate an item, use it if you can use Items
                    if (generator.ContainerFromIndex(idx) != null)
                    {
                        item = generator.ContainerFromIndex(idx) as UIElement;
                    }
                    else if (itemsOwner.Items.Count > idx)
                    {
                        item = itemsOwner.Items[idx] as UIElement;
                    }
                }

                if (item != null)
                {
                    // If the item size has already been measured, return its size
                    if (item.IsMeasureValid)
                    {
                        return item.DesiredSize;
                    }

                    // If item size is not measured, use recommended value
                    if (item is FrameworkElement i)
                    {
                        return new Size(i.Width, i.Height);
                    }
                }

                // Use the previous measured value if there is one
                if (containerLayouts.ContainsKey(idx))
                {
                    return containerLayouts[idx].Size;
                }

                // If a valid size could not be obtained, return the size of the previous item
                return prevSize;
            });

            var size = getSize(index);

            // Adjust if ItemWidth, ItemHeight is specified
            if (!double.IsNaN(ItemWidth))
            {
                size.Width = ItemWidth;
            }

            if (!double.IsNaN(ItemHeight))
            {
                size.Height = ItemHeight;
            }

            return prevSize = size;
        }

        #endregion

        #region OnItemsChanged

        /// <summary>
        /// This panel <see cref="System.Windows.Controls.ItemsControl"/> associated with
        /// <see cref="System.Windows.Controls.ItemsControl.Items"/> when the collection is changed
        /// callback to be called.
        /// </summary>
        /// <param name="sender">I generated an event <see cref="System.Object"/></param>
        /// <param name="args">Event data.</param>
        /// <remarks>
        /// <see cref="System.Windows.Controls.ItemsControl.Items"/> When changed
        /// <see cref="System.Windows.Controls.Panel.InternalChildren"/> As well.
        /// </remarks>
        protected override void OnItemsChanged(object sender, ItemsChangedEventArgs args)
        {
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                case NotifyCollectionChangedAction.Move:
                    RemoveInternalChildRange(args.Position.Index, args.ItemUICount);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    offset = new Point(0, 0);

                    if (ScrollOwner != null)
                    {
                        ScrollOwner.InvalidateScrollInfo();
                    }

                    InvalidateMeasure();
                    break;
            }
        }

        #endregion

        #region IScrollInfo Members

        #region Extent

        /// <summary>
        /// Size of the extent.
        /// </summary>
        private Size extent = default(Size);

        /// <summary>
        /// Gets the vertical width of the extent.
        /// </summary>
        public double ExtentHeight => extent.Height;

        /// <summary>
        /// Get the width of the extent.
        /// </summary>
        public double ExtentWidth => extent.Width;

        #endregion Extent

        #region Viewport

        /// <summary>
        /// Size of the viewport.
        /// </summary>
        private Size viewport = default(Size);

        /// <summary>
        /// Get the vertical width of the viewport for this content.
        /// </summary>
        public double ViewportHeight => viewport.Height;

        /// <summary>
        /// Get the width of the viewport for this content.
        /// </summary>
        public double ViewportWidth => viewport.Width;

        #endregion

        #region Offset

        /// <summary>
        /// Offset of scrolled content.
        /// </summary>
        private Point offset;

        /// <summary>
        /// Gets the horizontal offset of the scrolled content.
        /// </summary>
        public double HorizontalOffset => offset.X;

        /// <summary>
        /// Gets the vertical offset of the scrolled content.
        /// </summary>
        public double VerticalOffset => offset.Y;

        #endregion

        #region ScrollOwner

        /// <summary>
        /// Gets or sets the <see cref="System.Windows.Controls.ScrollViewer"/> element.
        /// </summary>
        public ScrollViewer ScrollOwner { get; set; }

        #endregion

        #region CanHorizontallyScroll

        /// <summary>
        /// Gets or sets a value indicating whether or not the horizontal axis can be scrolled.
        /// </summary>
        public bool CanHorizontallyScroll { get; set; }

        #endregion

        #region CanVerticallyScroll

        /// <summary>
        /// Gets or sets a value indicating whether or not the vertical axis can be scrolled.
        /// </summary>
        public bool CanVerticallyScroll { get; set; }

        #endregion

        #region LineUp

        /// <summary>
        /// Scroll up the content one logical unit at a time.
        /// </summary>
        public void LineUp()
        {
            SetVerticalOffset(VerticalOffset - SystemParameters.ScrollHeight);
        }

        #endregion

        #region LineDown

        /// <summary>
        /// Scroll down the contents one logical unit at a time.
        /// </summary>
        public void LineDown()
        {
            SetVerticalOffset(VerticalOffset + SystemParameters.ScrollHeight);
        }

        #endregion

        #region LineLeft

        /// <summary>
        /// Scroll left within the content one logical unit at a time.
        /// </summary>
        public void LineLeft()
        {
            SetHorizontalOffset(HorizontalOffset - SystemParameters.ScrollWidth);
        }

        #endregion

        #region LineRight

        /// <summary>
        /// Scroll right within the content one logical unit at a time.
        /// </summary>
        public void LineRight()
        {
            SetHorizontalOffset(HorizontalOffset + SystemParameters.ScrollWidth);
        }

        #endregion

        #region PageUp

        /// <summary>
        /// Scroll up the contents one page at a time.
        /// </summary>
        public void PageUp()
        {
            SetVerticalOffset(VerticalOffset - viewport.Height);
        }

        #endregion

        #region PageDown

        /// <summary>
        /// Scroll down the contents one page at a time.
        /// </summary>
        public void PageDown()
        {
            SetVerticalOffset(VerticalOffset + viewport.Height);
        }

        #endregion

        #region PageLeft

        /// <summary>
        /// Scroll left within the content one page at a time.
        /// </summary>
        public void PageLeft()
        {
            SetHorizontalOffset(HorizontalOffset - viewport.Width);
        }

        #endregion

        #region PageRight

        /// <summary>
        /// Scroll the contents one page to the right.
        /// </summary>
        public void PageRight()
        {
            SetHorizontalOffset(HorizontalOffset + viewport.Width);
        }

        #endregion

        #region MouseWheelUp

        /// <summary>
        /// After the user clicks the mouse's wheel button, scroll up in the content.
        /// </summary>
        public void MouseWheelUp()
        {
            SetVerticalOffset(VerticalOffset - (SystemParameters.ScrollHeight * SystemParameters.WheelScrollLines));
        }

        #endregion

        #region MouseWheelDown

        /// <summary>
        /// After the user clicks the mouse's wheel button, it scrolls down the contents.
        /// </summary>
        public void MouseWheelDown()
        {
            SetVerticalOffset(VerticalOffset + (SystemParameters.ScrollHeight * SystemParameters.WheelScrollLines));
        }

        #endregion

        #region MouseWheelLeft

        /// <summary>
        /// Scroll left within the content after the user clicks the wheel button on the mouse.
        /// </summary>
        public void MouseWheelLeft()
        {
            SetHorizontalOffset(HorizontalOffset - (SystemParameters.ScrollWidth * SystemParameters.WheelScrollLines));
        }

        #endregion

        #region MouseWheelRight

        /// <summary>
        /// After the user clicks the mouse wheel button, scrolls right within the content.
        /// </summary>
        public void MouseWheelRight()
        {
            SetHorizontalOffset(HorizontalOffset + (SystemParameters.ScrollWidth * SystemParameters.WheelScrollLines));
        }

        #endregion

        #region MakeVisible

        /// <summary>
        /// <see cref="System.Windows.Media.Visual"/> Until the coordinate space of the object is displayed,
        /// forcibly scroll the content.
        /// </summary>
        /// <param name="visual">It becomes displayable <see cref="System.Windows.Media.Visual"/>.</param>
        /// <param name="rectangle">The circumscribing rectangle that identifies the coordinate space to display.</param>
        /// <returns>Is displayed <see cref="System.Windows.Rect"/>.</returns>
        public Rect MakeVisible(Visual visual, Rect rectangle)
        {
            var idx = InternalChildren.IndexOf(visual as UIElement);

            if (ItemContainerGenerator is IItemContainerGenerator generator)
            {
                var pos = new GeneratorPosition(idx, 0);
                idx = generator.IndexFromGeneratorPosition(pos);
            }

            if (idx < 0)
            {
                return Rect.Empty;
            }

            if (!containerLayouts.ContainsKey(idx))
            {
                return Rect.Empty;
            }

            var layout = containerLayouts[idx];

            if (HorizontalOffset + ViewportWidth < layout.X + layout.Width)
            {
                SetHorizontalOffset(layout.X + layout.Width - ViewportWidth);
            }
            if (layout.X < HorizontalOffset)
            {
                SetHorizontalOffset(layout.X);
            }

            if (VerticalOffset + ViewportHeight < layout.Y + layout.Height)
            {
                SetVerticalOffset(layout.Y + layout.Height - ViewportHeight);
            }
            if (layout.Y < VerticalOffset)
            {
                SetVerticalOffset(layout.Y);
            }

            layout.Width = Math.Min(ViewportWidth, layout.Width);
            layout.Height = Math.Min(ViewportHeight, layout.Height);

            return layout;
        }
        
        #endregion

        #region SetHorizontalOffset

        /// <summary>
        /// Set the horizontal offset value.
        /// </summary>
        /// <param name="offset">The degree of horizontal offset of the content from the containing viewport.</param>
        public void SetHorizontalOffset(double offset)
        {
            if (offset < 0 || ViewportWidth >= ExtentWidth)
            {
                offset = 0;
            }
            else
            {
                if (offset + ViewportWidth >= ExtentWidth)
                {
                    offset = ExtentWidth - ViewportWidth;
                }
            }

            this.offset.X = offset;

            if (ScrollOwner != null)
            {
                ScrollOwner.InvalidateScrollInfo();
            }

            InvalidateMeasure();
        }
        
        #endregion

        #region SetVerticalOffset

        /// <summary>
        /// Set the value of the vertical offset.
        /// </summary>
        /// <param name="offset">The degree of vertical offset from the containing viewport.</param>
        public void SetVerticalOffset(double offset)
        {
            if (offset < 0 || ViewportHeight >= ExtentHeight)
            {
                offset = 0;
            }
            else
            {
                if (offset + ViewportHeight >= ExtentHeight)
                {
                    offset = ExtentHeight - ViewportHeight;
                }
            }

            this.offset.Y = offset;

            if (ScrollOwner != null)
            {
                ScrollOwner.InvalidateScrollInfo();
            }

            InvalidateMeasure();
        }
        
        #endregion

        #endregion
    }
    #endregion
}

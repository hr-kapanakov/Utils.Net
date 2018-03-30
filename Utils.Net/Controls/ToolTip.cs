using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Utils.Net.Controls
{
    /// <summary>
    /// Extended <see cref="System.Windows.Controls.ToolTip"/>.
    /// </summary>
    /// <remarks>Allow showing <see cref="Label"/> and <see cref="Hotkey"/>.</remarks>
    [TemplatePart(Name = "PART_Label", Type = typeof(TextBlock))]
    [TemplatePart(Name = "PART_ContentHost", Type = typeof(FrameworkElement))]
    public class ToolTip : System.Windows.Controls.ToolTip
    {
        #region Dependency Properties

        /// <summary>
        /// Identifies the <see cref="Label"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(
                nameof(Label),
                typeof(string),
                typeof(ToolTip),
                new PropertyMetadata(string.Empty, OnFullLabelChanged));

        /// <summary>
        /// Gets or sets the label of the <see cref="ToolTip"/>.
        /// </summary>
        public string Label
        {
            get => (string)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }


        /// <summary>
        /// Identifies the <see cref="Hotkey"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HotkeyProperty =
            DependencyProperty.Register(
                nameof(Hotkey),
                typeof(string),
                typeof(ToolTip),
                new PropertyMetadata(string.Empty, OnFullLabelChanged));

        /// <summary>
        /// Gets or sets the assigned hotkey.
        /// </summary>
        public string Hotkey
        {
            get => (string)GetValue(HotkeyProperty);
            set => SetValue(HotkeyProperty, value);
        }


        /// <summary>
        /// Identifies the <see cref="IconSource"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IconSourceProperty =
            DependencyProperty.Register(
                nameof(IconSource),
                typeof(ImageSource),
                typeof(ToolTip));

        /// <summary>
        /// Gets or sets the <see cref="ImageSource"/> of the icon.
        /// </summary>
        public ImageSource IconSource
        {
            get => (ImageSource)GetValue(IconSourceProperty);
            set => SetValue(IconSourceProperty, value);
        }


        /// <summary>
        /// Identifies the <see cref="IconWidth"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IconWidthProperty =
            DependencyProperty.Register(
                nameof(IconWidth),
                typeof(double),
                typeof(ToolTip),
                new PropertyMetadata(32.0));

        /// <summary>
        /// Gets or sets the width of the icon.
        /// </summary>
        public Size IconWidth
        {
            get => (Size)GetValue(IconWidthProperty);
            set => SetValue(IconWidthProperty, value);
        }

        #endregion


        private TextBlock labelTextBlock;

        
        static ToolTip()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ToolTip), new FrameworkPropertyMetadata(typeof(ToolTip)));
        }


        /// <summary>
        /// Is called when a control template is applied.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            labelTextBlock = GetTemplateChild("PART_Label") as TextBlock;
            RefreshLabelText();
        }


        private void RefreshLabelText()
        {
            if (labelTextBlock != null)
            {
                labelTextBlock.Text = string.IsNullOrEmpty(Hotkey) ? $"{Label}" : $"{Label} ({Hotkey})";
            }
        }


        private static void OnFullLabelChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if (target is ToolTip toolTip)
            {
                toolTip.RefreshLabelText();
            }
        } 
    }
}

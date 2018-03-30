using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Utils.Net.Interactivity.Behaviors
{
    /// <summary>
    /// Attached Auto Complete behavior to the <see cref="TextBox"/>.
    /// </summary>
    public class AutoCompleteBehavior : Behavior<TextBox>
    {
        #region Dependency Properties

        /// <summary>
        /// Identifies the <see cref="ItemsSource"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(
                nameof(ItemsSource),
                typeof(IEnumerable<string>), 
                typeof(AutoCompleteBehavior));

        /// <summary>
        /// Gets or sets the collection to search for matches from.
        /// </summary>
        public IEnumerable<string> ItemsSource
        {
            get => (IEnumerable<string>)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }


        /// <summary>
        /// Identifies the <see cref="StringComparison"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StringComparisonProperty =
            DependencyProperty.Register(
                nameof(StringComparison), 
                typeof(StringComparison), 
                typeof(AutoCompleteBehavior), 
                new UIPropertyMetadata(StringComparison.Ordinal));

        /// <summary>
        /// Gets or sets Whether or not to ignore case when searching for matches.
        /// </summary>
        public StringComparison StringComparison
        {
            get => (StringComparison)GetValue(StringComparisonProperty);
            set => SetValue(StringComparisonProperty, value);
        }

        #endregion


        /// <summary>
        /// Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.TextChanged += AssociatedObject_TextChanged;
            AssociatedObject.PreviewKeyDown += AssociatedObject_PreviewKeyDown;
        }

        /// <summary>
        /// Called when the behavior is being detached from its AssociatedObject, but before it has actually occurred.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.TextChanged -= AssociatedObject_TextChanged;
            AssociatedObject.PreviewKeyDown -= AssociatedObject_PreviewKeyDown;
        }


        private void AssociatedObject_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.Changes.Any(c => c.RemovedLength > 0) &&
                e.Changes.All(c => c.AddedLength == 0))
            {
                return;
            }

            var tb = e.OriginalSource as TextBox;
            if (sender == null)
            {
                return;
            }

            // no reason to search if there's nothing there
            if (string.IsNullOrEmpty(tb.Text))
            {
                return;
            }

            var values = ItemsSource;
            if (values == null)
            {
                return;
            }

            var textLength = tb.Text.Length;
            var comparer = StringComparison;

            // do search and changes here
            var match = values
                .Where(v => v != null && v.Length >= textLength)
                .FirstOrDefault(v => v.Substring(0, textLength).Equals(tb.Text, comparer));

            if (string.IsNullOrEmpty(match))
            {
                return;
            }

            tb.TextChanged -= AssociatedObject_TextChanged;
            tb.Text = match;
            tb.CaretIndex = textLength;
            tb.SelectionStart = textLength;
            tb.SelectionLength = match.Length - textLength;
            tb.TextChanged += AssociatedObject_TextChanged;
        }

        private void AssociatedObject_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                return;
            }

            var tb = e.OriginalSource as TextBox;
            if (tb == null)
            {
                return;
            }

            // if we pressed enter and if the selected text goes all the way to the end, 
            // move our caret position to the end
            if (tb.SelectionLength > 0 && (tb.SelectionStart + tb.SelectionLength) == tb.Text.Length)
            {
                tb.SelectionStart = tb.CaretIndex = tb.Text.Length;
                tb.SelectionLength = 0;
            }
        }
    }
}

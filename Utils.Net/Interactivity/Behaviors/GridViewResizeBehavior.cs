using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Utils.Net.Interactivity.Behaviors
{
    /// <summary>
    /// Attached <see cref="GridViewColumn"/> resize behavior.
    /// </summary>
    public class GridViewResizeBehavior : Behavior<ListView>
    {
        /// <summary>
        /// Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.SizeChanged += AssociatedObject_SizeChanged;
        }

        /// <summary>
        /// Called when the behavior is being detached from its AssociatedObject, but before it has actually occurred.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.SizeChanged -= AssociatedObject_SizeChanged;
        }


        private void AssociatedObject_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.WidthChanged)
            {
                double totalWidth = AssociatedObject.ActualWidth;
                if (AssociatedObject.View is GridView gv)
                {
                    var gridViewColumns = gv.Columns.Where(
                        t => GetWidth(t) != null && GetWidth(t).ToLower() != "auto");

                    double allowedSpace = totalWidth - GetAllocatedSpace(gv) - 5;
                    allowedSpace = Math.Max(0, allowedSpace);
                    double totalPercentage = gridViewColumns.Sum(c => GetPercentage(c));
                    foreach (var column in gridViewColumns)
                    {
                        SetGridViewColumnWidth(column, allowedSpace, totalPercentage);
                    }
                }
            }
        }


        private static double GetAllocatedSpace(GridView gv)
        {
            double totalWidth = 0;
            var scrollvis = ScrollViewer.GetVerticalScrollBarVisibility(gv);
            if (scrollvis == ScrollBarVisibility.Auto || scrollvis == ScrollBarVisibility.Visible)
            {
                totalWidth += SystemParameters.VerticalScrollBarWidth;
            }

            foreach (var column in gv.Columns)
            {
                if (GetWidth(column) != null)
                {
                    if (IsStaticWidth(column))
                    {
                        totalWidth += GetStaticWidth(column);
                    }
                    if (GetWidth(column).ToLower() == "auto")
                    {
                        totalWidth += column.ActualWidth;
                    }
                }
                else
                {
                    totalWidth += column.ActualWidth;
                }
            }
            return Math.Max(0, totalWidth);
        }

        private static void SetGridViewColumnWidth(
            GridViewColumn column, double allowedSpace, double totalPercentage)
        {
            if (IsStaticWidth(column))
            {
                column.Width = GetStaticWidth(column);
            }
            else if (GetWidth(column).ToLower() == "auto")
            {
                column.Width = column.ActualWidth; // force update column width
                column.Width = double.NaN;
            }
            else
            {
                double width = allowedSpace * (GetPercentage(column) / totalPercentage);
                column.Width = width;
            }

            var minWidth = GetMinWidth(column);
            if (!double.IsNaN(column.ActualWidth) && column.ActualWidth < minWidth)
            {
                column.Width = minWidth;
            }

            var maxWidth = GetMaxWidth(column);
            if (!double.IsNaN(column.ActualWidth) && 
                column.ActualWidth > maxWidth && maxWidth > 0)
            {
                column.Width = maxWidth;
            }
        }



        #region Attached Dependency Properties

        /// <summary>
        /// Identifies the attached Width dependency property.
        /// </summary>
        public static readonly DependencyProperty WidthProperty =
            DependencyProperty.RegisterAttached("Width", typeof(string), typeof(GridViewResizeBehavior));

        /// <summary>
        /// Get value of the <see cref="WidthProperty"/> dependency property.
        /// </summary>
        /// <param name="obj">Dependency object from which the value will be get.</param>
        /// <returns>Value of the <see cref="WidthProperty"/> dependency property.</returns>
        public static string GetWidth(DependencyObject obj)
        {
            return (string)obj.GetValue(WidthProperty);
        }

        /// <summary>
        /// Set value to the <see cref="WidthProperty"/> dependency property.
        /// </summary>
        /// <param name="obj">Dependency object to which the value will be set.</param>
        /// <param name="value">Value which will be set to the <see cref="WidthProperty"/> dependency property.</param>
        public static void SetWidth(DependencyObject obj, string value)
        {
            obj.SetValue(WidthProperty, value);
        }


        /// <summary>
        /// Identifies the attached MinWidth dependency property.
        /// </summary>
        public static readonly DependencyProperty MinWidthProperty =
            DependencyProperty.RegisterAttached("MinWidth", typeof(double), typeof(GridViewResizeBehavior));

        /// <summary>
        /// Get value of the <see cref="MinWidthProperty"/> dependency property.
        /// </summary>
        /// <param name="obj">Dependency object from which the value will be get.</param>
        /// <returns>Value of the <see cref="MinWidthProperty"/> dependency property.</returns>
        public static double GetMinWidth(DependencyObject obj)
        {
            return (double)obj.GetValue(MinWidthProperty);
        }

        /// <summary>
        /// Set value to the <see cref="MinWidthProperty"/> dependency property.
        /// </summary>
        /// <param name="obj">Dependency object to which the value will be set.</param>
        /// <param name="value">Value which will be set to the <see cref="MinWidthProperty"/> dependency property.</param>
        public static void SetMinWidth(DependencyObject obj, double value)
        {
            obj.SetValue(MinWidthProperty, value);
        }


        /// <summary>
        /// Identifies the attached MaxWidth dependency property.
        /// </summary>
        public static readonly DependencyProperty MaxWidthProperty =
            DependencyProperty.RegisterAttached("MaxWidth", typeof(double), typeof(GridViewResizeBehavior));

        /// <summary>
        /// Get value of the <see cref="MaxWidthProperty"/> dependency property.
        /// </summary>
        /// <param name="obj">Dependency object from which the value will be get.</param>
        /// <returns>Value of the <see cref="MaxWidthProperty"/> dependency property.</returns>
        public static double GetMaxWidth(DependencyObject obj)
        {
            return (double)obj.GetValue(MaxWidthProperty);
        }

        /// <summary>
        /// Set value to the <see cref="MaxWidthProperty"/> dependency property.
        /// </summary>
        /// <param name="obj">Dependency object to which the value will be set.</param>
        /// <param name="value">Value which will be set to the <see cref="MaxWidthProperty"/> dependency property.</param>
        public static void SetMaxWidth(DependencyObject obj, double value)
        {
            obj.SetValue(MaxWidthProperty, value);
        }

        #endregion


        /// <summary>
        /// Determine if <see cref="WidthProperty"/> is a static.
        /// </summary>
        /// <param name="obj">Dependency object from which the value will be get.</param>
        /// <returns>Returns true if <see cref="WidthProperty"/> is a static; otherwise false.</returns>
        public static bool IsStaticWidth(DependencyObject obj)
        {
            return GetStaticWidth(obj) >= 0;
        }

        /// <summary>
        /// Get the static width of the <see cref="WidthProperty"/>.
        /// </summary>
        /// <param name="obj">Dependency object from which the value will be get.</param>
        /// <returns>Static width of the <see cref="WidthProperty"/>.</returns>
        public static double GetStaticWidth(DependencyObject obj)
        {
            return double.TryParse(GetWidth(obj), out double result) ? result : -1;
        }

        /// <summary>
        /// Get the percentage of the <see cref="WidthProperty"/>.
        /// </summary>
        /// <param name="obj">Dependency object from which the value will be get.</param>
        /// <returns>Percentage of the <see cref="WidthProperty"/>.</returns>
        public static double GetPercentage(DependencyObject obj)
        {
            if (!IsStaticWidth(obj))
            {
                return GetMultiplier(obj) * 100;
            }
            return 0;
        }

        /// <summary>
        /// Get the multiplier of the <see cref="WidthProperty"/>.
        /// </summary>
        /// <param name="obj">Dependency object from which the value will be get.</param>
        /// <returns>Multiplier of the <see cref="WidthProperty"/>.</returns>
        public static double GetMultiplier(DependencyObject obj)
        {
            var width = GetWidth(obj);
            if (width == "*" || width == "1*")
            {
                return 1;
            }

            if (width.EndsWith("*"))
            {
                if (double.TryParse(width.Substring(0, width.Length - 1), out double perc))
                {
                    return perc;
                }
            }
            return 1;
        }
    }
}

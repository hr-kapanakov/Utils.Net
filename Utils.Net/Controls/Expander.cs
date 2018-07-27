using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Utils.Net.Controls
{
    /// <summary>
    /// Extended <see cref="System.Windows.Controls.Expander"/>.
    /// </summary>
    /// <remarks>
    /// Allow change of the <see cref="ControlTemplate"/> of the <see cref="System.Windows.Controls.Primitives.ToggleButton"/>.
    /// </remarks>
    [TemplatePart(Name = "HeaderSite", Type = typeof(System.Windows.Controls.Primitives.ToggleButton))]
    public class Expander : System.Windows.Controls.Expander
    {
        #region Dependency Properties

        /// <summary>
        /// Identifies the <see cref="CornerRadius"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ToggleButtonTemplateProperty =
            DependencyProperty.Register(
                nameof(ToggleButtonTemplate),
                typeof(ControlTemplate),
                typeof(Expander),
                new PropertyMetadata(OnToggleButtonTemplateChanged));

        /// <summary>
        /// Gets or sets the corner radius of the <see cref="Border"/>.
        /// </summary>
        public ControlTemplate ToggleButtonTemplate
        {
            get => (ControlTemplate)GetValue(ToggleButtonTemplateProperty);
            set => SetValue(ToggleButtonTemplateProperty, value);
        }

        #endregion

        private System.Windows.Controls.Primitives.ToggleButton toggleButton;
        private ControlTemplate defaultToggleButtonTemplate;


        /// <summary>
        /// Is called when a control template is applied.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            toggleButton = GetTemplateChild("HeaderSite") as System.Windows.Controls.Primitives.ToggleButton;
            RefreshToggleButtonTemplate();
        }

        private void RefreshToggleButtonTemplate()
        {
            if (toggleButton == null)
            {
                return;
            }

            if (ToggleButtonTemplate != null)
            {
                if (defaultToggleButtonTemplate == null)
                {
                    defaultToggleButtonTemplate = toggleButton.Template;
                }

                toggleButton.Template = ToggleButtonTemplate;
            }
            else
            {
                toggleButton.Template = defaultToggleButtonTemplate;
            }
        }


        private static void OnToggleButtonTemplateChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var expander = (Expander)target;
            expander.RefreshToggleButtonTemplate();
        }
    }
}

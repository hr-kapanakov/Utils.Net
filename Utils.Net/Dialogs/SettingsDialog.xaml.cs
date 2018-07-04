using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Utils.Net.Attributes;
using Utils.Net.Common;

namespace Utils.Net.Dialogs
{
    /// <summary>
    /// Interaction logic for SettingsDialog.xaml
    /// </summary>
    public partial class SettingsDialog : UserControl
    {
        /// <summary>
        /// Gets the object containing the settings.
        /// </summary>
        public object Settings => DataContext;

        /// <summary>
        /// Gets the dictionary of all <see cref="ICommand"/> that can be used.
        /// </summary>
        public Dictionary<string, ICommand> Commands { get; }

        /// <summary>
        /// Represents the method that will return custom control for editing property.
        /// </summary>
        /// <param name="property"><see cref="PropertyInfo"/> for which should be returned control.</param>
        /// <param name="dependencyProperty"><see cref="DependencyProperty"/> to which the setting should be binded.</param>
        /// <returns>Control which will be used for editing property.</returns>
        public delegate Control GetEditorDelegate(PropertyInfo property, out DependencyProperty dependencyProperty);

        private GetEditorDelegate getEditorHandler;
        /// <summary>
        /// Gets or sets the <see cref="GetEditorDelegate"/> handler.
        /// </summary>
        public GetEditorDelegate GetEditorHandler
        {
            get => getEditorHandler;
            set
            {
                if (getEditorHandler != value)
                {
                    getEditorHandler = value;
                    InitializeView();
                }
            }
        }

        
        private bool expandableGroups = false;
        /// <summary>
        /// Gets or sets a value indicating whether the property groups will be expandable.
        /// </summary>
        public bool ExpandableGroups
        {
            get => expandableGroups;
            set
            {
                if (expandableGroups != value)
                {
                    expandableGroups = value;
                    InitializeView();
                }
            }
        }

        private bool fixedSize = true;
        /// <summary>
        /// Gets or sets a value indicating whether the dialog will be with fixed size.
        /// </summary>
        public bool FixedSize
        {
            get => fixedSize;
            set
            {
                if (fixedSize != value)
                {
                    fixedSize = value;
                    InitializeView();
                }
            }
        }

        private UpdateSourceTrigger updateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
        /// <summary>
        /// Gets or sets a value that determines the timing of binding source updates.
        /// </summary>
        public UpdateSourceTrigger UpdateSourceTrigger
        {
            get => updateSourceTrigger;
            set
            {
                if (updateSourceTrigger != value)
                {
                    updateSourceTrigger = value;
                    InitializeView();
                }
            }
        }

        private int updateDelay = 100;
        /// <summary>
        /// Gets or sets the amount of time, in milliseconds, to wait before updating the <para/>
        /// binding source after the value on the target changes.
        /// </summary>
        public int UpdateDelay
        {
            get => updateDelay;
            set
            {
                if (updateDelay != value)
                {
                    updateDelay = value;
                    InitializeView();
                }
            }
        }


        private Window ParentWindow => Parent as Window;


        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsDialog"/> class.
        /// </summary>
        /// <param name="settings">The object containing the settings.</param>
        /// <param name="commands">The dictionary of all <see cref="ICommand"/> that can be used; default null.</param>
        /// <param name="getEditorHandler">The <see cref="GetEditorDelegate"/> handler; default null.</param>
        public SettingsDialog(
            object settings,
            Dictionary<string, ICommand> commands = null, 
            GetEditorDelegate getEditorHandler = null)
        {
            InitializeComponent();

            DataContext = settings;
            this.getEditorHandler = getEditorHandler;
            Commands = commands ?? new Dictionary<string, ICommand>();
            InitializeView();
        }


        private void InitializeView()
        {
            var properties = Settings.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(pi => pi.GetCustomAttribute<SettingAttribute>() != null);

            var categories = properties.GroupBy(pi => pi.GetCustomAttribute<SettingAttribute>().Category);
            if (categories.Count() > 1) // show tabs only if there are more than one category
            {
                var tabControl = new TabControl();
                foreach (var category in categories)
                {
                    var tabItem = new TabItem
                    {
                        Header = category.Key,
                        Content = GenerateContent(category)
                    };
                    tabControl.Items.Add(tabItem);
                }
                content.Content = tabControl;

                if (fixedSize)
                {
                    tabControl.Loaded += (_, __) => FixTabSizes(tabControl);
                }
            }
            else
            {
                content.Content = GenerateContent(properties);
            }
        }

        private void FixTabSizes(TabControl tabControl)
        {
            var maxSize = new Size(0, 0);
            foreach (var tabItem in tabControl.ItemContainerGenerator.Items.OfType<TabItem>())
            {
                var content = tabItem.Content as FrameworkElement;
                if (maxSize.Width < content.DesiredSize.Width)
                {
                    maxSize = new Size(content.DesiredSize.Width, maxSize.Height);
                }
                if (maxSize.Height < content.DesiredSize.Height)
                {
                    maxSize = new Size(maxSize.Width, content.DesiredSize.Height);
                }
            }
            foreach (var tabItem in tabControl.ItemContainerGenerator.Items.OfType<TabItem>())
            {
                var content = tabItem.Content as FrameworkElement;
                content.Width = maxSize.Width;
                content.Height = maxSize.Height;
            }
        }

        private object GenerateContent(IEnumerable<PropertyInfo> properties)
        {
            var stackPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(10, 10, 10, 0)
            };

            var groups = properties.GroupBy(p => p.GetCustomAttribute<SettingAttribute>().Group);
            foreach (var group in groups)
            {
                var innerStackPanel = stackPanel;

                // add group name
                if (!string.IsNullOrEmpty(group.Key))
                {
                    stackPanel.Children.Add(GetGroupControl(group.Key, ref innerStackPanel));
                }

                // foreach property sorted by SortName or property Name
                foreach (var property in group.OrderBy(p => p.GetCustomAttribute<SettingAttribute>().SortName ?? p.Name))
                {
                    var panel = new WrapPanel
                    {
                        Orientation = Orientation.Horizontal,
                        Margin = new Thickness(0, 0, 0, 5),
                        HorizontalAlignment = HorizontalAlignment.Right
                    };

                    var textBlock = new TextBlock
                    {
                        Text = property.GetCustomAttribute<SettingAttribute>().DisplayName ?? property.Name,
                        Margin = new Thickness(0, 0, 5, 0),
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    panel.Children.Add(textBlock);

                    panel.Children.Add(GetEditor(property));
                    
                    innerStackPanel.Children.Add(panel);
                }
            }

            return stackPanel;
        }

        private FrameworkElement GetGroupControl(string groupName, ref StackPanel innerStackPanel)
        {
            var textBlock = new TextBlock
            {
                Text = groupName,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(5),
                VerticalAlignment = VerticalAlignment.Center
            };

            if (!expandableGroups)
            {
                return textBlock;
            }
            else // if ExpandableGroups is enabled add every property into Expander
            {
                innerStackPanel = new StackPanel();
                var expander = new Expander
                {
                    Header = textBlock,
                    IsExpanded = true,
                    Content = innerStackPanel
                };
                return expander;
            }
        }

        private FrameworkElement GetEditor(PropertyInfo property)
        {
            Control control = null;
            DependencyProperty dependencyProperty = null;

            if (GetEditorHandler != null) // get a user defined editor
            {
                control = GetEditorHandler(property, out dependencyProperty);
            }
            if (control == null || dependencyProperty == null) // if there isn't, create default one
            {
                control = GetDefaultEditor(property, out dependencyProperty);
            }
            control.VerticalContentAlignment = VerticalAlignment.Center;
            control.Width = 150;

            // set appropriate binding
            var binding = new Binding(property.Name)
            {
                Mode = property.CanWrite ? BindingMode.TwoWay : BindingMode.OneWay,
                UpdateSourceTrigger = updateSourceTrigger,
                Delay = updateDelay
            };
            control.SetBinding(dependencyProperty, binding);

            // if there is an attached command - add button next to the editor
            var commandName = property.GetCustomAttribute<SettingAttribute>().Command;
            if (commandName != null && Commands.ContainsKey(commandName))
            {
                control.Width = double.NaN;

                var button = new Button
                {
                    Margin = new Thickness(5, 0, 0, 0),
                    Content = commandName,
                    Command = new RelayCommand(
                        _ => 
                    {
                        Commands[commandName].Execute(Settings);

                        // refresh the whole controls content because the command may modify other than current property
                        var settings = Settings;
                        DataContext = null;
                        DataContext = settings;
                    }, 
                    _ => Commands[commandName].CanExecute(Settings))
                };

                // add control and button to the DockPanel, add control as last so it'll fill the panel
                var dockPanel = new DockPanel { Width = 150 };
                DockPanel.SetDock(button, Dock.Right);
                dockPanel.Children.Add(button);
                DockPanel.SetDock(control, Dock.Left);
                dockPanel.Children.Add(control);
                return dockPanel;
            }
            return control;
        }

        private Control GetDefaultEditor(PropertyInfo property, out DependencyProperty dependencyProperty)
        {
            Control control = null;
            if (property.PropertyType.IsEnum)
            {
                control = new ComboBox
                {
                    ItemsSource = Enum.GetValues(property.PropertyType),
                    IsEnabled = property.CanWrite
                };
                dependencyProperty = ComboBox.TextProperty;
            }
            else if (property.GetCustomAttribute<NumericAttribute>() is NumericAttribute numberAttribute)
            {
                control = new Controls.NumericBox()
                {
                    StringFormat = numberAttribute.StringFormat,
                    Minimum = numberAttribute.Minimum,
                    Maximum = numberAttribute.Maximum,
                    Step = numberAttribute.Step,
                    IsEnabled = property.CanWrite
                };
                dependencyProperty = Controls.NumericBox.ValueProperty;
            }
            else
            {
                control = new Controls.TextBox
                {
                    Padding = new Thickness(0, 2, 0, 2),
                    IsReadOnly = !property.CanWrite
                };
                dependencyProperty = TextBox.TextProperty;
            }
            return control;
        }


        /// <summary>
        /// Invoked when an unhandled <see cref="Keyboard.KeyDownEvent"/> attached event 
        /// reaches an element in its route that is derived from this class.
        /// </summary>
        /// <param name="e">The <see cref="KeyEventArgs"/> that contains the event data.</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Key == Key.Escape)
            {
                CloseDialog(true);
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            CloseDialog(true);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            CloseDialog(false);
        }

        private void CloseDialog(bool result)
        {
            if (ParentWindow != null)
            {
                try
                {
                    ParentWindow.DialogResult = result;
                }
                catch
                {
                    ParentWindow.Close();
                }
            }
        }


        /// <summary>
        /// Show <see cref="SettingsDialog"/> window.
        /// </summary>
        /// <param name="caption">The caption of the showed dialog.</param>
        /// <param name="settings">The object containing the settings.</param>
        /// <param name="commands">The dictionary of all <see cref="ICommand"/> that can be used; default null.</param>
        /// <param name="getEditorHandler">The <see cref="GetEditorDelegate"/> handler; default null.</param>
        /// <returns>Whether the activity was accepted (true) or canceled (false).</returns>
        public static bool Show(
            string caption, 
            object settings, 
            Dictionary<string, ICommand> commands = null, 
            GetEditorDelegate getEditorHandler = null)
        {
            return Show(null, caption, settings, commands, getEditorHandler);
        }

        /// <summary>
        /// Show <see cref="SettingsDialog"/> window.
        /// </summary>
        /// <param name="owner">The window which owns this dialog.</param>
        /// <param name="caption">The caption of the showed dialog.</param>
        /// <param name="settings">The object containing the settings.</param>
        /// <param name="commands">The dictionary of all <see cref="ICommand"/> that can be used; default null.</param>
        /// <param name="getEditorHandler">The <see cref="GetEditorDelegate"/> handler; default null.</param>
        /// <returns>Whether the activity was accepted (true) or canceled (false).</returns>
        public static bool Show(
            Window owner, 
            string caption, 
            object settings,
            Dictionary<string, ICommand> commands = null, 
            GetEditorDelegate getEditorHandler = null)
        {
            var settingsControl = new SettingsDialog(settings, commands, getEditorHandler);

            var window = new Window()
            {
                Title = caption,
                Content = settingsControl,
                ResizeMode = ResizeMode.NoResize,
                SizeToContent = SizeToContent.WidthAndHeight,
                MaxWidth = 400,
                Owner = owner ?? Application.Current?.MainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                WindowStyle = WindowStyle.SingleBorderWindow,
                ShowInTaskbar = false,
            };

            var res = window.ShowDialog();
            return res.HasValue && res.Value;
        }
    }
}

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Autofac;
using Utils.Net.Common;
using Utils.Net.Interfaces;
using Utils.Net.Managers.Tutorial;
using Utils.Net.ViewModels;

namespace Utils.Net.Sample
{
    public class MainWindowViewModel : ViewModelBase
    {
        public INavigationManager NavigationManager => App.Container.Resolve<INavigationManager>();

        public ITutorialManager TutorialManager => App.Container.Resolve<ITutorialManager>();


        public ObservableCollection<string> Controls { get; }

        public string SelectedControl
        {
            get => NavigationManager.CurrentControl?.GetType().Name;
            set
            {
                if (SelectedControl != value)
                {
                    var fullTypeName = GetType().Namespace + ".Views." + value;
                    var ctrl = (Control)Assembly.GetExecutingAssembly().GetType(fullTypeName)?.GetConstructor(Type.EmptyTypes)?.Invoke(null);
                    NavigationManager.NavigateTo(ctrl);
                }
            }
        }


        public RelayCommand ForwardCommand { get; }

        public RelayCommand BackwardCommand { get; }

        public RelayCommand StartTutorialCommand { get; }


        public MainWindowViewModel()
        {
            Controls = new ObservableCollection<string>();
            PopulateControls();
            PopulateTutorial();

            ForwardCommand = new RelayCommand(_ => NavigationManager.NavigateForward(), _ => NavigationManager.CanNavigateForward);
            BackwardCommand = new RelayCommand(_ => NavigationManager.NavigateBackward(), _ => NavigationManager.CanNavigateBackward);
            StartTutorialCommand = new RelayCommand(_ => TutorialManager.Start(), _ => !TutorialManager.IsStarted);

            NavigationManager.CurrentControlChanged += (_, __) => OnPropertyChanged(nameof(SelectedControl));
            SelectedControl = Controls.FirstOrDefault();
        }


        private void PopulateControls()
        {
            var pageTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.Name.EndsWith("Page"));
            foreach (var type in pageTypes)
            {
                Controls.Add(type.Name);
            }
        }

        private void PopulateTutorial()
        {
            TutorialManager.Items.Add(
                "Begin",
                new TutorialItem(
                    "Welcome To Utils.Net Sample!", 
                    "*Utils.Net* is a _*.NET framework*_ utilities library.\n" +
                    "The sample application allows you to discover different functionalities which the library provides.\n" +
                    "This *Tutorial* will go throught the main points of the application."));
            TutorialManager.Items.Add(
                "Navigation",
                new TutorialItem(
                    "Navigation", 
                    "You can use the *Navigation* panel to navigate between the different content.\n" +
                    "This functionality is provided by the _*NavigationManager*_ class."));

            // Explorer Page
            TutorialManager.Items.Add(
                "ExplorerPage",
                new TutorialItem(
                    "Explorer Page", 
                    "This is the *Explorer* like page. It contains a tree and thumbnails view of items associated with the tree.",
                    beforeShow: _ => SelectedControl = "ExplorerPage"));
            TutorialManager.Items.Add(
                "TreeView",
                new TutorialItem(
                    "Tree View", 
                    "The *Tree View* allows to bind to the selected item and to deselect by " +
                    "clicking on the blank space because of the _*TreeViewExtensionBehavior*_.\n" +
                    "There are also used _*TreeItemViewModel*_ and _*CheckableItemViewModel*_ which allows to bind to _IsExpanded_ and _IsChecked_ properties.",
                    PlacementMode.Center, 
                    _ => SelectedControl = "ExplorerPage"));
            TutorialManager.Items.Add(
                "ListView",
                new TutorialItem(
                    "List View", 
                    "The *List View* allows to bind to the collection of the selected items and to deselect by " +
                    "clicking on the blank space because of the _*ListViewExtensionBehavior*_.\n" +
                    "It uses _*VirtualizingWrapPanel*_ which optimize the _WrapPanel_ to work with a lot of items.",
                    PlacementMode.Center, 
                    _ => SelectedControl = "ExplorerPage"));

            // List Page
            TutorialManager.Items.Add(
                "ListPage",
                new TutorialItem(
                    "List Page", 
                    "This is the *List* page which shows the typical list view with different filters functionalities.",
                    beforeShow: _ => SelectedControl = "ListPage"));
            TutorialManager.Items.Add(
                "TextBox",
                new TutorialItem(
                    "Text Box", 
                    "This *Text Box* allows adding of hint text, icon and clear button. " +
                    "The auto-complete functionality is introduced by _*AutoCompleteBehavior*_.\n" +
                    "Also there is assigned curstom *Tool Tip* which allows showing label, shortcut, icon and description.",
                    beforeShow: _ => SelectedControl = "ListPage"));
            TutorialManager.Items.Add(
                "ComboBox",
                new TutorialItem(
                    "Combo Box", 
                    "The *Combo Box* gives the same functionality like the text box and also _filtering_.",
                    beforeShow: _ => SelectedControl = "ListPage"));
            TutorialManager.Items.Add(
                "GridView",
                new TutorialItem(
                    "Grid View", 
                    "This *Grid View* is extended by _*GridViewResizeBehavior*_ and _*GridViewSortBehavior*_ " +
                    "which allows to auto resize grid columns with '_auto_', '_**_' and ratio ('_5**_'), and to sort items by clicking on the headers.",
                    PlacementMode.Center, 
                    _ => SelectedControl = "ListPage"));

            // Others Page
            TutorialManager.Items.Add(
                "OthersPage",
                new TutorialItem(
                    "Others Page", 
                    "The *Others* page contains all other small controls and features",
                    beforeShow: _ => SelectedControl = "OthersPage"));
            TutorialManager.Items.Add(
                "Dialogs",
                new TutorialItem(
                    "Dialogs", 
                    "... like different *Dialog* types.",
                    beforeShow: _ => SelectedControl = "OthersPage"));
            TutorialManager.Items.Add(
                "NumericBox",
                new TutorialItem(
                    "Numeric Box", 
                    "The *Numeric Box* allows to input integer or float number with validation. " +
                    "Also to change it with set step and restriction with the mouse wheel and keyboard.",
                    beforeShow: _ => SelectedControl = "OthersPage"));
            TutorialManager.Items.Add(
                "LinkButton",
                new TutorialItem(
                    "Button Styles", 
                    "There are also different *Button* and *Toggle Button* styles like _*LinkButtonStyle*_",
                    beforeShow: _ => SelectedControl = "OthersPage"));
            TutorialManager.Items.Add(
                "ImageButton",
                new TutorialItem(
                    "Button Styles", 
                    "... and _*ImageButtonStyle*_.",
                    beforeShow: _ => SelectedControl = "OthersPage"));
            TutorialManager.Items.Add(
                "ValidatedTextBox",
                new TutorialItem(
                    "Validation", 
                    "The delegate binding *Validator* allows make different validation much easier.",
                    beforeShow: _ => SelectedControl = "OthersPage"));
            TutorialManager.Items.Add(
                "Expander",
                new TutorialItem(
                    "Expander", 
                    "The custom *Expander* allow to set style to its toggle button like the _*PlusExpanderToggleButtonTemplate*_.",
                    beforeShow: _ => SelectedControl = "OthersPage"));

            TutorialManager.Items.Add(
                "End",
                new TutorialItem(
                    "Thank you!", 
                    "Thank you for being part of the Utils.Net *Tutorial*!\nIt was provided by the _*TutorialManager*_"));
        }
    }
}

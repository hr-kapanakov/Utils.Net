using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using Autofac;
using Utils.Net.Common;
using Utils.Net.Interfaces;
using Utils.Net.ViewModels;

namespace Utils.Net.Sample
{
    public class MainWindowViewModel : ViewModelBase
    {
        public INavigationManager NavigationManager => App.Container.Resolve<INavigationManager>();

        public ObservableCollection<string> Controls { get; }

        public string SelectedControl
        {
            get => NavigationManager.CurrentControl?.GetType().Name;
            set
            {
                var fullTypeName = GetType().Namespace + ".Views." + value;
                var ctrl = (Control)Assembly.GetExecutingAssembly().GetType(fullTypeName)?.GetConstructor(Type.EmptyTypes)?.Invoke(null);
                NavigationManager.NavigateTo(ctrl);
            }
        }


        public RelayCommand ForwardCommand { get; }

        public RelayCommand BackwardCommand { get; }


        public MainWindowViewModel()
        {
            Controls = new ObservableCollection<string>();
            var pageTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.Name.EndsWith("Page"));
            foreach (var type in pageTypes)
            {
                Controls.Add(type.Name);
            }

            ForwardCommand = new RelayCommand(_ => NavigationManager.NavigateForward(), _ => NavigationManager.CanNavigateForward);
            BackwardCommand = new RelayCommand(_ => NavigationManager.NavigateBackward(), _ => NavigationManager.CanNavigateBackward);

            NavigationManager.CurrentControlChanged += (_, __) => OnPropertyChanged(nameof(SelectedControl));
            SelectedControl = Controls.FirstOrDefault();
        }
    }
}

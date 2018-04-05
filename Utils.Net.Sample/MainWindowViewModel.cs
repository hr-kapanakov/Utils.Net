using System.Windows;
using Utils.Net.Managers;
using Utils.Net.Sample.Views;
using Utils.Net.ViewModels;

namespace Utils.Net.Sample
{
    public class MainWindowViewModel : ViewModelBase
    {
        private NavigationManager navigationManager;

        private FrameworkElement contentControl;
        public FrameworkElement ContentControl
        {
            get => contentControl;
            set => SetPropertyBackingField(ref contentControl, value);
        }


        public MainWindowViewModel()
        {
            navigationManager = new NavigationManager();
            navigationManager.CurrentControlChanged += (s, e) => ContentControl = e.Value;
            navigationManager.NavigateTo(new ListPage());
        }
    }
}

using System;
using System.Threading.Tasks;
using System.Windows;
using Utils.Net.Common;
using Utils.Net.Dialogs;
using Utils.Net.ViewModels;

namespace Utils.Net.Sample.ViewModels
{
    public class OthersPageViewModel : ViewModelBase
    {
        public double Number { get; set; }

        public RelayCommand SimpleInputDialogCommand { get; }

        public RelayCommand ProgressDialogCommand { get; }


        public OthersPageViewModel()
        {
            SimpleInputDialogCommand = new RelayCommand(_ => ShowSimpleInputDialog());
            ProgressDialogCommand = new RelayCommand(_ => ShowProgressDialog());
        }


        private void ShowSimpleInputDialog()
        {
            var text = SimpleInputDialog.GetText("Simple Input Dialog", "Please input simple text", "text");
            if (!string.IsNullOrEmpty(text))
            {
                MessageBox.Show(text);
            }
        }

        private async void ShowProgressDialog()
        {
            using (var dialog = ProgressDialog.Show("Progress", 100, "Progress"))
            {
                var rand = new Random();
                for (int i = 0; i <= 100; i++)
                {
                    var status = await Task.Delay(rand.Next(50, 200))
                        .ContinueWith(_ => dialog.UpdateProgress(i, i.ToString()));

                    if (!status)
                    {
                        MessageBox.Show("Cancelled");
                        break;
                    }
                }
            }
        }
    }
}

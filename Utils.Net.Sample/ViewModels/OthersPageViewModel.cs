using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using Utils.Net.Common;
using Utils.Net.Dialogs;
using Utils.Net.ViewModels;

namespace Utils.Net.Sample.ViewModels
{
    public class OthersPageViewModel : ViewModelBase
    {
        public RelayCommand SimpleInputDialogCommand { get; }

        public RelayCommand ProgressDialogCommand { get; }

        private double number;
        public double Number
        {
            get => number;
            set => SetPropertyBackingField(ref number, value);
        }

        private string validatedText;
        public string ValidatedText
        {
            get => validatedText;
            set => SetPropertyBackingField(ref validatedText, value);
        }

        public ValidationRule.ValidationDelegate Validator { get; }


        public OthersPageViewModel()
        {
            SimpleInputDialogCommand = new RelayCommand(_ => ShowSimpleInputDialog());
            ProgressDialogCommand = new RelayCommand(_ => ShowProgressDialog());

            Validator = TextValidation;
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

        private bool TextValidation(object value, out string message)
        {
            if (!(value is string text))
            {
                message = "Value type is invalid";
                return false;
            }

            var result = Regex.IsMatch(text, @"^[a-zA-Z]+$");
            if (!result)
            {
                message = "Value doesn't contains only letters";
                return false;
            }

            message = string.Empty;
            return true;
        }
    }
}

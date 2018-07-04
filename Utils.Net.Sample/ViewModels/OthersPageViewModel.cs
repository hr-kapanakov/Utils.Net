using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Utils.Net.Common;
using Utils.Net.Dialogs;
using Utils.Net.Sample.Models;
using Utils.Net.ViewModels;

namespace Utils.Net.Sample.ViewModels
{
    public class OthersPageViewModel : ViewModelBase
    {
        public RelayCommand SimpleInputDialogCommand { get; }

        public RelayCommand ProgressDialogCommand { get; }

        public RelayCommand SettingsDialogCommand { get; }

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
            SettingsDialogCommand = new RelayCommand(_ => ShowSettingsDialog());

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

        private void ShowSettingsDialog()
        {
            var settings = new Settings();

            var commands = new Dictionary<string, ICommand>
            {
                {
                    "Browse",
                    new RelayCommand<Settings>(s =>
                    {
                        var openFileDialog = new Microsoft.Win32.OpenFileDialog();
                        if (openFileDialog.ShowDialog() == true)
                        {
                            s.Name = System.IO.Path.GetFileName(openFileDialog.FileName);
                        }
                    })
                }
            };

            SettingsDialog.Show("Settings", settings, commands, GetEditorHandler);
        }
        private System.Windows.Controls.Control GetEditorHandler(PropertyInfo property, out DependencyProperty dependencyProperty)
        {
            if (property.PropertyType == typeof(DateTime))
            {
                dependencyProperty = System.Windows.Controls.TextBox.TextProperty;
                return new System.Windows.Controls.TextBox
                {
                    Padding = new Thickness(0, 2, 0, 2),
                    Background = System.Windows.Media.Brushes.Red,
                    IsReadOnly = !property.CanWrite
                };
            }

            dependencyProperty = null;
            return null;
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

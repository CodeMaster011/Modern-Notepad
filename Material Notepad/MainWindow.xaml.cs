using Material_Notepad.ViewModels;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Material_Notepad
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string title = "Material Notepad";
        bool isDark = true;
        string currentFilePath = null;
        string currentFileName = "Note.txt";
        bool forceExit = false;

        public MainWindow()
        {
            InitializeComponent();
            UpdateTitle();
            CreateFontSize();

            (Resources["vm"] as FindUserControlViewModel).DocumentTextBox = textbox;
            (Resources["vm"] as FindUserControlViewModel).ParentView = findReplaceView;
        }

        public void CreateFontSize()
        {
            // fontComboBox.ItemsSource
            var fontSizes = new List<int>();
            var f = 8;
            for (int i = 0; i < 20; i++)
            {
                if((f + i) % 2 == 0)
                    fontSizes.Add(f + i);
            }
            fontComboBox.ItemsSource = fontSizes;
            fontComboBox.SelectedItem = fontSizes.Find(d => d == 14);
        }

        private void changeThemeDark_Click(object sender, RoutedEventArgs e)
        {
            ApplyBase(isDark = !isDark);
        }

        private readonly PaletteHelper _paletteHelper = new PaletteHelper();
        private void ApplyBase(bool isDark)
        {
            ITheme theme = _paletteHelper.GetTheme();
            IBaseTheme baseTheme = isDark ? new MaterialDesignDarkTheme() : (IBaseTheme)new MaterialDesignLightTheme();
            theme.SetBaseTheme(baseTheme);
            _paletteHelper.SetTheme(theme);
        }

        private async void textbox_Drop(object sender, DragEventArgs e)
        {
            var filePaths = e.Data.GetData("FileDrop") as string[];
            var file = filePaths.FirstOrDefault();
            var text = await System.IO.File.ReadAllTextAsync(file);
            textbox.Text += text;
            //var formates = e.Data.GetFormats();
            //foreach (var formate in formates)
            //{
            //    try
            //    {
            //        object data = e.Data.GetData(formate);
            //        textbox.Text += $"{formate}: " + data + "\n";
            //        if(data.GetType() == typeof(string[]))
            //        {
            //            foreach (var line in (data as string[]))
            //            {
            //                textbox.Text += line + "\n";
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        textbox.Text += $"{formate}: " + ex.Message + "\n";
            //    }
            //}

        }

        private void textbox_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Handled = true;
        }

        private void newWindowButton_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
        }

        private async void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(currentFilePath))
            {
                saveAsButton_Click(null, null);
            }
            else
            {
                await System.IO.File.WriteAllTextAsync(currentFilePath, textbox.Text);
                UpdateTitle();
                MainSnackbar.MessageQueue?.Enqueue("File saved successfully");
            }
        }

        private async void saveAsButton_Click(object sender, RoutedEventArgs e)
        {
            var dig = new SaveFileDialog();
            dig.Filter = "All Files|*.*";
            if (dig.ShowDialog() != true) return;
            

            var filePath = dig.FileName;
            currentFileName = System.IO.Path.GetFileName(filePath);

            await System.IO.File.WriteAllTextAsync(filePath, textbox.Text);

            UpdateTitle();
            MainSnackbar.MessageQueue?.Enqueue("File saved successfully");
        }

        public void UpdateTitle()
        {
            Title = $"{title} - {currentFileName}".Trim();
        }

        private void wordWrapButton_Click(object sender, RoutedEventArgs e)
        {
            textbox.TextWrapping = wordWrapButton.IsChecked.GetValueOrDefault(false) ? TextWrapping.Wrap : TextWrapping.NoWrap;
        }

        private void fontComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            textbox.FontSize = (int)fontComboBox.SelectedItem;
        }

        private async void newButton_Click(object sender, RoutedEventArgs e)
        {
            var view = new Dialogs.ConfirmationDialog
            {
                DataContext = new ViewModels.ConfirmationDialogViewModel
                {
                    Title = "Confirm",
                    Message = "You want to remove the data?",
                }
            };

            //show the dialog
            var result = await DialogHost.Show(view, "RootDialog");

            // MessageBox.Show(((bool)result).ToString());
        }

        protected override async void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (!forceExit && !string.IsNullOrEmpty(textbox.Text))
            {
                var view = new Dialogs.ConfirmationDialog
                {
                    DataContext = new ViewModels.ConfirmationDialogViewModel
                    {
                        Title = "Confirm",
                        Message = "Do you really want to exit, there is text which will be discarded?",
                    }
                };

                e.Cancel = true;

                var result = await DialogHost.Show(view, "RootDialog");
                if ((bool)result)
                {
                    forceExit = true;
                    Close();
                }
            }
        }

        private void findButton_Click(object sender, RoutedEventArgs e)
        {
            // Resources["vm"] as 
            if (findReplaceView.Visibility == Visibility.Collapsed)
                findReplaceView.Visibility = Visibility.Visible;
            else if (findReplaceView.Visibility == Visibility.Visible)
                findReplaceView.Visibility = Visibility.Collapsed;
        }
    }
}

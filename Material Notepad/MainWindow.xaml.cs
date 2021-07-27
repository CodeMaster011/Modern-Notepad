using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
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
        bool isDark = true;
        public MainWindow()
        {
            InitializeComponent();
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

        private void textbox_PreviewDrop(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;

            var formates = e.Data.GetFormats();
            Console.WriteLine("Hello");
        }

        private void textbox_DragEnter(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;
        }

        private void textbox_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Handled = true;
        }
    }
}

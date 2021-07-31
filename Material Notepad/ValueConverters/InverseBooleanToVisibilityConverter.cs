using MaterialDesignThemes.Wpf.Converters;
using System.Windows;

namespace Material_Notepad.ValueConverters
{
    public class InverseBooleanToVisibilityConverter : BooleanConverter<Visibility>
    {
        public InverseBooleanToVisibilityConverter():
            base(Visibility.Collapsed, Visibility.Visible)
        {

        }
    }
}

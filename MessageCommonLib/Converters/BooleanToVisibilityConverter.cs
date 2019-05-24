using System;
using System.Globalization;
using System.Windows;

namespace MessageCommonLib.Converters
{
    public class BooleanToVisibilityConverter : BaseValueConverter<BooleanToVisibilityConverter>
    {
        public BooleanToVisibilityConverter()
        {
            True = Visibility.Visible;
            False = Visibility.Collapsed;
        }

        public Visibility True { get; set; }
        public Visibility False { get; set; }

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool && ((bool)value) ? True : False;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

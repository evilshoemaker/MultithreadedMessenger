using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MessageCommonLib.Converters
{
    public class EventTypeToColorConverter : BaseValueConverter<EventTypeToColorConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != DependencyProperty.UnsetValue)
            {
                switch (value)
                {
                    case "Error":
                        return (SolidColorBrush)(new BrushConverter().ConvertFrom("#ffd4d4"));
                    case "Warning":
                        return (SolidColorBrush)(new BrushConverter().ConvertFrom("#ffffcc"));
                    default:
                        return (SolidColorBrush)(new BrushConverter().ConvertFrom("#ffffff"));
                }
            }
            else
            {
                return new SolidColorBrush();
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

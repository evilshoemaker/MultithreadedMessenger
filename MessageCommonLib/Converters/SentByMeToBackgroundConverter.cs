using System;
using System.Globalization;
using System.Windows.Media;

namespace MessageCommonLib.Converters
{
    public class SentByMeToBackgroundConverter : BaseValueConverter<SentByMeToBackgroundConverter>
    {
        public Brush Color1 { set; private get; }

        public Brush Color2 { set; private get; }

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Color2
                : Color1;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

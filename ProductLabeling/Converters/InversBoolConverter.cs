using System;
using System.Globalization;
using System.Windows.Data;

namespace ProductLabeling.Converters
{
    [ValueConversion(typeof(bool), typeof(bool))]
    internal class InversBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => !(bool)value;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => !(bool)value;
    }
}

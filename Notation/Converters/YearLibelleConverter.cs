using System;
using System.Globalization;
using System.Windows.Data;

namespace Notation.Converters
{
    public class YearLibelleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int year && year != 0)
            {
                return $"{year}/{year + 1}";
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Globalization;
using System.Windows.Data;

namespace Notation.Converters
{
    public class YearLibelleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int)
            {
                int year = (int)value;
                return year != 0 ? string.Format("{0}/{1}", year, year + 1) : "";
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

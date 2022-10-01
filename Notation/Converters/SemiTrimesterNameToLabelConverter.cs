using System;
using System.Globalization;
using System.Windows.Data;

namespace Notation.Converters
{
    public class SemiTrimesterNameToLabelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string name = (string)value;
            return $"Demi-trimestre {(!string.IsNullOrEmpty(name) && !(name.ToUpper().StartsWith('A') || name.ToUpper().StartsWith('O')) ? "de " : "d'")}{name}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

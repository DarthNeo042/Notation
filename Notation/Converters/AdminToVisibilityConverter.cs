using Notation.ViewModels;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Notation.Converters
{
    public class AdminToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((UserViewModel)value)?.IsAdmin ?? false ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

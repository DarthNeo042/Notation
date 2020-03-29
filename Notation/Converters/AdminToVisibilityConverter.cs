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
            UserViewModel user = (UserViewModel)value;
            return user != null && user.IsAdmin ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

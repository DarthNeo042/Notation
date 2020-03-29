using Notation.ViewModels;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Notation.Converters
{
    public class SubjectToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SubjectViewModel subject)
            {
                return (subject.ParentSubject != null ? " | " : "") + subject.Name;
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

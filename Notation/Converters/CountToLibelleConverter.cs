using System;
using System.Globalization;
using System.Windows.Data;

namespace Notation.Converters
{
    public class CountToLibelleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool plural = ((int)value) > 1;
            string label = "";
            switch ((string)parameter)
            {
                case "Level":
                    label = "niveau" + (plural ? "x" : "");
                    break;
                case "Subject":
                    label = "matière" + (plural ? "s" : "");
                    break;
                case "Teacher":
                    label = "professeur" + (plural ? "s" : "");
                    break;
                case "Class":
                    label = "classe" + (plural ? "s" : "");
                    break;
                case "Student":
                    label = "élève" + (plural ? "s" : "");
                    break;
                case "Period":
                    label = "période" + (plural ? "s" : "");
                    break;
                case "SemiTrimester":
                    label = "demi-trimestre" + (plural ? "s" : "");
                    break;
            }
            return $"{(int)value} {label}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

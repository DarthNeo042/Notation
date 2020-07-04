using System.Windows;

namespace Notation.ViewModels
{
    public class YearParametersViewModel : BaseViewModel
    {
        public string DivisionPrefect
        {
            get { return (string)GetValue(DivisionPrefectProperty); }
            set { SetValue(DivisionPrefectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DivisionPrefect.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DivisionPrefectProperty =
            DependencyProperty.Register("DivisionPrefect", typeof(string), typeof(YearParametersViewModel), new PropertyMetadata(""));
    }
}

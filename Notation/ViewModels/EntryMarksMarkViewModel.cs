using System.Windows;

namespace Notation.ViewModels
{
    public class EntryMarksMarkViewModel : DependencyObject
    {
        public string Mark
        {
            get { return (string)GetValue(MarkProperty); }
            set { SetValue(MarkProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Mark.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MarkProperty =
            DependencyProperty.Register("Mark", typeof(string), typeof(EntryMarksMarkViewModel), new PropertyMetadata(""));
    }
}

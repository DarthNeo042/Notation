using System.Windows;

namespace Notation.ViewModels
{
    public class EntryTrimesterSubjectCommentsSubjectViewModel : DependencyObject
    {
        public SubjectViewModel Subject
        {
            get { return (SubjectViewModel)GetValue(SubjectProperty); }
            set { SetValue(SubjectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Subject.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SubjectProperty =
            DependencyProperty.Register("Subject", typeof(SubjectViewModel), typeof(EntryTrimesterSubjectCommentsSubjectViewModel), new PropertyMetadata(null));

        public string Average
        {
            get { return (string)GetValue(AverageProperty); }
            set { SetValue(AverageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Average.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AverageProperty =
            DependencyProperty.Register("Average", typeof(string), typeof(EntryTrimesterSubjectCommentsSubjectViewModel), new PropertyMetadata(""));
    }
}

using System.Windows;

namespace Notation.ViewModels
{
    public class TrimesterCommentViewModel : BaseViewModel
    {
        public string MainTeacherComment
        {
            get { return (string)GetValue(MainTeacherCommentProperty); }
            set { SetValue(MainTeacherCommentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MainTeacherComment.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MainTeacherCommentProperty =
            DependencyProperty.Register("MainTeacherComment", typeof(string), typeof(TrimesterCommentViewModel), new PropertyMetadata(""));

        public string DivisionPrefectComment
        {
            get { return (string)GetValue(DivisionPrefectCommentProperty); }
            set { SetValue(DivisionPrefectCommentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DivisionPrefectComment.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DivisionPrefectCommentProperty =
            DependencyProperty.Register("DivisionPrefectComment", typeof(string), typeof(TrimesterCommentViewModel), new PropertyMetadata(""));

        public StudentViewModel Student
        {
            get { return (StudentViewModel)GetValue(StudentProperty); }
            set { SetValue(StudentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Student.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StudentProperty =
            DependencyProperty.Register("Student", typeof(StudentViewModel), typeof(TrimesterCommentViewModel), new PropertyMetadata(null));

        public int Trimester
        {
            get { return (int)GetValue(TrimesterProperty); }
            set { SetValue(TrimesterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Trimester.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TrimesterProperty =
            DependencyProperty.Register("Trimester", typeof(int), typeof(TrimesterCommentViewModel), new PropertyMetadata(1));
    }
}

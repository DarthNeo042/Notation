using System.Windows;

namespace Notation.ViewModels
{
    public class SemiTrimesterCommentViewModel : BaseViewModel
    {
        public string MainTeacherComment
        {
            get { return (string)GetValue(MainTeacherCommentProperty); }
            set { SetValue(MainTeacherCommentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MainTeacherComment.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MainTeacherCommentProperty =
            DependencyProperty.Register("MainTeacherComment", typeof(string), typeof(SemiTrimesterCommentViewModel), new PropertyMetadata(""));


        public string DivisionPrefectComment
        {
            get { return (string)GetValue(DivisionPrefectCommentProperty); }
            set { SetValue(DivisionPrefectCommentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DivisionPrefectComment.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DivisionPrefectCommentProperty =
            DependencyProperty.Register("DivisionPrefectComment", typeof(string), typeof(SemiTrimesterCommentViewModel), new PropertyMetadata(""));

        public int IdStudent
        {
            get { return (int)GetValue(IdStudentProperty); }
            set { SetValue(IdStudentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IdStudent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IdStudentProperty =
            DependencyProperty.Register("IdStudent", typeof(int), typeof(SemiTrimesterCommentViewModel), new PropertyMetadata(0));

        public int IdSemiTrimester
        {
            get { return (int)GetValue(IdSemiTrimesterProperty); }
            set { SetValue(IdSemiTrimesterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IdSemiTrimester.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IdSemiTrimesterProperty =
            DependencyProperty.Register("IdSemiTrimester", typeof(int), typeof(SemiTrimesterCommentViewModel), new PropertyMetadata(0));
    }
}

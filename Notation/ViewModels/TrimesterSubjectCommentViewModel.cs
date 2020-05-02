using System.Windows;

namespace Notation.ViewModels
{
    public class TrimesterSubjectCommentViewModel : BaseViewModel
    {
        public string Comment
        {
            get { return (string)GetValue(CommentProperty); }
            set { SetValue(CommentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Comment.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommentProperty =
            DependencyProperty.Register("Comment", typeof(string), typeof(TrimesterSubjectCommentViewModel), new PropertyMetadata(""));

        public StudentViewModel Student
        {
            get { return (StudentViewModel)GetValue(StudentProperty); }
            set { SetValue(StudentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Student.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StudentProperty =
            DependencyProperty.Register("Student", typeof(StudentViewModel), typeof(TrimesterSubjectCommentViewModel), new PropertyMetadata(null));

        public SubjectViewModel Subject
        {
            get { return (SubjectViewModel)GetValue(SubjectProperty); }
            set { SetValue(SubjectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Subject.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SubjectProperty =
            DependencyProperty.Register("Subject", typeof(SubjectViewModel), typeof(TrimesterSubjectCommentViewModel), new PropertyMetadata(null));

        public int Trimester
        {
            get { return (int)GetValue(TrimesterProperty); }
            set { SetValue(TrimesterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Trimester.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TrimesterProperty =
            DependencyProperty.Register("Trimester", typeof(int), typeof(TrimesterSubjectCommentViewModel), new PropertyMetadata(1));
    }
}

using System.Windows;

namespace Notation.ViewModels
{
    public class MarkViewModel : BaseViewModel
    {
        public int Mark
        {
            get { return (int)GetValue(MarkProperty); }
            set { SetValue(MarkProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Mark.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MarkProperty =
            DependencyProperty.Register("Mark", typeof(int), typeof(MarkViewModel), new PropertyMetadata(0));

        public int Coefficient
        {
            get { return (int)GetValue(CoefficientProperty); }
            set { SetValue(CoefficientProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Coefficient.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CoefficientProperty =
            DependencyProperty.Register("Coefficient", typeof(int), typeof(MarkViewModel), new PropertyMetadata(1));

        public int IdClass
        {
            get { return (int)GetValue(IdClassProperty); }
            set { SetValue(IdClassProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IdClass.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IdClassProperty =
            DependencyProperty.Register("IdClass", typeof(int), typeof(MarkViewModel), new PropertyMetadata(0));

        public int IdPeriod
        {
            get { return (int)GetValue(IdPeriodProperty); }
            set { SetValue(IdPeriodProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IdPeriod.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IdPeriodProperty =
            DependencyProperty.Register("IdPeriod", typeof(int), typeof(MarkViewModel), new PropertyMetadata(0));

        public int IdSubject
        {
            get { return (int)GetValue(IdSubjectProperty); }
            set { SetValue(IdSubjectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IdSubject.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IdSubjectProperty =
            DependencyProperty.Register("IdSubject", typeof(int), typeof(MarkViewModel), new PropertyMetadata(0));

        public int IdStudent
        {
            get { return (int)GetValue(IdStudentProperty); }
            set { SetValue(IdStudentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IdStudent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IdStudentProperty =
            DependencyProperty.Register("IdStudent", typeof(int), typeof(MarkViewModel), new PropertyMetadata(0));

        public int IdTeacher
        {
            get { return (int)GetValue(IdTeacherProperty); }
            set { SetValue(IdTeacherProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IdTeacher.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IdTeacherProperty =
            DependencyProperty.Register("IdTeacher", typeof(int), typeof(MarkViewModel), new PropertyMetadata(0));
    }
}

using System.Windows;

namespace Notation.ViewModels
{
    public class UserViewModel : BaseViewModel
    {
        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Name.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register("Name", typeof(string), typeof(UserViewModel), new PropertyMetadata(""));

        public bool IsAdmin
        {
            get { return (bool)GetValue(IsAdminProperty); }
            set { SetValue(IsAdminProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsAdmin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsAdminProperty =
            DependencyProperty.Register("IsAdmin", typeof(bool), typeof(UserViewModel), new PropertyMetadata(false));

        public TeacherViewModel Teacher
        {
            get { return (TeacherViewModel)GetValue(TeacherProperty); }
            set { SetValue(TeacherProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Teacher.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TeacherProperty =
            DependencyProperty.Register("Teacher", typeof(TeacherViewModel), typeof(TeacherViewModel), new PropertyMetadata(null));
    }
}

using Notation.Utils;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace Notation.ViewModels
{
    public class TeacherViewModel : BaseViewModel
    {
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(TeacherViewModel), new PropertyMetadata("M."));

        public string LastName
        {
            get { return (string)GetValue(LastNameProperty); }
            set { SetValue(LastNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LastName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LastNameProperty =
            DependencyProperty.Register("LastName", typeof(string), typeof(TeacherViewModel), new PropertyMetadata("", LastNamePropertyChanged));

        private static void LastNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TeacherViewModel teacher = (TeacherViewModel)d;
            teacher.LastName = teacher.LastName.ToUpper();
        }

        public string FirstName
        {
            get { return (string)GetValue(FirstNameProperty); }
            set { SetValue(FirstNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FirstName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FirstNameProperty =
            DependencyProperty.Register("FirstName", typeof(string), typeof(TeacherViewModel), new PropertyMetadata("", FirstNamePropertyChanged));

        private static void FirstNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TeacherViewModel teacher = (TeacherViewModel)d;
            teacher.FirstName = NameUtils.FormatPascalFirstName(teacher.FirstName);
            GenerateLoginPassword(teacher);
        }

        private static void GenerateLoginPassword(TeacherViewModel teacher)
        {
            if (!string.IsNullOrWhiteSpace(teacher.LastName) && !string.IsNullOrWhiteSpace(teacher.FirstName))
            {
                teacher.Login = string.Format("{0}{1}", teacher.FirstName.ToLower()[0], teacher.LastName.Replace(" ", "").Replace("-", "").ToLower());
                teacher.Password = string.Format("{0}{1}", teacher.LastName.Replace(" ", "").Replace("-", "").ToLower().Substring(0, 3), new Random().Next(100, 999));
            }
        }

        public string Login
        {
            get { return (string)GetValue(LoginProperty); }
            set { SetValue(LoginProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Login.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LoginProperty =
            DependencyProperty.Register("Login", typeof(string), typeof(TeacherViewModel), new PropertyMetadata(""));

        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Password.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(TeacherViewModel), new PropertyMetadata(""));

        public SubjectViewModel SelectedSubject
        {
            get { return (SubjectViewModel)GetValue(SelectedSubjectProperty); }
            set { SetValue(SelectedSubjectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedSubject.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedSubjectProperty =
            DependencyProperty.Register("SelectedSubject", typeof(SubjectViewModel), typeof(TeacherViewModel), new PropertyMetadata(null));

        public ObservableCollection<SubjectViewModel> Subjects { get; set; }

        public ClassViewModel SelectedClass
        {
            get { return (ClassViewModel)GetValue(SelectedClassProperty); }
            set { SetValue(SelectedClassProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedClass.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedClassProperty =
            DependencyProperty.Register("SelectedClass", typeof(ClassViewModel), typeof(TeacherViewModel), new PropertyMetadata(null));

        public ObservableCollection<ClassViewModel> Classes { get; set; }

        public TeacherViewModel()
        {
            Subjects = new ObservableCollection<SubjectViewModel>();
            Classes = new ObservableCollection<ClassViewModel>();
        }
    }
}

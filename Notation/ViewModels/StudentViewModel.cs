using Notation.Utils;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace Notation.ViewModels
{
    public class StudentViewModel : BaseViewModel
    {
        public string LastName
        {
            get { return (string)GetValue(LastNameProperty); }
            set { SetValue(LastNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LastName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LastNameProperty =
            DependencyProperty.Register("LastName", typeof(string), typeof(StudentViewModel), new PropertyMetadata("", LastNamePropertyChanged));

        private static void LastNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            StudentViewModel student = (StudentViewModel)d;
            student.LastName = student.LastName.ToUpper();
        }

        public string FirstName
        {
            get { return (string)GetValue(FirstNameProperty); }
            set { SetValue(FirstNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FirstName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FirstNameProperty =
            DependencyProperty.Register("FirstName", typeof(string), typeof(StudentViewModel), new PropertyMetadata("", FirstNamePropertyChanged));

        private static void FirstNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            StudentViewModel student = (StudentViewModel)d;
            student.FirstName = NameUtils.FormatPascal(student.FirstName);
        }
        public DateTime BirthDate
        {
            get { return (DateTime)GetValue(BirthDateProperty); }
            set { SetValue(BirthDateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BirthDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BirthDateProperty =
            DependencyProperty.Register("BirthDate", typeof(DateTime), typeof(StudentViewModel), new PropertyMetadata(DateTime.Now));

        public ClassViewModel Class
        {
            get { return (ClassViewModel)GetValue(ClassProperty); }
            set { SetValue(ClassProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Class.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ClassProperty =
            DependencyProperty.Register("Class", typeof(ClassViewModel), typeof(StudentViewModel), new PropertyMetadata(null));

        public ObservableCollection<ClassViewModel> Classes { get; set; }

        public StudentViewModel()
        {
            Classes = new ObservableCollection<ClassViewModel>();
        }

        public void LoadClasses()
        {
            Classes.Clear();
            foreach (ClassViewModel _class in MainViewModel.Instance.Parameters.Classes)
            {
                Classes.Add(_class);
            }
            Classes.Insert(0, new ClassViewModel());
        }
    }
}

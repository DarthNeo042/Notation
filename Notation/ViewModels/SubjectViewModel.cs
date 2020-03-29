using Notation.Utils;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Notation.ViewModels
{
    public class SubjectViewModel : BaseViewModel
    {
        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Name.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register("Name", typeof(string), typeof(SubjectViewModel), new PropertyMetadata("", NamePropertyChanged));

        private static void NamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SubjectViewModel subject = (SubjectViewModel)d;
            subject.Name = NameUtils.FormatPascal(subject.Name);
        }

        public double Coefficient
        {
            get { return (double)GetValue(CoefficientProperty); }
            set { SetValue(CoefficientProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Coefficient.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CoefficientProperty =
            DependencyProperty.Register("Coefficient", typeof(double), typeof(SubjectViewModel), new PropertyMetadata(1d));

        public bool Option
        {
            get { return (bool)GetValue(OptionProperty); }
            set { SetValue(OptionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Option.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OptionProperty =
            DependencyProperty.Register("Option", typeof(bool), typeof(SubjectViewModel), new PropertyMetadata(false));

        public SubjectViewModel ParentSubject
        {
            get { return (SubjectViewModel)GetValue(ParentSubjectProperty); }
            set { SetValue(ParentSubjectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ParentSubject.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ParentSubjectProperty =
            DependencyProperty.Register("ParentSubject", typeof(SubjectViewModel), typeof(SubjectViewModel), new PropertyMetadata(null));

        public ObservableCollection<SubjectViewModel> ParentsSubjects { get; set; }
        public ObservableCollection<SubjectViewModel> ChildrenSubjects { get; set; }

        public LevelViewModel SelectedLevel
        {
            get { return (LevelViewModel)GetValue(SelectedLevelProperty); }
            set { SetValue(SelectedLevelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedLevel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedLevelProperty =
            DependencyProperty.Register("SelectedLevel", typeof(LevelViewModel), typeof(SubjectViewModel), new PropertyMetadata(null));

        public ObservableCollection<LevelViewModel> Levels { get; set; }

        public TeacherViewModel SelectedTeacher
        {
            get { return (TeacherViewModel)GetValue(SelectedTeacherProperty); }
            set { SetValue(SelectedTeacherProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedTeacher.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedTeacherProperty =
            DependencyProperty.Register("SelectedTeacher", typeof(TeacherViewModel), typeof(SubjectViewModel), new PropertyMetadata(null));

        public ObservableCollection<TeacherViewModel> Teachers { get; set; }

        public SubjectViewModel()
        {
            ParentsSubjects = new ObservableCollection<SubjectViewModel>();
            ChildrenSubjects = new ObservableCollection<SubjectViewModel>();
            Levels = new ObservableCollection<LevelViewModel>();

            Teachers = new ObservableCollection<TeacherViewModel>();
        }

        public void LoadParentSubjects()
        {
            ParentsSubjects.Clear();
            if (!MainViewModel.Instance.Parameters.Subjects.Any(s => s.ParentSubject != null && s.ParentSubject.Id == Id))
            {
                foreach (SubjectViewModel subject in MainViewModel.Instance.Parameters.Subjects.Where(s => s.Id != Id && s.ParentSubject == null))
                {
                    ParentsSubjects.Add(subject);
                }
                ParentsSubjects.Insert(0, new SubjectViewModel());
            }
        }
    }
}

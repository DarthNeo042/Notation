using System.Collections.ObjectModel;
using System.Windows;

namespace Notation.ViewModels
{
    public class ClassViewModel : BaseViewModel
    {
        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Name.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register("Name", typeof(string), typeof(ClassViewModel), new PropertyMetadata(""));

        public TeacherViewModel MainTeacher
        {
            get { return (TeacherViewModel)GetValue(MainTeacherProperty); }
            set { SetValue(MainTeacherProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MainTeacher.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MainTeacherProperty =
            DependencyProperty.Register("MainTeacher", typeof(TeacherViewModel), typeof(ClassViewModel), new PropertyMetadata(null));

        public LevelViewModel Level
        {
            get { return (LevelViewModel)GetValue(LevelProperty); }
            set { SetValue(LevelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Level.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LevelProperty =
            DependencyProperty.Register("Level", typeof(LevelViewModel), typeof(ClassViewModel), new PropertyMetadata(null));

        public ObservableCollection<TeacherViewModel> MainTeachers { get; set; }
        public ObservableCollection<LevelViewModel> Levels { get; set; }

        public TeacherViewModel SelectedTeacher
        {
            get { return (TeacherViewModel)GetValue(SelectedTeacherProperty); }
            set { SetValue(SelectedTeacherProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedTeacher.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedTeacherProperty =
            DependencyProperty.Register("SelectedTeacherModel", typeof(TeacherViewModel), typeof(ClassViewModel), new PropertyMetadata(null));

        public ObservableCollection<TeacherViewModel> Teachers { get; set; }

        public StudentViewModel SelectedStudent
        {
            get { return (StudentViewModel)GetValue(SelectedStudentProperty); }
            set { SetValue(SelectedStudentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedStudent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedStudentProperty =
            DependencyProperty.Register("SelectedStudentModel", typeof(StudentViewModel), typeof(ClassViewModel), new PropertyMetadata(null));

        public ObservableCollection<StudentViewModel> Students { get; set; }

        public ClassViewModel()
        {
            MainTeachers = new ObservableCollection<TeacherViewModel>();
            Teachers = new ObservableCollection<TeacherViewModel>();
            Levels = new ObservableCollection<LevelViewModel>();
            Students = new ObservableCollection<StudentViewModel>();
        }

        public void LoadMainTeachersLevels()
        {
            MainTeachers.Clear();
            foreach (TeacherViewModel teacher in MainViewModel.Instance.Parameters.Teachers)
            {
                MainTeachers.Add(teacher);
            }
            MainTeachers.Insert(0, new TeacherViewModel() { Title = ""} );
            Levels.Clear();
            foreach (LevelViewModel level in MainViewModel.Instance.Parameters.Levels)
            {
                Levels.Add(level);
            }
            Levels.Insert(0, new LevelViewModel());
        }
    }
}

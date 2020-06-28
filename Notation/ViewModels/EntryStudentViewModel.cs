using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Notation.ViewModels
{
    public class EntryStudentViewModel : DependencyObject
    {
        public delegate void SelectedSubjectChangedEventHandler();

        public event SelectedSubjectChangedEventHandler SelectedSubjectChangedEvent;

        public bool SelectedSubjectChangedSet
        {
            get
            {
                return SelectedSubjectChangedEvent != null;
            }
        }

        public StudentViewModel Student
        {
            get { return (StudentViewModel)GetValue(StudentProperty); }
            set { SetValue(StudentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Student.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StudentProperty =
            DependencyProperty.Register("Student", typeof(StudentViewModel), typeof(EntryStudentViewModel), new PropertyMetadata(null));

        public EntryMarksSubjectViewModel SelectedMarksSubject
        {
            get { return (EntryMarksSubjectViewModel)GetValue(SelectedMarksSubjectProperty); }
            set { SetValue(SelectedMarksSubjectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedEntryMarkSubject.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedMarksSubjectProperty =
            DependencyProperty.Register("SelectedMarksSubject", typeof(EntryMarksSubjectViewModel), typeof(EntryStudentViewModel), new PropertyMetadata(null, SelectedMarksSubjectChanged));

        private static void SelectedMarksSubjectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EntryStudentViewModel entryStudent = (EntryStudentViewModel)d;
            entryStudent.SelectedSubjectChangedEvent?.Invoke();
        }

        public ObservableCollection<EntryMarksSubjectViewModel> MarksSubjects { get; set; }

        public EntryStudentViewModel()
        {
            MarksSubjects = new ObservableCollection<EntryMarksSubjectViewModel>();
        }

        public void LoadEntryMarks(TeacherViewModel teacher)
        {
            if (Student.Class != null && Student.Class.Level != null)
            {
                foreach (SubjectViewModel subject in Student.Class.Level.Subjects.Where(s => s.Teachers.Contains(teacher)))
                {
                    EntryMarksSubjectViewModel entryMarksSubject = new EntryMarksSubjectViewModel() { Subject = subject };
                    entryMarksSubject.Coefficients.Add(new EntryMarksCoefficientViewModel() { Coefficient = 1, Name = "Leçons" });
                    entryMarksSubject.Coefficients.Add(new EntryMarksCoefficientViewModel() { Coefficient = 2, Name = "Devoirs" });
                    entryMarksSubject.Coefficients.Add(new EntryMarksCoefficientViewModel() { Coefficient = 4, Name = "Examens" });
                    MarksSubjects.Add(entryMarksSubject);
                }
            }
        }
    }
}

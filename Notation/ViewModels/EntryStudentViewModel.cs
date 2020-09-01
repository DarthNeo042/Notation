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

        public EntryTrimesterSubjectCommentsSubjectViewModel SelectedTrimesterSubjectCommentsSubject
        {
            get { return (EntryTrimesterSubjectCommentsSubjectViewModel)GetValue(SelectedTrimesterSubjectCommentsSubjectProperty); }
            set { SetValue(SelectedTrimesterSubjectCommentsSubjectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedEntryTrimesterSubjectCommentSubject.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedTrimesterSubjectCommentsSubjectProperty =
            DependencyProperty.Register("SelectedTrimesterSubjectCommentsSubject", typeof(EntryTrimesterSubjectCommentsSubjectViewModel), typeof(EntryStudentViewModel), new PropertyMetadata(null, SelectedTrimesterSubjectCommentsSubjectChanged));

        private static void SelectedTrimesterSubjectCommentsSubjectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EntryStudentViewModel entryStudent = (EntryStudentViewModel)d;
            entryStudent.SelectedSubjectChangedEvent?.Invoke();
        }

        public ObservableCollection<EntryTrimesterSubjectCommentsSubjectViewModel> TrimesterSubjectCommentsSubjects { get; set; }

        public EntryStudentViewModel()
        {
            MarksSubjects = new ObservableCollection<EntryMarksSubjectViewModel>();
            TrimesterSubjectCommentsSubjects = new ObservableCollection<EntryTrimesterSubjectCommentsSubjectViewModel>();
        }

        public void LoadEntryMarks(TeacherViewModel teacher)
        {
            if (Student.Class != null && Student.Class.Level != null)
            {
                foreach (SubjectViewModel subject in Student.Class.Level.Subjects.Where(s => s.Teachers.Contains(teacher)).OrderBy(s => s.Order))
                {
                    if (subject.ChildrenSubjects.Any())
                    {
                        foreach (SubjectViewModel subject2 in subject.ChildrenSubjects.OrderBy(s => s.Order))
                        {
                            MarksSubjects.Add(new EntryMarksSubjectViewModel() { Subject = subject2 });
                        }
                    }
                    else
                    {
                        MarksSubjects.Add(new EntryMarksSubjectViewModel() { Subject = subject });
                    }
                }
            }
        }

        public void LoadEntryTrimesterSubjectComments(TeacherViewModel teacher)
        {
            if (Student.Class != null && Student.Class.Level != null)
            {
                foreach (SubjectViewModel subject in Student.Class.Level.Subjects.Where(s => s.Teachers.Contains(teacher)).OrderBy(s => s.Order))
                {
                    if (subject.ChildrenSubjects.Any())
                    {
                        foreach (SubjectViewModel subject2 in subject.ChildrenSubjects.OrderBy(s => s.Order))
                        {
                            TrimesterSubjectCommentsSubjects.Add(new EntryTrimesterSubjectCommentsSubjectViewModel() { Subject = subject2 });
                        }
                    }
                    else
                    {
                        TrimesterSubjectCommentsSubjects.Add(new EntryTrimesterSubjectCommentsSubjectViewModel() { Subject = subject });
                    }
                }
            }
        }
    }
}

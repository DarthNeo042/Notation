using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Notation.ViewModels
{
    public class EntryMarksViewModel : DependencyObject
    {
        public delegate void SelectedClassChangedEventHandler();

        public event SelectedClassChangedEventHandler SelectedClassChangedEvent;

        public ObservableCollection<EntryClassViewModel> Classes { get; set; }

        public EntryClassViewModel SelectedClass
        {
            get { return (EntryClassViewModel)GetValue(SelectedClassProperty); }
            set { SetValue(SelectedClassProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedClass.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedClassProperty =
            DependencyProperty.Register("SelectedClass", typeof(EntryClassViewModel), typeof(EntryMarksViewModel), new PropertyMetadata(null, SelectedClassChanged));

        private static void SelectedClassChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EntryMarksViewModel entryMarks = (EntryMarksViewModel)d;
            entryMarks.SelectedClassChangedEvent?.Invoke();
        }

        public ObservableCollection<PeriodViewModel> Periods { get; set; }

        public PeriodViewModel SelectedPeriod
        {
            get { return (PeriodViewModel)GetValue(SelectedPeriodProperty); }
            set { SetValue(SelectedPeriodProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedPeriod.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedPeriodProperty =
            DependencyProperty.Register("SelectedPeriod", typeof(PeriodViewModel), typeof(EntryMarksViewModel), new PropertyMetadata(null));

        public ObservableCollection<TeacherViewModel> Teachers { get; set; }

        public TeacherViewModel SelectedTeacher
        {
            get { return (TeacherViewModel)GetValue(SelectedTeacherProperty); }
            set { SetValue(SelectedTeacherProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedTeacher.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedTeacherProperty =
            DependencyProperty.Register("SelectedTeacher", typeof(TeacherViewModel), typeof(EntryMarksViewModel), new PropertyMetadata(null, SelectedTeacherChanged));

        private static void SelectedTeacherChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EntryMarksViewModel entryMarks = (EntryMarksViewModel)d;
            entryMarks.Load();
        }

        public EntryMarksViewModel()
        {
            Classes = new ObservableCollection<EntryClassViewModel>();
            Periods = new ObservableCollection<PeriodViewModel>(MainViewModel.Instance.Parameters.Periods);
            Teachers = new ObservableCollection<TeacherViewModel>(MainViewModel.Instance.Parameters.Teachers);

            SelectedPeriod = Periods.FirstOrDefault(p => p.FromDate <= DateTime.Now.Date && p.ToDate > DateTime.Now.Date.AddDays(1));
            if (SelectedPeriod == null)
            {
                SelectedPeriod = Periods.FirstOrDefault();
            }

            if (MainViewModel.Instance.User.Teacher != null)
            {
                SelectedTeacher = MainViewModel.Instance.Parameters.Teachers.FirstOrDefault(t => t.Id == MainViewModel.Instance.User.Teacher.Id);
            }
        }

        public void Load()
        {
            Classes.Clear();
            if (SelectedTeacher != null)
            {
                foreach (ClassViewModel _class in MainViewModel.Instance.Parameters.Classes.Where(c => c.Teachers.Contains(SelectedTeacher)))
                {
                    EntryClassViewModel entryClass = new EntryClassViewModel() { Class = _class };
                    foreach (StudentViewModel student in _class.Students)
                    {
                        EntryStudentViewModel entryStudent = new EntryStudentViewModel() { Student = student };
                        entryStudent.LoadEntryMarks(SelectedTeacher);
                        entryClass.Students.Add(entryStudent);
                    }
                    Classes.Add(entryClass);
                }

                SelectedClass = Classes.FirstOrDefault();
                if (SelectedClass != null)
                {
                    SelectedClass.SelectedStudent = SelectedClass.Students.FirstOrDefault();
                    if (SelectedClass.SelectedStudent != null)
                    {
                        SelectedClass.SelectedStudent.SelectedMarksSubject = SelectedClass.SelectedStudent.MarksSubjects.FirstOrDefault();
                    }
                }
            }
        }
    }
}

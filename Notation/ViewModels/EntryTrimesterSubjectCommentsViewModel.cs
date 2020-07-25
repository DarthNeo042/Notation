using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Notation.ViewModels
{
    public class EntryTrimesterSubjectCommentsViewModel : DependencyObject
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
            DependencyProperty.Register("SelectedClass", typeof(EntryClassViewModel), typeof(EntryTrimesterSubjectCommentsViewModel), new PropertyMetadata(null, SelectedClassChanged));

        private static void SelectedClassChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EntryTrimesterSubjectCommentsViewModel entryTrimesterSubjectComments = (EntryTrimesterSubjectCommentsViewModel)d;
            entryTrimesterSubjectComments.SelectedClassChangedEvent?.Invoke();
        }

        public ObservableCollection<int> Trimesters { get; set; }

        public int SelectedTrimester
        {
            get { return (int)GetValue(SelectedTrimesterProperty); }
            set { SetValue(SelectedTrimesterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedTrimester.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedTrimesterProperty =
            DependencyProperty.Register("SelectedTrimester", typeof(int), typeof(EntryTrimesterSubjectCommentsViewModel), new PropertyMetadata(0, SelectedTrimesterChanged));

        private static void SelectedTrimesterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EntryTrimesterSubjectCommentsViewModel entryTrimesterSubjectComments = (EntryTrimesterSubjectCommentsViewModel)d;
            entryTrimesterSubjectComments.FromDate = MainViewModel.Instance.Parameters.Periods.Where(p => p.Trimester == entryTrimesterSubjectComments.SelectedTrimester).OrderBy(p => p.Number).First().FromDate;
            entryTrimesterSubjectComments.ToDate = MainViewModel.Instance.Parameters.Periods.Where(p => p.Trimester == entryTrimesterSubjectComments.SelectedTrimester).OrderByDescending(p => p.Number).First().ToDate;
        }

        public DateTime FromDate
        {
            get { return (DateTime)GetValue(FromDateProperty); }
            set { SetValue(FromDateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FromDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FromDateProperty =
            DependencyProperty.Register("FromDate", typeof(DateTime), typeof(EntryTrimesterSubjectCommentsViewModel), new PropertyMetadata(DateTime.Now));

        public DateTime ToDate
        {
            get { return (DateTime)GetValue(ToDateProperty); }
            set { SetValue(ToDateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ToDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ToDateProperty =
            DependencyProperty.Register("ToDate", typeof(DateTime), typeof(EntryTrimesterSubjectCommentsViewModel), new PropertyMetadata(DateTime.Now));

        public ObservableCollection<TeacherViewModel> Teachers { get; set; }

        public TeacherViewModel SelectedTeacher
        {
            get { return (TeacherViewModel)GetValue(SelectedTeacherProperty); }
            set { SetValue(SelectedTeacherProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedTeacher.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedTeacherProperty =
            DependencyProperty.Register("SelectedTeacher", typeof(TeacherViewModel), typeof(EntryTrimesterSubjectCommentsViewModel), new PropertyMetadata(null, SelectedTeacherChanged));

        private static void SelectedTeacherChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((EntryTrimesterSubjectCommentsViewModel)d).Load();
        }

        public EntryTrimesterSubjectCommentsViewModel()
        {
            Classes = new ObservableCollection<EntryClassViewModel>();
            Trimesters = new ObservableCollection<int>(MainViewModel.Instance.Parameters.Periods.Select(p => p.Trimester).Distinct());
            Teachers = new ObservableCollection<TeacherViewModel>(MainViewModel.Instance.Parameters.Teachers);

            PeriodViewModel period = MainViewModel.Instance.Parameters.Periods.FirstOrDefault(p => p.FromDate <= DateTime.Now.Date && p.ToDate > DateTime.Now.Date.AddDays(1));
            SelectedTrimester = period != null ? period.Trimester : 1;

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
                        entryStudent.LoadEntryTrimesterSubjectComments(SelectedTeacher);
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
                        SelectedClass.SelectedStudent.SelectedTrimesterSubjectCommentsSubject = SelectedClass.SelectedStudent.TrimesterSubjectCommentsSubjects.FirstOrDefault();
                    }
                }
            }
        }
    }
}

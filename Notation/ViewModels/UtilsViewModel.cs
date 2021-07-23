using Notation.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Notation.ViewModels
{
    public class UtilsViewModel : DependencyObject
    {
        public ObservableCollection<PeriodViewModel> Periods { get; set; }

        public PeriodViewModel SelectedPeriod
        {
            get { return (PeriodViewModel)GetValue(SelectedPeriodProperty); }
            set { SetValue(SelectedPeriodProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedPeriod.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedPeriodProperty =
            DependencyProperty.Register("SelectedPeriod", typeof(PeriodViewModel), typeof(UtilsViewModel), new PropertyMetadata(null, SelectedPeriodChanged));

        private static void SelectedPeriodChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UtilsViewModel utils = (UtilsViewModel)d;
            utils.Classes.Clear();
            if (utils.SelectedPeriod != null)
            {
                foreach (int idClass in MarkModel.Read(MainViewModel.Instance.SelectedYear, utils.SelectedPeriod.Id).Select(m => m.IdClass).Distinct())
                {
                    utils.Classes.Add(MainViewModel.Instance.Parameters.Classes.FirstOrDefault(c => c.Id == idClass));
                }
            }
            utils.SelectedClass = utils.Classes.FirstOrDefault();
        }

        public ObservableCollection<ClassViewModel> Classes { get; set; }

        public ClassViewModel SelectedClass
        {
            get { return (ClassViewModel)GetValue(SelectedClassProperty); }
            set { SetValue(SelectedClassProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedClass.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedClassProperty =
            DependencyProperty.Register("SelectedClass", typeof(ClassViewModel), typeof(UtilsViewModel), new PropertyMetadata(null, SelectedClassChanged));

        private static void SelectedClassChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UtilsViewModel utils = (UtilsViewModel)d;
            utils.Subjects.Clear();
            if (utils.SelectedClass != null)
            {
                foreach (int idSubject in MarkModel.Read(MainViewModel.Instance.SelectedYear, utils.SelectedPeriod.Id).Where(m => m.IdClass == utils.SelectedClass.Id).Select(m => m.IdSubject).Distinct())
                {
                    utils.Subjects.Add(MainViewModel.Instance.Parameters.Subjects.FirstOrDefault(s => s.Id == idSubject));
                }
            }
            utils.SelectedSubject = utils.Subjects.FirstOrDefault();
        }

        public ObservableCollection<SubjectViewModel> Subjects { get; set; }

        public SubjectViewModel SelectedSubject
        {
            get { return (SubjectViewModel)GetValue(SelectedSubjectProperty); }
            set { SetValue(SelectedSubjectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedSubject.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedSubjectProperty =
            DependencyProperty.Register("SelectedSubject", typeof(SubjectViewModel), typeof(UtilsViewModel), new PropertyMetadata(null, SelectedSubjectChanged));

        private static void SelectedSubjectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UtilsViewModel utils = (UtilsViewModel)d;
            utils.Teachers.Clear();
            if (utils.SelectedClass != null)
            {
                if (utils.SelectedSubject != null)
                {
                    foreach (TeacherViewModel teacher in MainViewModel.Instance.Parameters.Teachers.Where(t => t.Classes.Any(c => c.Id == utils.SelectedClass.Id)
                        && t.Subjects.Any(s => s.Id == (utils.SelectedSubject?.ParentSubject?.Id ?? utils.SelectedSubject.Id))))
                    {
                        utils.Teachers.Add(teacher);
                    }
                }
                int idTeacher = MarkModel.Read(MainViewModel.Instance.SelectedYear, utils.SelectedPeriod.Id).FirstOrDefault(m => m.IdClass == utils.SelectedClass.Id && m.IdSubject == utils.SelectedSubject.Id)?.IdTeacher ?? 0;
                if (idTeacher != 0)
                {
                    utils.SelectedTeacher = MainViewModel.Instance.Parameters.Teachers.FirstOrDefault(t => t.Id == idTeacher);
                }
                else
                {
                    utils.SelectedTeacher = utils.Teachers.FirstOrDefault();
                }
                utils.NbMarks = MarkModel.Read(MainViewModel.Instance.SelectedYear, utils.SelectedPeriod.Id).Count(m => m.IdClass == utils.SelectedClass.Id && m.IdSubject == utils.SelectedSubject.Id);
                utils.ActualTeacher = utils.SelectedTeacher;
            }
        }

        public int NbMarks
        {
            get { return (int)GetValue(NbMarksProperty); }
            set { SetValue(NbMarksProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NbMarks.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NbMarksProperty =
            DependencyProperty.Register("NbMarks", typeof(int), typeof(UtilsViewModel), new PropertyMetadata(0));

        public TeacherViewModel ActualTeacher
        {
            get { return (TeacherViewModel)GetValue(ActualTeacherProperty); }
            set { SetValue(ActualTeacherProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ActualTeacher.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActualTeacherProperty =
            DependencyProperty.Register("ActualTeacher", typeof(TeacherViewModel), typeof(UtilsViewModel), new PropertyMetadata(null));

        public ObservableCollection<TeacherViewModel> Teachers { get; set; }

        public TeacherViewModel SelectedTeacher
        {
            get { return (TeacherViewModel)GetValue(SelectedTeacherProperty); }
            set { SetValue(SelectedTeacherProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedTeacher.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedTeacherProperty =
            DependencyProperty.Register("SelectedTeacher", typeof(TeacherViewModel), typeof(UtilsViewModel), new PropertyMetadata(null));

        public ICommand AffectTeacherMarksCommand { get; set; }

        private void AffectTeacherMarksCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = SelectedPeriod != null && SelectedClass != null && SelectedSubject != null && SelectedTeacher != null && SelectedTeacher != ActualTeacher;
        }

        private void AffectTeacherMarksExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (MessageBox.Show($"Voulez vous changer le professeur de ces {NbMarks} notes\r\n"
                + $"de {ActualTeacher.Title} {ActualTeacher.FirstName} {ActualTeacher.LastName}\r\n"
                + $"en {SelectedTeacher.Title} {SelectedTeacher.FirstName} {SelectedTeacher.LastName} ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                List<MarkModel> clearMarks = new List<MarkModel>();
                foreach (StudentViewModel student in MainViewModel.Instance.Parameters.Classes.FirstOrDefault(c => c.Id == SelectedClass.Id).Students)
                {
                    clearMarks.Add(new MarkModel()
                    {
                        IdClass = SelectedClass.Id,
                        IdPeriod = SelectedPeriod.Id,
                        IdStudent = student.Id,
                        IdSubject = SelectedSubject.Id,
                        IdTeacher = ActualTeacher.Id,
                    });
                }
                MarkModel.Clear(clearMarks, MainViewModel.Instance.SelectedYear);
                IEnumerable<MarkModel> marks = MarkModel.Read(MainViewModel.Instance.SelectedYear, SelectedPeriod.Id).Where(m => m.IdClass == SelectedClass.Id && m.IdSubject == SelectedSubject.Id);
                foreach (MarkModel mark in marks)
                {
                    mark.IdTeacher = SelectedTeacher.Id;
                }
                MarkModel.Save(marks, MainViewModel.Instance.SelectedYear);
                MessageBox.Show("Changement de professeur réussi.", "Succés", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public CommandBindingCollection Bindings { get; set; }

        public UtilsViewModel()
        {
            Periods = new ObservableCollection<PeriodViewModel>();
            Classes = new ObservableCollection<ClassViewModel>();
            Subjects = new ObservableCollection<SubjectViewModel>();
            Teachers = new ObservableCollection<TeacherViewModel>();

            AffectTeacherMarksCommand = new RoutedUICommand("AffectTeacherMarks", "AffectTeacherMarks", typeof(MainViewModel));

            Bindings = new CommandBindingCollection()
            {
                new CommandBinding(AffectTeacherMarksCommand, AffectTeacherMarksExecuted, AffectTeacherMarksCanExecute),
            };
        }

        public void LoadData()
        {
            MainViewModel.Instance.Parameters.Periods.CollectionChanged += Periods_CollectionChanged;
            Periods_CollectionChanged(null, null);
        }

        private void Periods_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Periods.Clear();
            foreach (PeriodViewModel period in MainViewModel.Instance.Parameters.Periods)
            {
                Periods.Add(period);
            }
            SelectedPeriod = Periods.FirstOrDefault();
        }
    }
}

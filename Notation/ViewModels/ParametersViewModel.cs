using Notation.Models;
using Notation.Views;
using Notation.Settings;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Notation.ViewModels
{
    public class ParametersViewModel : DependencyObject
    {
        public int Year { get; set; }

        public PeriodViewModel SelectedPeriod
        {
            get { return (PeriodViewModel)GetValue(SelectedPeriodProperty); }
            set { SetValue(SelectedPeriodProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedPeriod.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedPeriodProperty =
            DependencyProperty.Register("SelectedPeriod", typeof(PeriodViewModel), typeof(ParametersViewModel), new PropertyMetadata(null, SelectedPeriodChanged));

        private static void SelectedPeriodChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ParametersViewModel ParametersViewModel = (ParametersViewModel)d;
            ParametersViewModel.ModificationPeriod = null;
        }

        public PeriodViewModel ModificationPeriod
        {
            get { return (PeriodViewModel)GetValue(ModificationPeriodProperty); }
            set { SetValue(ModificationPeriodProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ModificationPeriod.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ModificationPeriodProperty =
            DependencyProperty.Register("ModificationPeriod", typeof(PeriodViewModel), typeof(ParametersViewModel), new PropertyMetadata(null));

        public ObservableCollection<PeriodViewModel> Periods { get; set; }

        public SemiTrimesterViewModel SelectedSemiTrimester
        {
            get { return (SemiTrimesterViewModel)GetValue(SelectedSemiTrimesterProperty); }
            set { SetValue(SelectedSemiTrimesterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedSemiTrimester.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedSemiTrimesterProperty =
            DependencyProperty.Register("SelectedSemiTrimester", typeof(SemiTrimesterViewModel), typeof(ParametersViewModel), new PropertyMetadata(null, SelectedSemiTrimesterChanged));

        private static void SelectedSemiTrimesterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ParametersViewModel ParametersViewModel = (ParametersViewModel)d;
            ParametersViewModel.ModificationSemiTrimester = null;
        }

        public SemiTrimesterViewModel ModificationSemiTrimester
        {
            get { return (SemiTrimesterViewModel)GetValue(ModificationSemiTrimesterProperty); }
            set { SetValue(ModificationSemiTrimesterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ModificationSemiTrimester.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ModificationSemiTrimesterProperty =
            DependencyProperty.Register("ModificationSemiTrimester", typeof(SemiTrimesterViewModel), typeof(ParametersViewModel), new PropertyMetadata(null));

        public ObservableCollection<SemiTrimesterViewModel> SemiTrimesters { get; set; }

        public LevelViewModel SelectedLevel
        {
            get { return (LevelViewModel)GetValue(SelectedLevelProperty); }
            set { SetValue(SelectedLevelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedLevel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedLevelProperty =
            DependencyProperty.Register("SelectedLevel", typeof(LevelViewModel), typeof(ParametersViewModel), new PropertyMetadata(null));

        public LevelViewModel ModificationLevel
        {
            get { return (LevelViewModel)GetValue(ModificationLevelProperty); }
            set { SetValue(ModificationLevelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ModificationLevel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ModificationLevelProperty =
            DependencyProperty.Register("ModificationLevel", typeof(LevelViewModel), typeof(ParametersViewModel), new PropertyMetadata(null));

        public ObservableCollection<LevelViewModel> Levels { get; set; }

        public SubjectViewModel SelectedSubject
        {
            get { return (SubjectViewModel)GetValue(SelectedSubjectProperty); }
            set { SetValue(SelectedSubjectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedSubject.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedSubjectProperty =
            DependencyProperty.Register("SelectedSubject", typeof(SubjectViewModel), typeof(ParametersViewModel), new PropertyMetadata(null));

        public SubjectViewModel ModificationSubject
        {
            get { return (SubjectViewModel)GetValue(ModificationSubjectProperty); }
            set { SetValue(ModificationSubjectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ModificationSubject.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ModificationSubjectProperty =
            DependencyProperty.Register("ModificationSubject", typeof(SubjectViewModel), typeof(ParametersViewModel), new PropertyMetadata(null));

        public ObservableCollection<SubjectViewModel> Subjects { get; set; }

        public TeacherViewModel SelectedTeacher
        {
            get { return (TeacherViewModel)GetValue(SelectedTeacherProperty); }
            set { SetValue(SelectedTeacherProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedTeacher.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedTeacherProperty =
            DependencyProperty.Register("SelectedTeacher", typeof(TeacherViewModel), typeof(ParametersViewModel), new PropertyMetadata(null));

        public TeacherViewModel ModificationTeacher
        {
            get { return (TeacherViewModel)GetValue(ModificationTeacherProperty); }
            set { SetValue(ModificationTeacherProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ModificationTeacher.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ModificationTeacherProperty =
            DependencyProperty.Register("ModificationTeacher", typeof(TeacherViewModel), typeof(ParametersViewModel), new PropertyMetadata(null));

        public ObservableCollection<TeacherViewModel> Teachers { get; set; }

        public ClassViewModel SelectedClass
        {
            get { return (ClassViewModel)GetValue(SelectedClassProperty); }
            set { SetValue(SelectedClassProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedClass.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedClassProperty =
            DependencyProperty.Register("SelectedClass", typeof(ClassViewModel), typeof(ParametersViewModel), new PropertyMetadata(null));

        public ClassViewModel ModificationClass
        {
            get { return (ClassViewModel)GetValue(ModificationClassProperty); }
            set { SetValue(ModificationClassProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ModificationClass.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ModificationClassProperty =
            DependencyProperty.Register("ModificationClass", typeof(ClassViewModel), typeof(ParametersViewModel), new PropertyMetadata(null));

        public ObservableCollection<ClassViewModel> Classes { get; set; }

        public StudentViewModel SelectedStudent
        {
            get { return (StudentViewModel)GetValue(SelectedStudentProperty); }
            set { SetValue(SelectedStudentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedStudent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedStudentProperty =
            DependencyProperty.Register("SelectedStudent", typeof(StudentViewModel), typeof(ParametersViewModel), new PropertyMetadata(null));

        public StudentViewModel ModificationStudent
        {
            get { return (StudentViewModel)GetValue(ModificationStudentProperty); }
            set { SetValue(ModificationStudentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ModificationStudent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ModificationStudentProperty =
            DependencyProperty.Register("ModificationStudent", typeof(StudentViewModel), typeof(ParametersViewModel), new PropertyMetadata(null));

        public ObservableCollection<StudentViewModel> Students { get; set; }

        public BaseParametersViewModel BaseParameters
        {
            get { return (BaseParametersViewModel)GetValue(BaseParametersProperty); }
            set { SetValue(BaseParametersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BaseParameters.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BaseParametersProperty =
            DependencyProperty.Register("BaseParameters", typeof(BaseParametersViewModel), typeof(ParametersViewModel), new PropertyMetadata(null, BaseParametersChanged));

        public delegate void BaseParametersChangedEventHandler();

        public event BaseParametersChangedEventHandler BaseParametersChangedEvent;

        private static void BaseParametersChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ParametersViewModel)d).BaseParametersChangedEvent?.Invoke();
        }

        public YearParametersViewModel YearParameters
        {
            get { return (YearParametersViewModel)GetValue(YearParametersProperty); }
            set { SetValue(YearParametersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for YearParameters.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty YearParametersProperty =
            DependencyProperty.Register("YearParameters", typeof(YearParametersViewModel), typeof(ParametersViewModel), new PropertyMetadata(null));

        public CalendarViewModel Calendar
        {
            get { return (CalendarViewModel)GetValue(CalendarProperty); }
            set { SetValue(CalendarProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Calendar.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CalendarProperty =
            DependencyProperty.Register("Calendar", typeof(CalendarViewModel), typeof(ParametersViewModel), new PropertyMetadata(null));

        public UtilsViewModel Utils
        {
            get { return (UtilsViewModel)GetValue(UtilsProperty); }
            set { SetValue(UtilsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Utils.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UtilsProperty =
            DependencyProperty.Register("Utils", typeof(UtilsViewModel), typeof(ParametersViewModel), new PropertyMetadata(null));

        public ICommand AddPeriodCommand { get; set; }

        private void AddPeriodCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Year != 0;
        }

        private void AddPeriodExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ModificationPeriod = new PeriodViewModel() { Year = Year };
            ModificationPeriod.InitNumber();
        }

        public ICommand ModifyPeriodCommand { get; set; }

        private void ModifyPeriodCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = SelectedPeriod != null;
        }

        private void ModifyPeriodExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ModificationPeriod = new PeriodViewModel()
            {
                FromDate = SelectedPeriod.FromDate,
                Id = SelectedPeriod.Id,
                Order = SelectedPeriod.Order,
                Number = SelectedPeriod.Number,
                ToDate = SelectedPeriod.ToDate,
                Trimester = SelectedPeriod.Trimester,
                Year = SelectedPeriod.Year,
            };
        }

        public ICommand DeletePeriodCommand { get; set; }

        private void DeletePeriodCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = SelectedPeriod != null;
        }

        private void DeletePeriodExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (SemiTrimesterModel.CanDeleteAll(SelectedPeriod.Year) && PeriodModel.CanDelete(SelectedPeriod.Year, SelectedPeriod.Id))
            {
                SemiTrimesterModel.DeleteAll(SelectedPeriod.Year);
                PeriodModel.Delete(SelectedPeriod.Year, SelectedPeriod.Id);
                LoadData();
                GenerateSemiTrimester();
                SelectedPeriod = Periods.FirstOrDefault();
            }
            else
            {
                MessageBox.Show("Impossible de supprimer des périodes une fois la saisie des bulletins commencée.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ICommand SavePeriodCommand { get; set; }

        private void SavePeriodExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            PeriodModel.Save(ModificationPeriod);
            LoadData(ModificationPeriod);
            GenerateSemiTrimester();
        }

        public ICommand CancelPeriodCommand { get; set; }

        private void CancelPeriodExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ModificationPeriod = null;
        }

        public ICommand ModifySemiTrimesterCommand { get; set; }

        private void ModifySemiTrimesterCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = SelectedSemiTrimester != null;
        }

        private void ModifySemiTrimesterExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ModificationSemiTrimester = new SemiTrimesterViewModel()
            {
                Id = SelectedSemiTrimester.Id,
                Year = SelectedSemiTrimester.Year,
                Period1 = SelectedSemiTrimester.Period1,
                Period2 = SelectedSemiTrimester.Period2,
                Name = SelectedSemiTrimester.Name,
            };
        }

        public ICommand AddSemiTrimesterPeriodCommand { get; set; }

        private void AddSemiTrimesterPeriodCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            int index = SemiTrimesters.IndexOf(SelectedSemiTrimester);
            bool canExecute = false;
            if (SemiTrimesters.Count > index + 1)
            {
                SemiTrimesterViewModel semiTrimester = SemiTrimesters[index + 1];
                canExecute = SelectedSemiTrimester != null && SelectedSemiTrimester.Period2 == null && semiTrimester.Period2 == null
                    && SelectedSemiTrimester.Period1.Trimester == semiTrimester.Period1.Trimester;
            }
            e.CanExecute = canExecute;
        }

        private void AddSemiTrimesterPeriodExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (SemiTrimesterModel.CanDeleteAll(SelectedSemiTrimester.Year))
            {
                int index = SemiTrimesters.IndexOf(SelectedSemiTrimester);
                SelectedSemiTrimester.Period2 = SemiTrimesters[index + 1].Period1;
                SemiTrimesters.RemoveAt(index + 1);
                SemiTrimesterModel.Save(SemiTrimesters);
                LoadData(SelectedSemiTrimester);
            }
            else
            {
                MessageBox.Show("Impossible de modifier les demi-trimestres une fois la saisie des bulletins commencée.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ICommand DeleteSemiTrimesterPeriodCommand { get; set; }

        private void DeleteSemiTrimesterPeriodCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = SelectedSemiTrimester != null && SelectedSemiTrimester.Period2 != null;
        }

        private void DeleteSemiTrimesterPeriodExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (SemiTrimesterModel.CanDeleteAll(SelectedSemiTrimester.Year))
            {
                SemiTrimesters.Add(new SemiTrimesterViewModel()
                {
                    Period1 = SelectedSemiTrimester.Period2,
                    Year = SelectedSemiTrimester.Year,
                });
                SelectedSemiTrimester.Period2 = null;
                SemiTrimesterModel.Save(SemiTrimesters);
                LoadData(SelectedSemiTrimester);
            }
            else
            {
                MessageBox.Show("Impossible de modifier les demi-trimestres une fois la saisie des bulletins commencée.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ICommand SaveSemiTrimesterCommand { get; set; }

        private void SaveSemiTrimesterExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SemiTrimesterModel.Save(ModificationSemiTrimester);
            LoadData(ModificationSemiTrimester);
        }

        public ICommand CancelSemiTrimesterCommand { get; set; }

        private void CancelSemiTrimesterExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ModificationSemiTrimester = null;
        }

        public ICommand AddLevelCommand { get; set; }

        private void AddLevelCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Year != 0;
        }

        private void AddLevelExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ModificationLevel = new LevelViewModel() { Year = Year, Order = Levels.Count };
        }

        public ICommand ModifyLevelCommand { get; set; }

        private void ModifyLevelCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = SelectedLevel != null;
        }

        private void ModifyLevelExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ModificationLevel = new LevelViewModel()
            {
                Id = SelectedLevel.Id,
                Year = SelectedLevel.Year,
                Order = SelectedLevel.Order,
                Name = SelectedLevel.Name,
                Subjects = new ObservableCollection<SubjectViewModel>(SelectedLevel.Subjects),
            };
        }

        public ICommand DeleteLevelCommand { get; set; }

        private void DeleteLevelCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = SelectedLevel != null;
        }

        private void DeleteLevelExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            foreach (ClassViewModel _class in Classes.Where(c => c.Level?.Id == SelectedLevel.Id))
            {
                _class.Level = null;
                ClassModel.Save(_class);
            }
            LevelModel.Delete(SelectedLevel.Year, SelectedLevel.Id);
            LoadData();
            SelectedLevel = Levels.FirstOrDefault();
        }

        public ICommand UpLevelCommand { get; set; }

        private void UpLevelCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = SelectedLevel != null && SelectedLevel.Order > 0;
        }

        private void UpLevelExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SelectedLevel.Order = SelectedLevel.Order - 1;
            LevelModel.Save(SelectedLevel);
            LevelViewModel level = Levels[SelectedLevel.Order];
            level.Order = level.Order + 1;
            LevelModel.Save(level);
            LoadData(SelectedLevel);
        }

        public ICommand DownLevelCommand { get; set; }

        private void DownLevelCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = SelectedLevel != null && SelectedLevel.Order < Levels.Count - 1;
        }

        private void DownLevelExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SelectedLevel.Order = SelectedLevel.Order + 1;
            LevelModel.Save(SelectedLevel);
            LevelViewModel level = Levels[SelectedLevel.Order];
            level.Order = level.Order - 1;
            LevelModel.Save(level);
            LoadData(SelectedLevel);
        }

        public ICommand AddLevelSubjectsCommand { get; set; }

        private void AddLevelSubjectsCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ModificationLevel != null && Subjects.Any(s => s.ParentSubject == null && !ModificationLevel.Subjects.Contains(s));
        }

        private void AddLevelSubjectsExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            AddSubjects form = new AddSubjects(Subjects.Where(s => s.ParentSubject == null && !ModificationLevel.Subjects.Contains(s)));
            if (form.ShowDialog() ?? false)
            {
                foreach (SubjectViewModel subject in form.Subjects.Where(s => s.Selected))
                {
                    ModificationLevel.Subjects.Add(subject);
                }
            }
        }

        public ICommand DeleteLevelSubjectCommand { get; set; }

        private void DeleteLevelSubjectCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ModificationLevel != null && ModificationLevel.SelectedSubject != null;
        }

        private void DeleteLevelSubjectExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ModificationLevel.Subjects.Remove(ModificationLevel.SelectedSubject);
        }

        public ICommand SaveLevelCommand { get; set; }

        private void SaveLevelExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ModificationLevel.Name))
            {
                LevelModel.Save(ModificationLevel);
                ModificationLevel.Id = LevelModel.Read(ModificationLevel.Year).LastOrDefault(l => l.Name == ModificationLevel.Name).Id;
                LevelSubjectModel.SaveLevelSubjects(ModificationLevel);
                LoadData(ModificationLevel);
            }
            else
            {
                MessageBox.Show("Vous devez saisir un nom.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ICommand CancelLevelCommand { get; set; }

        private void CancelLevelExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ModificationLevel = null;
        }

        public ICommand AddSubjectCommand { get; set; }

        private void AddSubjectCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Year != 0;
        }

        private void AddSubjectExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ModificationSubject = new SubjectViewModel() { Year = Year, Order = Subjects.Count };
            ModificationSubject.LoadParentSubjects();
        }

        public ICommand ModifySubjectCommand { get; set; }

        private void ModifySubjectCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = SelectedSubject != null;
        }

        private void ModifySubjectExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ModificationSubject = new SubjectViewModel()
            {
                Id = SelectedSubject.Id,
                Year = SelectedSubject.Year,
                Order = SelectedSubject.Order,
                Coefficient = SelectedSubject.Coefficient,
                Name = SelectedSubject.Name,
                Option = SelectedSubject.Option,
                ParentSubject = SelectedSubject.ParentSubject,
                Levels = new ObservableCollection<LevelViewModel>(SelectedSubject.Levels),
                Teachers = new ObservableCollection<TeacherViewModel>(SelectedSubject.Teachers),
            };
            ModificationSubject.LoadParentSubjects();
        }

        public ICommand DeleteSubjectCommand { get; set; }

        private void DeleteSubjectCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = SelectedSubject != null;
        }

        private void DeleteSubjectExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (SubjectModel.CanDelete(SelectedSubject.Year, SelectedSubject.Id))
            {
                SelectedSubject.Levels.Clear();
                LevelSubjectModel.SaveSubjectLevels(SelectedSubject);
                foreach (TeacherViewModel teacher in Teachers)
                {
                    SubjectViewModel subject = teacher.Subjects.FirstOrDefault(s => s.Id == SelectedSubject.Id);
                    if (subject != null)
                    {
                        teacher.Subjects.Remove(subject);
                    }
                    SubjectTeacherModel.SaveTeacherSubjects(teacher);
                }
                SubjectModel.Delete(SelectedSubject.Year, SelectedSubject.Id);
                LoadData();
                SelectedSubject = Subjects.FirstOrDefault();
            }
            else
            {
                MessageBox.Show("Impossible de supprimer des matières une fois la saisie des bulletins commencée.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ICommand UpSubjectCommand { get; set; }

        private void UpSubjectCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            int index = Subjects.IndexOf(SelectedSubject);
            e.CanExecute = SelectedSubject != null && index > 0 && (SelectedSubject.ParentSubject == null || Subjects[index - 1].ParentSubject == SelectedSubject.ParentSubject);
        }

        private void UpSubjectExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SubjectViewModel subject = SelectedSubject;
            int index = Subjects.IndexOf(subject);
            if (subject.ParentSubject != null)
            {
                if (Subjects[index - 1].ParentSubject == subject.ParentSubject)
                {
                    Subjects.Remove(subject);
                    Subjects.Insert(index - 1, subject);
                }
            }
            else
            {
                index--;
                while (Subjects[index].ParentSubject != null)
                {
                    index--;
                }
                Subjects.Remove(subject);
                Subjects.Insert(index, subject);
            }
            SaveSubjectsOrder();
            LoadData(subject);
        }

        public ICommand DownSubjectCommand { get; set; }

        private void DownSubjectCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            int index = Subjects.IndexOf(SelectedSubject);
            e.CanExecute = SelectedSubject != null && index < Subjects.Count - 1;
            if (e.CanExecute)
            {
                if (SelectedSubject.ParentSubject != null)
                {
                    e.CanExecute = Subjects[index + 1].ParentSubject == SelectedSubject.ParentSubject;
                }
                else
                {
                    e.CanExecute = false;
                    index++;
                    while (index < Subjects.Count)
                    {
                        if (Subjects[index].ParentSubject == null)
                        {
                            e.CanExecute = true;
                            break;
                        }
                        index++;
                    }
                }
            }
        }

        private void DownSubjectExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SubjectViewModel subject = SelectedSubject;
            int index = Subjects.IndexOf(subject);
            if (subject.ParentSubject != null)
            {
                if (Subjects[index + 1].ParentSubject == subject.ParentSubject)
                {
                    Subjects.Remove(subject);
                    Subjects.Insert(index + 1, subject);
                }
            }
            else
            {
                index++;
                while (Subjects[index].ParentSubject != null)
                {
                    index++;
                }
                Subjects.Remove(subject);
                Subjects.Insert(index, subject);
            }
            SaveSubjectsOrder();
            LoadData(subject);
        }

        public void SaveSubjectsOrder()
        {
            int order = 0;
            foreach (SubjectViewModel subject in Subjects)
            {
                subject.Order = order;
                SubjectModel.Save(subject);
                order++;
            }
        }

        public ICommand AddSubjectLevelsCommand { get; set; }

        private void AddSubjectLevelsCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ModificationSubject != null && ModificationSubject.ParentSubject == null && Levels.Any(l => !ModificationSubject.Levels.Contains(l));
        }

        private void AddSubjectLevelsExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            AddLevels form = new AddLevels(Levels.Where(l => !ModificationSubject.Levels.Contains(l)));
            if (form.ShowDialog() ?? false)
            {
                foreach (LevelViewModel level in form.Levels.Where(l => l.Selected))
                {
                    ModificationSubject.Levels.Add(level);
                }
            }
        }

        public ICommand DeleteSubjectLevelCommand { get; set; }

        private void DeleteSubjectLevelCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ModificationSubject != null && ModificationSubject.ParentSubject == null && ModificationSubject.SelectedLevel != null;
        }

        private void DeleteSubjectLevelExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ModificationSubject.Levels.Remove(ModificationSubject.SelectedLevel);
        }

        public ICommand AddSubjectTeachersCommand { get; set; }

        private void AddSubjectTeachersCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ModificationSubject != null && ModificationSubject.ParentSubject == null && Teachers.Any(l => !ModificationSubject.Teachers.Contains(l));
        }

        private void AddSubjectTeachersExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            AddTeachers form = new AddTeachers(Teachers.Where(l => !ModificationSubject.Teachers.Contains(l)));
            if (form.ShowDialog() ?? false)
            {
                foreach (TeacherViewModel Teacher in form.Teachers.Where(l => l.Selected))
                {
                    ModificationSubject.Teachers.Add(Teacher);
                }
            }
        }

        public ICommand DeleteSubjectTeacherCommand { get; set; }

        private void DeleteSubjectTeacherCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ModificationSubject != null && ModificationSubject.ParentSubject == null && ModificationSubject.SelectedTeacher != null;
        }

        private void DeleteSubjectTeacherExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ModificationSubject.Teachers.Remove(ModificationSubject.SelectedTeacher);
        }

        public ICommand SaveSubjectCommand { get; set; }

        private void SaveSubjectExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ModificationSubject.Name))
            {
                if (ModificationSubject.ParentSubject != null && ModificationSubject.ParentSubject.Id == 0)
                {
                    ModificationSubject.ParentSubject = null;
                }
                SubjectModel.Save(ModificationSubject);
                IEnumerable<SubjectViewModel> subjects = SubjectModel.ReadParents(ModificationSubject.Year);
                subjects = SubjectModel.ReadChildren(ModificationSubject.Year, subjects);
                ModificationSubject.Id = subjects.LastOrDefault(s => s.Name == ModificationSubject.Name).Id;
                LevelSubjectModel.SaveSubjectLevels(ModificationSubject);
                SubjectTeacherModel.SaveSubjectTeachers(ModificationSubject);
                LoadData(ModificationSubject);
            }
            else
            {
                MessageBox.Show("Vous devez saisir un nom.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ICommand CancelSubjectCommand { get; set; }

        private void CancelSubjectExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ModificationSubject = null;
        }

        public ICommand AddTeacherCommand { get; set; }

        private void AddTeacherCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Year != 0;
        }

        private void AddTeacherExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ModificationTeacher = new TeacherViewModel() { Year = Year };
        }

        public ICommand ModifyTeacherCommand { get; set; }

        private void ModifyTeacherCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = SelectedTeacher != null;
        }

        private void ModifyTeacherExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ModificationTeacher = new TeacherViewModel()
            {
                Id = SelectedTeacher.Id,
                Year = SelectedTeacher.Year,
                FirstName = SelectedTeacher.FirstName,
                LastName = SelectedTeacher.LastName,
                Login = SelectedTeacher.Login,
                Password = SelectedTeacher.Password,
                Title = SelectedTeacher.Title,
                Subjects = new ObservableCollection<SubjectViewModel>(SelectedTeacher.Subjects),
                Classes = new ObservableCollection<ClassViewModel>(SelectedTeacher.Classes),
            };
        }

        public ICommand AddTeacherSubjectsCommand { get; set; }

        private void AddTeacherSubjectsCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ModificationTeacher != null && Subjects.Any(s => s.ParentSubject == null && !ModificationTeacher.Subjects.Contains(s));
        }

        private void AddTeacherSubjectsExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            AddSubjects form = new AddSubjects(Subjects.Where(s => s.ParentSubject == null && !ModificationTeacher.Subjects.Contains(s)));
            if (form.ShowDialog() ?? false)
            {
                foreach (SubjectViewModel subject in form.Subjects.Where(s => s.Selected))
                {
                    ModificationTeacher.Subjects.Add(subject);
                }
            }
        }

        public ICommand DeleteTeacherSubjectCommand { get; set; }

        private void DeleteTeacherSubjectCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ModificationTeacher != null && ModificationTeacher.SelectedSubject != null;
        }

        private void DeleteTeacherSubjectExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ModificationTeacher.Subjects.Remove(ModificationTeacher.SelectedSubject);
        }

        public ICommand AddTeacherClassesCommand { get; set; }

        private void AddTeacherClassesCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ModificationTeacher != null && Classes.Any(c => !ModificationTeacher.Classes.Contains(c));
        }

        private void AddTeacherClassesExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            AddClasses form = new AddClasses(Classes.Where(c => !ModificationTeacher.Classes.Contains(c)));
            if (form.ShowDialog() ?? false)
            {
                foreach (ClassViewModel Class in form.Classes.Where(s => s.Selected))
                {
                    ModificationTeacher.Classes.Add(Class);
                }
            }
        }

        public ICommand DeleteTeacherClassCommand { get; set; }

        private void DeleteTeacherClassCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ModificationTeacher != null && ModificationTeacher.SelectedClass != null;
        }

        private void DeleteTeacherClassExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ModificationTeacher.Classes.Remove(ModificationTeacher.SelectedClass);
        }

        public ICommand DeleteTeacherCommand { get; set; }

        private void DeleteTeacherCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = SelectedTeacher != null;
        }

        private void DeleteTeacherExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (TeacherModel.CanDelete(SelectedTeacher.Year, SelectedTeacher.Id))
            {
                SelectedTeacher.Classes.Clear();
                TeacherClassModel.SaveTeacherClasses(SelectedTeacher);
                SelectedTeacher.Subjects.Clear();
                SubjectTeacherModel.SaveTeacherSubjects(SelectedTeacher);
                foreach (ClassViewModel _class in Classes.Where(c => c.MainTeacher != null && c.MainTeacher.Id == SelectedTeacher.Id))
                {
                    _class.MainTeacher = null;
                    ClassModel.Save(_class);
                }
                TeacherModel.Delete(SelectedTeacher.Year, SelectedTeacher.Id);
                LoadData();
                SelectedTeacher = Teachers.FirstOrDefault();
            }
            else
            {
                MessageBox.Show("Impossible de supprimer des professeurs une fois la saisie des bulletins commencée.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ICommand SaveTeacherCommand { get; set; }

        private void SaveTeacherExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ModificationTeacher.LastName))
            {
                TeacherModel.Save(ModificationTeacher);
                ModificationTeacher.Id = TeacherModel.Read(ModificationTeacher.Year).LastOrDefault(l => l.LastName == ModificationTeacher.LastName && l.FirstName == ModificationTeacher.FirstName).Id;
                SubjectTeacherModel.SaveTeacherSubjects(ModificationTeacher);
                TeacherClassModel.SaveTeacherClasses(ModificationTeacher);
                LoadData(ModificationTeacher);
            }
            else
            {
                MessageBox.Show("Vous devez saisir un nom.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ICommand CancelTeacherCommand { get; set; }

        private void CancelTeacherExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ModificationTeacher = null;
        }

        public ICommand AddClassCommand { get; set; }

        private void AddClassCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Year != 0;
        }

        private void AddClassExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ModificationClass = new ClassViewModel() { Year = Year };
            ModificationClass.LoadMainTeachersLevels();
        }

        public ICommand ModifyClassCommand { get; set; }

        private void ModifyClassCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = SelectedClass != null;
        }

        private void ModifyClassExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ModificationClass = new ClassViewModel()
            {
                Id = SelectedClass.Id,
                Year = SelectedClass.Year,
                Name = SelectedClass.Name,
                MainTeacher = SelectedClass.MainTeacher,
                Level = SelectedClass.Level,
                Teachers = new ObservableCollection<TeacherViewModel>(SelectedClass.Teachers),
                Students = new ObservableCollection<StudentViewModel>(SelectedClass.Students),
            };
            ModificationClass.LoadMainTeachersLevels();
        }

        public ICommand DeleteClassCommand { get; set; }

        private void DeleteClassCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = SelectedClass != null;
        }

        private void DeleteClassExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (ClassModel.CanDelete(SelectedClass.Year, SelectedClass.Id))
            {
                foreach (StudentViewModel student in Students.Where(s => s.Class.Id == SelectedClass.Id))
                {
                    student.Class = null;
                    StudentModel.Save(student);
                }
                foreach (TeacherViewModel teacher in Teachers)
                {
                    ClassViewModel _class = teacher.Classes.FirstOrDefault(c => c.Id == SelectedClass.Id);
                    if (_class != null)
                    {
                        teacher.Classes.Remove(_class);
                    }
                    TeacherClassModel.SaveTeacherClasses(teacher);
                }
                ClassModel.Delete(SelectedClass.Year, SelectedClass.Id);
                LoadData();
            }
            else
            {
                MessageBox.Show("Impossible de supprimer des matières une fois la saisie des bulletins commencée.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            ClassModel.Delete(SelectedClass.Year, SelectedClass.Id);
            LoadData();
            SelectedClass = Classes.FirstOrDefault();
        }

        public ICommand UpClassCommand { get; set; }

        private void UpClassCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = SelectedClass != null && SelectedClass.Order > 0;
        }

        private void UpClassExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SelectedClass.Order = SelectedClass.Order - 1;
            ClassModel.Save(SelectedClass);
            ClassViewModel _class = Classes[SelectedClass.Order];
            _class.Order = _class.Order + 1;
            ClassModel.Save(_class);
            LoadData(SelectedClass);
        }

        public ICommand DownClassCommand { get; set; }

        private void DownClassCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = SelectedClass != null && SelectedClass.Order < Classes.Count - 1;
        }

        private void DownClassExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SelectedClass.Order = SelectedClass.Order + 1;
            ClassModel.Save(SelectedClass);
            ClassViewModel _class = Classes[SelectedClass.Order];
            _class.Order = _class.Order - 1;
            ClassModel.Save(_class);
            LoadData(SelectedClass);
        }

        public ICommand AddClassTeachersCommand { get; set; }

        private void AddClassTeachersCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ModificationClass != null && Teachers.Any(t => !ModificationClass.Teachers.Contains(t));
        }

        private void AddClassTeachersExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            AddTeachers form = new AddTeachers(Teachers.Where(t => !ModificationClass.Teachers.Contains(t)));
            if (form.ShowDialog() ?? false)
            {
                foreach (TeacherViewModel teacher in form.Teachers.Where(t => t.Selected))
                {
                    ModificationClass.Teachers.Add(teacher);
                }
            }
        }

        public ICommand DeleteClassTeacherCommand { get; set; }

        private void DeleteClassTeacherCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ModificationClass != null && ModificationClass.SelectedTeacher != null;
        }

        private void DeleteClassTeacherExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ModificationClass.Teachers.Remove(ModificationClass.SelectedTeacher);
        }

        public ICommand AddClassStudentsCommand { get; set; }

        private void AddClassStudentsCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ModificationClass != null && Students.Any(s => s.Class == null && !ModificationClass.Students.Contains(s));
        }

        private void AddClassStudentsExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            AddStudents form = new AddStudents(Students.Where(s => s.Class == null && !ModificationClass.Students.Contains(s)));
            if (form.ShowDialog() ?? false)
            {
                foreach (StudentViewModel Student in form.Students.Where(t => t.Selected))
                {
                    ModificationClass.Students.Add(Student);
                }
            }
        }

        public ICommand DeleteClassStudentCommand { get; set; }

        private void DeleteClassStudentCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ModificationClass != null && ModificationClass.SelectedStudent != null;
        }

        private void DeleteClassStudentExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ModificationClass.Students.Remove(ModificationClass.SelectedStudent);
        }

        public ICommand SaveClassCommand { get; set; }

        private void SaveClassExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ModificationClass.Name))
            {
                if (ModificationClass.MainTeacher != null && ModificationClass.MainTeacher.Id == 0)
                {
                    ModificationClass.MainTeacher = null;
                }
                if (ModificationClass.Level != null && ModificationClass.Level.Id == 0)
                {
                    ModificationClass.Level = null;
                }
                ClassModel.Save(ModificationClass);
                ModificationClass.Id = ClassModel.Read(ModificationClass.Year, MainViewModel.Instance.Parameters.Teachers, MainViewModel.Instance.Parameters.Levels).LastOrDefault(c => c.Name == ModificationClass.Name).Id;
                TeacherClassModel.SaveClassTeachers(ModificationClass);
                StudentModel.SaveClass(ModificationClass);
                LoadData(ModificationClass);
            }
            else
            {
                MessageBox.Show("Vous devez saisir un nom.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ICommand CancelClassCommand { get; set; }

        private void CancelClassExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ModificationClass = null;
        }

        public ICommand AddStudentCommand { get; set; }

        private void AddStudentCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Year != 0;
        }

        private void AddStudentExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ModificationStudent = new StudentViewModel() { Year = Year };
            ModificationStudent.LoadClasses();
        }

        public ICommand ModifyStudentCommand { get; set; }

        private void ModifyStudentCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = SelectedStudent != null;
        }

        private void ModifyStudentExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ModificationStudent = new StudentViewModel()
            {
                Id = SelectedStudent.Id,
                Year = SelectedStudent.Year,
                LastName = SelectedStudent.LastName,
                FirstName = SelectedStudent.FirstName,
                BirthDate = SelectedStudent.BirthDate,
                Class = SelectedStudent.Class,
            };
            ModificationStudent.LoadClasses();
        }

        public ICommand DeleteStudentCommand { get; set; }

        private void DeleteStudentCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = SelectedStudent != null;
        }

        private void DeleteStudentExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (!StudentModel.CanDelete(SelectedStudent.Year, SelectedStudent.Id))
            {
                if (MessageBox.Show("Voulez-vous supprimer les notes et commentaires associés à cet élève ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    StudentModel.DeleteMarksAndComments(SelectedStudent.Year, SelectedStudent.Id);
                }
                else
                {
                    return;
                }
            }
            StudentModel.Delete(SelectedStudent.Year, SelectedStudent.Id);
            LoadData();
            SelectedStudent = Students.FirstOrDefault();
        }

        public ICommand SaveStudentCommand { get; set; }

        private void SaveStudentExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ModificationStudent.FirstName) && !string.IsNullOrEmpty(ModificationStudent.LastName))
            {
                if (ModificationStudent.Class != null && ModificationStudent.Class.Id == 0)
                {
                    ModificationStudent.Class = null;
                }
                StudentModel.Save(ModificationStudent);
                ModificationStudent.Id = StudentModel.Read(ModificationStudent.Year, MainViewModel.Instance.Parameters.Classes)
                    .LastOrDefault(c => c.LastName == ModificationStudent.LastName && c.FirstName == ModificationStudent.FirstName).Id;
                LoadData(ModificationStudent);
            }
            else
            {
                MessageBox.Show("Vous devez saisir un nom et un prénom.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ICommand CancelStudentCommand { get; set; }

        private void CancelStudentExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ModificationStudent = null;
        }

        public ICommand PlusColorRCommand { get; set; }

        private void PlusColorRCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = BaseParameters.ColorR < 255;
        }

        private void PlusColorRExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            BaseParameters.ColorR = (byte)(BaseParameters.ColorR - (BaseParameters.ColorR % 5) + 5);
        }

        public ICommand MinusColorRCommand { get; set; }

        private void MinusColorRCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = BaseParameters.ColorR > 0;
        }

        private void MinusColorRExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            BaseParameters.ColorR = (byte)(BaseParameters.ColorR - (BaseParameters.ColorR % 5) - (BaseParameters.ColorR % 5 == 0 ? 5 : 0));
        }

        public ICommand PlusColorGCommand { get; set; }

        private void PlusColorGCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = BaseParameters.ColorG < 255;
        }

        private void PlusColorGExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            BaseParameters.ColorG = (byte)(BaseParameters.ColorG - (BaseParameters.ColorG % 5) + 5);
        }

        public ICommand MinusColorGCommand { get; set; }

        private void MinusColorGCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = BaseParameters.ColorG > 0;
        }

        private void MinusColorGExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            BaseParameters.ColorG = (byte)(BaseParameters.ColorG - (BaseParameters.ColorG % 5) - (BaseParameters.ColorG % 5 == 0 ? 5 : 0));
        }

        public ICommand PlusColorBCommand { get; set; }

        private void PlusColorBCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = BaseParameters.ColorB < 255;
        }

        private void PlusColorBExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            BaseParameters.ColorB = (byte)(BaseParameters.ColorB - (BaseParameters.ColorB % 5) + 5);
        }

        public ICommand MinusColorBCommand { get; set; }

        private void MinusColorBCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = BaseParameters.ColorB > 0;
        }

        private void MinusColorBExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            BaseParameters.ColorB = (byte)(BaseParameters.ColorB - (BaseParameters.ColorB % 5) - (BaseParameters.ColorB % 5 == 0 ? 5 : 0));
        }

        public ICommand SaveBaseParametersCommand { get; set; }

        private void SaveBaseParametersExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            BaseParametersModel.Save(BaseParameters);
            MessageBox.Show("Enregistrement réussi.", "Réussi", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public ICommand CancelBaseParametersCommand { get; set; }

        private void CancelBaseParametersExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            BaseParameters = BaseParametersModel.Read();
        }

        public ICommand SaveYearParametersCommand { get; set; }

        private void SaveYearParametersExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            YearParametersModel.Save(YearParameters);
            MessageBox.Show("Enregistrement réussi.", "Réussi", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public ICommand CancelYearParametersCommand { get; set; }

        private void CancelYearParametersExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            YearParameters = YearParametersModel.Read(Year);
        }

        public CommandBindingCollection Bindings { get; set; }

        public ParametersViewModel()
        {
            Periods = new ObservableCollection<PeriodViewModel>();
            SemiTrimesters = new ObservableCollection<SemiTrimesterViewModel>();
            Levels = new ObservableCollection<LevelViewModel>();
            Subjects = new ObservableCollection<SubjectViewModel>();
            Teachers = new ObservableCollection<TeacherViewModel>();
            Classes = new ObservableCollection<ClassViewModel>();
            Students = new ObservableCollection<StudentViewModel>();

            Calendar = new CalendarViewModel();
            Utils = new UtilsViewModel();
            YearParameters = new YearParametersViewModel();

            GetBaseName();

            LoadBaseParameters();

            AddPeriodCommand = new RoutedUICommand("AddPeriod", "AddPeriod", typeof(ParametersViewModel));
            ModifyPeriodCommand = new RoutedUICommand("ModifyPeriod", "ModifyPeriod", typeof(ParametersViewModel));
            DeletePeriodCommand = new RoutedUICommand("DeletePeriod", "DeletePeriod", typeof(ParametersViewModel));
            SavePeriodCommand = new RoutedUICommand("SavePeriod", "SavePeriod", typeof(ParametersViewModel));
            CancelPeriodCommand = new RoutedUICommand("CancelPeriod", "CancelPeriod", typeof(ParametersViewModel));
            ModifySemiTrimesterCommand = new RoutedUICommand("ModifySemiTrimester", "ModifySemiTrimester", typeof(ParametersViewModel));
            AddSemiTrimesterPeriodCommand = new RoutedUICommand("AddSemiTrimesterPeriod", "AddSemiTrimesterPeriod", typeof(ParametersViewModel));
            DeleteSemiTrimesterPeriodCommand = new RoutedUICommand("DeleteSemiTrimesterPeriod", "DeleteSemiTrimesterPeriod", typeof(ParametersViewModel));
            SaveSemiTrimesterCommand = new RoutedUICommand("SaveSemiTrimester", "SaveSemiTrimester", typeof(ParametersViewModel));
            CancelSemiTrimesterCommand = new RoutedUICommand("CancelSemiTrimester", "CancelSemiTrimester", typeof(ParametersViewModel));
            AddLevelCommand = new RoutedUICommand("AddLevel", "AddLevel", typeof(ParametersViewModel));
            ModifyLevelCommand = new RoutedUICommand("ModifyLevel", "ModifyLevel", typeof(ParametersViewModel));
            DeleteLevelCommand = new RoutedUICommand("DeleteLevel", "DeleteLevel", typeof(ParametersViewModel));
            UpLevelCommand = new RoutedUICommand("UpLevel", "UpLevel", typeof(ParametersViewModel));
            DownLevelCommand = new RoutedUICommand("DownLevel", "DownLevel", typeof(ParametersViewModel));
            AddLevelSubjectsCommand = new RoutedUICommand("AddLevelSubjects", "AddLevelSubjects", typeof(ParametersViewModel));
            DeleteLevelSubjectCommand = new RoutedUICommand("DeleteLevelSubject", "DeleteLevelSubject", typeof(ParametersViewModel));
            SaveLevelCommand = new RoutedUICommand("SaveLevel", "SaveLevel", typeof(ParametersViewModel));
            CancelLevelCommand = new RoutedUICommand("CancelLevel", "CancelLevel", typeof(ParametersViewModel));
            AddSubjectCommand = new RoutedUICommand("AddSubject", "AddSubject", typeof(ParametersViewModel));
            ModifySubjectCommand = new RoutedUICommand("ModifySubject", "ModifySubject", typeof(ParametersViewModel));
            DeleteSubjectCommand = new RoutedUICommand("DeleteSubject", "DeleteSubject", typeof(ParametersViewModel));
            UpSubjectCommand = new RoutedUICommand("UpSubject", "UpSubject", typeof(ParametersViewModel));
            DownSubjectCommand = new RoutedUICommand("DownSubject", "DownSubject", typeof(ParametersViewModel));
            AddSubjectLevelsCommand = new RoutedUICommand("AddSubjectLevels", "AddSubjectLevels", typeof(ParametersViewModel));
            DeleteSubjectLevelCommand = new RoutedUICommand("DeleteSubjectLevel", "DeleteSubjectLevel", typeof(ParametersViewModel));
            AddSubjectTeachersCommand = new RoutedUICommand("AddSubjectTeachers", "AddSubjectTeachers", typeof(ParametersViewModel));
            DeleteSubjectTeacherCommand = new RoutedUICommand("DeleteSubjectTeacher", "DeleteSubjectTeacher", typeof(ParametersViewModel));
            SaveSubjectCommand = new RoutedUICommand("SaveSubject", "SaveSubject", typeof(ParametersViewModel));
            CancelSubjectCommand = new RoutedUICommand("CancelSubject", "CancelSubject", typeof(ParametersViewModel));
            AddTeacherCommand = new RoutedUICommand("AddTeacher", "AddTeacher", typeof(ParametersViewModel));
            ModifyTeacherCommand = new RoutedUICommand("ModifyTeacher", "ModifyTeacher", typeof(ParametersViewModel));
            DeleteTeacherCommand = new RoutedUICommand("DeleteTeacher", "DeleteTeacher", typeof(ParametersViewModel));
            AddTeacherSubjectsCommand = new RoutedUICommand("AddTeacherSubjects", "AddTeacherSubjects", typeof(ParametersViewModel));
            DeleteTeacherSubjectCommand = new RoutedUICommand("DeleteTeacherSubject", "DeleteTeacherSubject", typeof(ParametersViewModel));
            AddTeacherClassesCommand = new RoutedUICommand("AddTeacherClasses", "AddTeacherClasses", typeof(ParametersViewModel));
            DeleteTeacherClassCommand = new RoutedUICommand("DeleteTeacherClass", "DeleteTeacherClass", typeof(ParametersViewModel));
            SaveTeacherCommand = new RoutedUICommand("SaveTeacher", "SaveTeacher", typeof(ParametersViewModel));
            CancelTeacherCommand = new RoutedUICommand("CancelTeacher", "CancelTeacher", typeof(ParametersViewModel));
            AddClassCommand = new RoutedUICommand("AddClass", "AddClass", typeof(ParametersViewModel));
            ModifyClassCommand = new RoutedUICommand("ModifyClass", "ModifyClass", typeof(ParametersViewModel));
            DeleteClassCommand = new RoutedUICommand("DeleteClass", "DeleteClass", typeof(ParametersViewModel));
            UpClassCommand = new RoutedUICommand("UpClass", "UpClass", typeof(ParametersViewModel));
            DownClassCommand = new RoutedUICommand("DownClass", "DownClass", typeof(ParametersViewModel));
            AddClassTeachersCommand = new RoutedUICommand("AddClassTeachers", "AddClassTeachers", typeof(ParametersViewModel));
            DeleteClassTeacherCommand = new RoutedUICommand("DeleteClassTeacher", "DeleteClassTeacher", typeof(ParametersViewModel));
            AddClassStudentsCommand = new RoutedUICommand("AddClassStudents", "AddClassStudents", typeof(ParametersViewModel));
            DeleteClassStudentCommand = new RoutedUICommand("DeleteClassStudent", "DeleteClassStudent", typeof(ParametersViewModel));
            SaveClassCommand = new RoutedUICommand("SaveClass", "SaveClass", typeof(ParametersViewModel));
            CancelClassCommand = new RoutedUICommand("CancelClass", "CancelClass", typeof(ParametersViewModel));
            AddStudentCommand = new RoutedUICommand("AddStudent", "AddStudent", typeof(ParametersViewModel));
            ModifyStudentCommand = new RoutedUICommand("ModifyStudent", "ModifyStudent", typeof(ParametersViewModel));
            DeleteStudentCommand = new RoutedUICommand("DeleteStudent", "DeleteStudent", typeof(ParametersViewModel));
            SaveStudentCommand = new RoutedUICommand("SaveStudent", "SaveStudent", typeof(ParametersViewModel));
            CancelStudentCommand = new RoutedUICommand("CancelStudent", "CancelStudent", typeof(ParametersViewModel));
            SaveBaseParametersCommand = new RoutedUICommand("SaveBaseParameters", "SaveBaseParameters", typeof(ParametersViewModel));
            CancelBaseParametersCommand = new RoutedUICommand("CancelBaseParameters", "CancelBaseParameters", typeof(ParametersViewModel));
            SaveYearParametersCommand = new RoutedUICommand("SaveYearParameters", "SaveYearParameters", typeof(ParametersViewModel));
            CancelYearParametersCommand = new RoutedUICommand("CancelYearParameters", "CancelYearParameters", typeof(ParametersViewModel));
            PlusColorRCommand = new RoutedUICommand("PlusColorR", "PlusColorR", typeof(ParametersViewModel));
            MinusColorRCommand = new RoutedUICommand("MinusColorR", "MinusColorR", typeof(ParametersViewModel));
            PlusColorGCommand = new RoutedUICommand("PlusColorG", "PlusColorG", typeof(ParametersViewModel));
            MinusColorGCommand = new RoutedUICommand("MinusColorG", "MinusColorG", typeof(ParametersViewModel));
            PlusColorBCommand = new RoutedUICommand("PlusColorB", "PlusColorB", typeof(ParametersViewModel));
            MinusColorBCommand = new RoutedUICommand("MinusColorB", "MinusColorB", typeof(ParametersViewModel));

            Bindings = new CommandBindingCollection()
            {
                new CommandBinding(AddPeriodCommand, AddPeriodExecuted, AddPeriodCanExecute),
                new CommandBinding(ModifyPeriodCommand, ModifyPeriodExecuted, ModifyPeriodCanExecute),
                new CommandBinding(DeletePeriodCommand, DeletePeriodExecuted, DeletePeriodCanExecute),
                new CommandBinding(SavePeriodCommand, SavePeriodExecuted),
                new CommandBinding(CancelPeriodCommand, CancelPeriodExecuted),
                new CommandBinding(ModifySemiTrimesterCommand, ModifySemiTrimesterExecuted, ModifySemiTrimesterCanExecute),
                new CommandBinding(AddSemiTrimesterPeriodCommand, AddSemiTrimesterPeriodExecuted, AddSemiTrimesterPeriodCanExecute),
                new CommandBinding(DeleteSemiTrimesterPeriodCommand, DeleteSemiTrimesterPeriodExecuted, DeleteSemiTrimesterPeriodCanExecute),
                new CommandBinding(SaveSemiTrimesterCommand, SaveSemiTrimesterExecuted),
                new CommandBinding(CancelSemiTrimesterCommand, CancelSemiTrimesterExecuted),
                new CommandBinding(AddLevelCommand, AddLevelExecuted, AddLevelCanExecute),
                new CommandBinding(ModifyLevelCommand, ModifyLevelExecuted, ModifyLevelCanExecute),
                new CommandBinding(DeleteLevelCommand, DeleteLevelExecuted, DeleteLevelCanExecute),
                new CommandBinding(UpLevelCommand, UpLevelExecuted, UpLevelCanExecute),
                new CommandBinding(DownLevelCommand, DownLevelExecuted, DownLevelCanExecute),
                new CommandBinding(AddLevelSubjectsCommand, AddLevelSubjectsExecuted, AddLevelSubjectsCanExecute),
                new CommandBinding(DeleteLevelSubjectCommand, DeleteLevelSubjectExecuted, DeleteLevelSubjectCanExecute),
                new CommandBinding(SaveLevelCommand, SaveLevelExecuted),
                new CommandBinding(CancelLevelCommand, CancelLevelExecuted),
                new CommandBinding(AddSubjectCommand, AddSubjectExecuted, AddSubjectCanExecute),
                new CommandBinding(ModifySubjectCommand, ModifySubjectExecuted, ModifySubjectCanExecute),
                new CommandBinding(DeleteSubjectCommand, DeleteSubjectExecuted, DeleteSubjectCanExecute),
                new CommandBinding(UpSubjectCommand, UpSubjectExecuted, UpSubjectCanExecute),
                new CommandBinding(DownSubjectCommand, DownSubjectExecuted, DownSubjectCanExecute),
                new CommandBinding(AddSubjectLevelsCommand, AddSubjectLevelsExecuted, AddSubjectLevelsCanExecute),
                new CommandBinding(DeleteSubjectLevelCommand, DeleteSubjectLevelExecuted, DeleteSubjectLevelCanExecute),
                new CommandBinding(AddSubjectTeachersCommand, AddSubjectTeachersExecuted, AddSubjectTeachersCanExecute),
                new CommandBinding(DeleteSubjectTeacherCommand, DeleteSubjectTeacherExecuted, DeleteSubjectTeacherCanExecute),
                new CommandBinding(SaveSubjectCommand, SaveSubjectExecuted),
                new CommandBinding(CancelSubjectCommand, CancelSubjectExecuted),
                new CommandBinding(AddTeacherCommand, AddTeacherExecuted, AddTeacherCanExecute),
                new CommandBinding(ModifyTeacherCommand, ModifyTeacherExecuted, ModifyTeacherCanExecute),
                new CommandBinding(DeleteTeacherCommand, DeleteTeacherExecuted, DeleteTeacherCanExecute),
                new CommandBinding(AddTeacherSubjectsCommand, AddTeacherSubjectsExecuted, AddTeacherSubjectsCanExecute),
                new CommandBinding(DeleteTeacherSubjectCommand, DeleteTeacherSubjectExecuted, DeleteTeacherSubjectCanExecute),
                new CommandBinding(AddTeacherClassesCommand, AddTeacherClassesExecuted, AddTeacherClassesCanExecute),
                new CommandBinding(DeleteTeacherClassCommand, DeleteTeacherClassExecuted, DeleteTeacherClassCanExecute),
                new CommandBinding(SaveTeacherCommand, SaveTeacherExecuted),
                new CommandBinding(CancelTeacherCommand, CancelTeacherExecuted),
                new CommandBinding(AddClassCommand, AddClassExecuted, AddClassCanExecute),
                new CommandBinding(ModifyClassCommand, ModifyClassExecuted, ModifyClassCanExecute),
                new CommandBinding(DeleteClassCommand, DeleteClassExecuted, DeleteClassCanExecute),
                new CommandBinding(UpClassCommand, UpClassExecuted, UpClassCanExecute),
                new CommandBinding(DownClassCommand, DownClassExecuted, DownClassCanExecute),
                new CommandBinding(AddClassTeachersCommand, AddClassTeachersExecuted, AddClassTeachersCanExecute),
                new CommandBinding(DeleteClassTeacherCommand, DeleteClassTeacherExecuted, DeleteClassTeacherCanExecute),
                new CommandBinding(AddClassStudentsCommand, AddClassStudentsExecuted, AddClassStudentsCanExecute),
                new CommandBinding(DeleteClassStudentCommand, DeleteClassStudentExecuted, DeleteClassStudentCanExecute),
                new CommandBinding(SaveClassCommand, SaveClassExecuted),
                new CommandBinding(CancelClassCommand, CancelClassExecuted),
                new CommandBinding(AddStudentCommand, AddStudentExecuted, AddStudentCanExecute),
                new CommandBinding(ModifyStudentCommand, ModifyStudentExecuted, ModifyStudentCanExecute),
                new CommandBinding(DeleteStudentCommand, DeleteStudentExecuted, DeleteStudentCanExecute),
                new CommandBinding(SaveStudentCommand, SaveStudentExecuted),
                new CommandBinding(CancelStudentCommand, CancelStudentExecuted),
                new CommandBinding(SaveBaseParametersCommand, SaveBaseParametersExecuted),
                new CommandBinding(CancelBaseParametersCommand, CancelBaseParametersExecuted),
                new CommandBinding(SaveYearParametersCommand, SaveYearParametersExecuted),
                new CommandBinding(CancelYearParametersCommand, CancelYearParametersExecuted),
                new CommandBinding(PlusColorRCommand, PlusColorRExecuted, PlusColorRCanExecute),
                new CommandBinding(MinusColorRCommand, MinusColorRExecuted, MinusColorRCanExecute),
                new CommandBinding(PlusColorGCommand, PlusColorGExecuted, PlusColorGCanExecute),
                new CommandBinding(MinusColorGCommand, MinusColorGExecuted, MinusColorGCanExecute),
                new CommandBinding(PlusColorBCommand, PlusColorBExecuted, PlusColorBCanExecute),
                new CommandBinding(MinusColorBCommand, MinusColorBExecuted, MinusColorBCanExecute),
            };
        }

        public void LoadData(PeriodViewModel period)
        {
            int idPeriod = period.Id;
            LoadData();
            SelectedPeriod = Periods.FirstOrDefault(l => l.Id == idPeriod);
        }

        public void LoadData(SemiTrimesterViewModel semiTrimester)
        {
            int idSemiTrimester = semiTrimester.Id;
            LoadData();
            SelectedSemiTrimester = SemiTrimesters.FirstOrDefault(l => l.Id == idSemiTrimester);
        }

        public void LoadData(LevelViewModel level)
        {
            int idLevel = level.Id;
            LoadData();
            SelectedLevel = Levels.FirstOrDefault(l => l.Id == idLevel);
        }

        public void LoadData(SubjectViewModel subject)
        {
            int idSubject = subject.Id;
            LoadData();
            SelectedSubject = Subjects.FirstOrDefault(s => s.Id == idSubject);
        }

        public void LoadData(TeacherViewModel teacher)
        {
            int idTeacher = teacher.Id;
            LoadData();
            SelectedTeacher = Teachers.FirstOrDefault(t => t.Id == idTeacher);
        }

        public void LoadData(ClassViewModel _class)
        {
            int idClass = _class.Id;
            LoadData();
            SelectedClass = Classes.FirstOrDefault(c => c.Id == idClass);
        }

        public void LoadData(StudentViewModel student)
        {
            int idStudent = student.Id;
            LoadData();
            SelectedStudent = Students.FirstOrDefault(s => s.Id == idStudent);
        }

        public void LoadData()
        {
            LoadPeriods();
            LoadSemiTrimesters();
            LoadLevels();
            LoadSubjects();
            LoadTeachers();
            LoadClasses();
            LoadStudents();

            LoadLevelSubjects();
            LoadSubjectTeachers();
            LoadTeacherClasses();

            LoadYearParameters();
            LoadUtils();
        }

        public void LoadLevelSubjects()
        {
            LevelSubjectModel.ReadLevelSubjects(Levels, Subjects, Year);
        }

        public void LoadSubjectTeachers()
        {
            SubjectTeacherModel.ReadSubjectTeachers(Subjects, Teachers, Year);
        }

        public void LoadTeacherClasses()
        {
            TeacherClassModel.ReadTeacherClasss(Teachers, Classes, Year);
        }

        public void LoadPeriods()
        {
            Periods.Clear();
            if (Year != 0)
            {
                foreach (PeriodViewModel period in PeriodModel.Read(Year))
                {
                    Periods.Add(period);
                }
            }
            ModificationPeriod = null;
            Calendar.LoadCalendarSummaries(Periods);
        }

        public void LoadSemiTrimesters()
        {
            SemiTrimesters.Clear();
            if (Year != 0)
            {
                foreach (SemiTrimesterViewModel semiTrimester in SemiTrimesterModel.Read(Year, Periods))
                {
                    SemiTrimesters.Add(semiTrimester);
                }
            }
            Calendar.LoadCalendarSummaries(SemiTrimesters);
        }

        public void LoadLevels()
        {
            Levels.Clear();
            if (Year != 0)
            {
                foreach (LevelViewModel level in LevelModel.Read(Year))
                {
                    Levels.Add(level);
                }
            }
            ModificationLevel = null;
        }

        public void LoadSubjects()
        {
            Subjects.Clear();
            if (Year != 0)
            {
                IEnumerable<SubjectViewModel> subjects = SubjectModel.ReadParents(Year);
                int order = 0;
                foreach (SubjectViewModel subject in SubjectModel.ReadChildren(Year, subjects))
                {
                    subject.Order = order;
                    Subjects.Add(subject);
                    order++;
                }
            }
            ModificationSubject = null;
        }

        public void LoadTeachers()
        {
            Teachers.Clear();
            if (Year != 0)
            {
                foreach (TeacherViewModel teacher in TeacherModel.Read(Year))
                {
                    Teachers.Add(teacher);
                }
            }
            ModificationTeacher = null;
        }

        public void LoadClasses()
        {
            Classes.Clear();
            if (Year != 0)
            {
                foreach (ClassViewModel _class in ClassModel.Read(Year, MainViewModel.Instance.Parameters.Teachers, MainViewModel.Instance.Parameters.Levels))
                {
                    Classes.Add(_class);
                }
            }
            ModificationClass = null;
        }

        public void LoadStudents()
        {
            Students.Clear();
            if (Year != 0)
            {
                foreach (StudentViewModel Student in StudentModel.Read(Year, MainViewModel.Instance.Parameters.Classes))
                {
                    Students.Add(Student);
                }
            }
            ModificationStudent = null;
        }

        private void LoadBaseParameters()
        {
            BaseParameters = BaseParametersModel.Read();
            if (BaseParameters == null)
            {
                BaseParameters = new BaseParametersViewModel() { Name = GetBaseName() };
                BaseParametersModel.Create(BaseParameters);
            }
        }

        public void LoadYearParameters()
        {
            YearParameters = YearParametersModel.Read(Year);
        }

        public void LoadUtils()
        {
            Utils.LoadData();
        }

        private string GetBaseName()
        {
            string cut = Settings.Settings.Instance.SQLConnection?.Split(';')?.FirstOrDefault(s => s.StartsWith("Initial Catalog="));
            if (cut != null)
            {
                return cut.Substring("Initial Catalog=".Length);
            }
            return "";
        }

        private void GenerateSemiTrimester()
        {
            SemiTrimesters.Clear();
            SemiTrimesterViewModel semiTrimester = new SemiTrimesterViewModel() { Year = Year };
            foreach (IGrouping<int, PeriodViewModel> periodGroup in Periods.GroupBy(p => p.Trimester))
            {
                foreach (PeriodViewModel period in periodGroup)
                {
                    if (semiTrimester.Period1 == null)
                    {
                        semiTrimester.Period1 = period;
                    }
                    else
                    {
                        semiTrimester.Period2 = period;
                        SemiTrimesters.Add(semiTrimester);
                        semiTrimester = new SemiTrimesterViewModel() { Year = Year };
                    }
                }
                if (semiTrimester.Period1 != null)
                {
                    SemiTrimesters.Add(semiTrimester);
                    semiTrimester = new SemiTrimesterViewModel() { Year = Year };
                }
            }
            SemiTrimesterModel.Save(SemiTrimesters);
            LoadSemiTrimesters();
        }
    }
}

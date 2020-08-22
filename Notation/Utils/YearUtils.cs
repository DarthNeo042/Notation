using Notation.Models;
using Notation.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Notation.Utils
{
    public static class YearUtils
    {
        public static void CreateYear(int year)
        {
            YearModel.Create(year);

            MainViewModel mainViewModel = new MainViewModel()
            {
                SelectedYear = year
            };

            CreatePeriods(mainViewModel, year);
            CreateSemiTrimesters(mainViewModel, year);
            CreateLevels(mainViewModel, year);
            CreateSubjects(mainViewModel, year);
            CreateTeachers(mainViewModel, year);
            CreateClasses(mainViewModel, year);
            CreateStudents(mainViewModel, year);

            CreateLevelSubjects(mainViewModel, year);
            CreateSubjectTeachers(mainViewModel, year);
            CreateTeacherClasses(mainViewModel, year);

            CreateYearParameters(mainViewModel, year);

            MessageBox.Show("Création de la nouvelle année réussie.", "Réussite", MessageBoxButton.OK, MessageBoxImage.Information);

            MainViewModel.Instance.LoadYears();
            MainViewModel.Instance.SelectedYear = year;
        }

        private static void CreatePeriods(MainViewModel mainViewModel, int year)
        {
            foreach (PeriodViewModel period in MainViewModel.Instance.Parameters.Periods)
            {
                PeriodModel.Save(new PeriodViewModel()
                {
                    FromDate = period.FromDate.AddYears(1),
                    Number = period.Number,
                    Order = period.Order,
                    ToDate = period.ToDate.AddYears(1),
                    Trimester = period.Trimester,
                    Year = year,
                });
            }
            mainViewModel.Parameters.LoadPeriods();
        }

        private static void CreateSemiTrimesters(MainViewModel mainViewModel, int year)
        {
            List<SemiTrimesterViewModel> semiTrimesters = new List<SemiTrimesterViewModel>();
            foreach (SemiTrimesterViewModel semiTrimester in MainViewModel.Instance.Parameters.SemiTrimesters)
            {
                semiTrimesters.Add(new SemiTrimesterViewModel()
                {
                    Name = semiTrimester.Name,
                    Period1 = mainViewModel.Parameters.Periods.FirstOrDefault(p => p.Number == semiTrimester.Period1.Number),
                    Period2 = mainViewModel.Parameters.Periods.FirstOrDefault(p => p.Number == semiTrimester.Period2.Number),
                    Year = year,
                });
            }
            SemiTrimesterModel.Save(semiTrimesters);
            mainViewModel.Parameters.LoadSemiTrimesters();
        }

        private static void CreateLevels(MainViewModel mainViewModel, int year)
        {
            foreach (LevelViewModel level in MainViewModel.Instance.Parameters.Levels)
            {
                LevelModel.Save(new LevelViewModel()
                {
                    Name = level.Name,
                    Order = level.Order,
                    Year = year,
                });
            }
            mainViewModel.Parameters.LoadLevels();
        }

        private static void CreateSubjects(MainViewModel mainViewModel, int year)
        {
            foreach (SubjectViewModel subject in MainViewModel.Instance.Parameters.Subjects.Where(s => s.ParentSubject == null))
            {
                SubjectModel.Save(new SubjectViewModel()
                {
                    Coefficient = subject.Coefficient,
                    Name = subject.Name,
                    Option = subject.Option,
                    Order = subject.Order,
                    Year = year,
                });
            }
            mainViewModel.Parameters.LoadSubjects();
            foreach (SubjectViewModel subject in MainViewModel.Instance.Parameters.Subjects.Where(s => s.ParentSubject != null))
            {
                SubjectModel.Save(new SubjectViewModel()
                {
                    Coefficient = subject.Coefficient,
                    Name = subject.Name,
                    Option = subject.Option,
                    Order = subject.Order,
                    ParentSubject = mainViewModel.Parameters.Subjects.FirstOrDefault(s => s.Name == subject.ParentSubject.Name),
                    Year = year,
                });
            }
            mainViewModel.Parameters.LoadSubjects();
        }

        private static void CreateTeachers(MainViewModel mainViewModel, int year)
        {
            foreach (TeacherViewModel teacher in MainViewModel.Instance.Parameters.Teachers)
            {
                TeacherModel.Save(new TeacherViewModel()
                {
                    FirstName = teacher.FirstName,
                    LastName = teacher.LastName,
                    Login = teacher.Login,
                    Order = teacher.Order,
                    Password = teacher.Password,
                    Title = teacher.Title,
                    Year = year,
                });
            }
            mainViewModel.Parameters.LoadTeachers();
        }

        private static void CreateClasses(MainViewModel mainViewModel, int year)
        {
            foreach (ClassViewModel _class in MainViewModel.Instance.Parameters.Classes)
            {
                ClassModel.Save(new ClassViewModel()
                {
                    Level = _class.Level != null ? mainViewModel.Parameters.Levels.FirstOrDefault(l => l.Name == _class.Level.Name) : null,
                    MainTeacher = _class.MainTeacher != null ? mainViewModel.Parameters.Teachers.FirstOrDefault(t => t.FirstName == _class.MainTeacher.FirstName
                        && t.LastName == _class.MainTeacher.LastName) : null,
                    Name = _class.Name,
                    Order = _class.Order,
                    Year = year,
                });
            }
            mainViewModel.Parameters.LoadClasses();
        }

        private static void CreateSubjectTeachers(MainViewModel mainViewModel, int year)
        {
            foreach (SubjectViewModel _subject in MainViewModel.Instance.Parameters.Subjects)
            {
                SubjectViewModel subject = mainViewModel.Parameters.Subjects.FirstOrDefault(s => s.Name == _subject.Name);
                foreach (TeacherViewModel teacher in _subject.Teachers)
                {
                    subject.Teachers.Add(mainViewModel.Parameters.Teachers.FirstOrDefault(t => t.FirstName == teacher.FirstName && t.LastName == teacher.LastName));
                }
                SubjectTeacherModel.SaveSubjectTeachers(subject);
            }
        }

        private static void CreateLevelSubjects(MainViewModel mainViewModel, int year)
        {
            foreach (LevelViewModel _level in MainViewModel.Instance.Parameters.Levels)
            {
                LevelViewModel level = mainViewModel.Parameters.Levels.FirstOrDefault(l => l.Name == _level.Name);
                foreach (SubjectViewModel subject in _level.Subjects)
                {
                    level.Subjects.Add(mainViewModel.Parameters.Subjects.FirstOrDefault(s => s.Name == subject.Name));
                }
                LevelSubjectModel.SaveLevelSubjects(level);
            }
        }

        private static void CreateTeacherClasses(MainViewModel mainViewModel, int year)
        {
            foreach (TeacherViewModel _teacher in MainViewModel.Instance.Parameters.Teachers)
            {
                TeacherViewModel teacher = mainViewModel.Parameters.Teachers.FirstOrDefault(t => t.LastName == _teacher.LastName && t.FirstName == _teacher.FirstName);
                foreach (ClassViewModel _class in _teacher.Classes)
                {
                    teacher.Classes.Add(mainViewModel.Parameters.Classes.FirstOrDefault(c => c.Name == _class.Name));
                }
                TeacherClassModel.SaveTeacherClasses(teacher);
            }
        }

        private static void CreateStudents(MainViewModel mainViewModel, int year)
        {
            foreach (StudentViewModel student in MainViewModel.Instance.Parameters.Students)
            {
                StudentModel.Save(new StudentViewModel()
                {
                    BirthDate = student.BirthDate,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    Order = student.Order,
                    Year = year,
                });
            }
            mainViewModel.Parameters.LoadStudents();
        }

        private static void CreateYearParameters(MainViewModel mainViewModel, int year)
        {
            YearParametersModel.Create(new YearParametersViewModel()
            {
                DivisionPrefect = MainViewModel.Instance.Parameters.YearParameters != null ? MainViewModel.Instance.Parameters.YearParameters.DivisionPrefect : "",
                Year = year,
            });
            mainViewModel.Parameters.LoadYearParameters();
        }

        public static void DeleteYear(int year)
        {
            TrimesterCommentModel.DeleteAll(year);
            TrimesterSubjectCommentModel.DeleteAll(year);
            SemiTrimesterCommentModel.DeleteAll(year);
            PeriodCommentModel.DeleteAll(year);
            MarkModel.DeleteAll(year);
            TeacherClassModel.DeleteAll(year);
            LevelSubjectModel.DeleteAll(year);
            SubjectTeacherModel.DeleteAll(year);
            SemiTrimesterModel.DeleteAll(year);
            PeriodModel.DeleteAll(year);
            StudentModel.DeleteAll(year);
            ClassModel.DeleteAll(year);
            LevelModel.DeleteAll(year);
            SubjectModel.DeleteAll(year);
            TeacherModel.DeleteAll(year);
            YearParametersModel.DeleteAll(year);
            YearModel.Delete(year);

            MessageBox.Show("Suppresion de l'année réussie.", "Réussite", MessageBoxButton.OK, MessageBoxImage.Information);

            MainViewModel.Instance.LoadYears();
            MainViewModel.Instance.SelectedYear = year - 1;
        }
    }
}

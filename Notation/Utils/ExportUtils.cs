﻿using Notation.Models;
using Notation.Settings;
using Notation.ViewModels;
using OfficeOpenXml;
using OfficeOpenXml.DataValidation;
using OfficeOpenXml.DataValidation.Contracts;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using static Notation.Views.MainWindow;

namespace Notation.Utils
{
    static class ExportUtils
    {
        private const string ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public static void ExportPeriodModels(PeriodViewModel period, UpdatePeriodModelsDelegate _updatePeriodModels)
        {
            try
            {
                string directory = FileUtils.SelectDirectory(Settings.Settings.Instance.LastSelectedDirectoryPeriodModels, "LastSelectedDirectoryPeriodModels");
                if (!string.IsNullOrEmpty(directory))
                {
                    MainViewModel.Instance.Models.PeriodModels.Clear();

                    ExportPeriodCommentsModels(period, directory, _updatePeriodModels);
                    ExportPeriodMarksModels(period, directory, _updatePeriodModels);

                    MainViewModel.Instance.Models.PeriodModelsPath = directory;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private static void ExportPeriodCommentsModels(PeriodViewModel period, string directory, UpdatePeriodModelsDelegate _updatePeriodModels)
        {
            string filename = Path.Combine(directory, $"Appréciations générales période {period.Number}.xlsx");
            File.Delete(filename);

            ExcelPackage excel = new ExcelPackage(new FileInfo(filename));

            ExcelWorksheet workSheet = excel.Workbook.Worksheets.Add("Feuil1");
            workSheet.Cells[1, 1].Value = "type";
            workSheet.Cells[1, 2].Value = "APP_GEN_PER";
            workSheet.Cells[2, 1].Value = "année";
            workSheet.Cells[2, 2].Value = period.Year;
            workSheet.Cells[3, 1].Value = "période";
            workSheet.Cells[3, 2].Value = period.Number;
            workSheet.Cells[4, 1].Value = "élève";
            workSheet.Cells[4, 2].Value = "compte rendu des études";
            workSheet.Cells[4, 3].Value = "compte rendu de discipline";

            int row = 5;
            foreach (ClassViewModel _class in MainViewModel.Instance.Parameters.Classes.OrderBy(c => c.Order))
            {
                foreach (StudentViewModel student in _class.Students.OrderBy(s => s.LastName).ThenBy(s => s.FirstName))
                {
                    workSheet.Cells[row++, 1].Value = $"{student.LastName} {student.FirstName}";
                }
            }

            workSheet.Column(1).AutoFit();
            workSheet.Column(2).Width = 50;
            workSheet.Column(2).Style.WrapText = true;
            workSheet.Column(3).Width = 50;
            workSheet.Column(3).Style.WrapText = true;

            excel.Save();

            MainViewModel.Instance.Models.PeriodModels.Add(Path.GetFileName(filename));

            _updatePeriodModels(50);
        }

        private static void ExportPeriodMarksModels(PeriodViewModel period, string directory, UpdatePeriodModelsDelegate _updatePeriodModels)
        {
            int subjectCount = 0;
            int classCount = 0;
            foreach (SubjectViewModel mainSubject in MainViewModel.Instance.Parameters.Subjects)
            {
                if (mainSubject.ChildrenSubjects.Any())
                {
                    classCount = 0;
                    foreach (ClassViewModel _class in MainViewModel.Instance.Parameters.Classes)
                    {
                        foreach (TeacherViewModel teacher in ModelUtils.GetTeachersFromClassAndSubject(_class, mainSubject, period))
                        {
                            string filename = Path.Combine(directory, string.Format("Notes période {0} - {1} - {2} - {3}.xlsx", period.Number, mainSubject.Name, _class.Name,
                                string.IsNullOrEmpty(teacher.FirstName) ? teacher.LastName : $"{teacher.LastName} {teacher.FirstName}"));
                            File.Delete(filename);

                            ExcelPackage excel = new ExcelPackage(new FileInfo(filename));

                            ExcelWorksheet workSheet = excel.Workbook.Worksheets.Add("Feuil1");
                            workSheet.Cells[1, 1].Value = "type";
                            workSheet.Cells[1, 2].Value = "MRK_PER";
                            workSheet.Cells[2, 1].Value = "année";
                            workSheet.Cells[2, 2].Value = period.Year;
                            workSheet.Cells[3, 1].Value = "période";
                            workSheet.Cells[3, 2].Value = period.Number;
                            workSheet.Cells[4, 1].Value = "professeur";
                            workSheet.Cells[4, 2].Value = string.IsNullOrEmpty(teacher.FirstName) ? teacher.LastName : $"{teacher.LastName} {teacher.FirstName}";
                            workSheet.Cells[5, 1].Value = "matière";
                            workSheet.Cells[6, 1].Value = "élève / coefficient";

                            int column = 0;
                            foreach (SubjectViewModel subject in MainViewModel.Instance.Parameters.Subjects.Where(s => s.ParentSubject != null && s.ParentSubject.Id == mainSubject.Id).OrderBy(s => s.Order))
                            {
                                workSheet.Cells[5, 2 + column].Value = subject.Name;

                                int maxCoeff1 = int.MinValue;
                                int maxCoeff2 = int.MinValue;
                                int maxCoeff4 = int.MinValue;

                                Dictionary<int, List<MarkModel>> marksMap = MarkModel.Read(MainViewModel.Instance.Parameters.Subjects.Select(s => s.Id),
                                    _class.Students.Select(s => s.Id), teacher.Id, _class.Id, period.Id, period.Year);

                                foreach (StudentViewModel student in _class.Students)
                                {
                                    IEnumerable<MarkModel> marks = marksMap[student.Id];
                                    int coeff1 = marks.Count(m => m.Coefficient == 1 && m.IdSubject == subject.Id);
                                    int coeff2 = marks.Count(m => m.Coefficient == 2 && m.IdSubject == subject.Id);
                                    int coeff4 = marks.Count(m => m.Coefficient == 4 && m.IdSubject == subject.Id);
                                    if (coeff1 > maxCoeff1)
                                    {
                                        maxCoeff1 = coeff1;
                                    }
                                    if (coeff2 > maxCoeff2)
                                    {
                                        maxCoeff2 = coeff2;
                                    }
                                    if (coeff4 > maxCoeff4)
                                    {
                                        maxCoeff4 = coeff4;
                                    }
                                }

                                for (int i = 0; i < maxCoeff1; i++)
                                {
                                    workSheet.Cells[6, 2 + i + column].Value = 1;
                                }
                                for (int i = 0; i < maxCoeff2; i++)
                                {
                                    workSheet.Cells[6, 2 + maxCoeff1 + i + column].Value = 2;
                                }
                                for (int i = 0; i < maxCoeff4; i++)
                                {
                                    workSheet.Cells[6, 2 + maxCoeff1 + maxCoeff2 + i + column].Value = 4;
                                }

                                int row = 7;
                                foreach (StudentViewModel student in _class.Students)
                                {
                                    workSheet.Cells[row, 1].Value = $"{student.LastName} {student.FirstName}";
                                    IEnumerable<MarkModel> marks = marksMap[student.Id];
                                    int i = 0;
                                    foreach (MarkModel mark in marks.Where(m => m.Coefficient == 1 && m.IdSubject == subject.Id))
                                    {
                                        workSheet.Cells[row, 2 + i + column].Value = mark.Mark;
                                        i++;
                                    }
                                    i = 0;
                                    foreach (MarkModel mark in marks.Where(m => m.Coefficient == 2 && m.IdSubject == subject.Id))
                                    {
                                        workSheet.Cells[row, 2 + maxCoeff1 + i + column].Value = mark.Mark;
                                        i++;
                                    }
                                    i = 0;
                                    foreach (MarkModel mark in marks.Where(m => m.Coefficient == 4 && m.IdSubject == subject.Id))
                                    {
                                        workSheet.Cells[row, 2 + maxCoeff1 + maxCoeff2 + i + column].Value = mark.Mark;
                                        i++;
                                    }
                                    row++;
                                }
                                if (maxCoeff1 + maxCoeff2 + maxCoeff4 > 0)
                                {
                                    column += maxCoeff1 + maxCoeff2 + maxCoeff4;
                                }
                                else
                                {
                                    workSheet.Cells[6, 2 + column].Value = 1;
                                    column++;
                                }
                            }

                            workSheet.Cells.AutoFitColumns();
                            excel.Save();

                            MainViewModel.Instance.Models.PeriodModels.Add(Path.GetFileName(filename));
                        }
                        classCount++;
                        _updatePeriodModels(50 + (subjectCount * 750 / MainViewModel.Instance.Parameters.Subjects.Count)
                            + (750 / MainViewModel.Instance.Parameters.Subjects.Count * classCount / MainViewModel.Instance.Parameters.Classes.Count));
                    }
                }
                else
                {
                    classCount = 0;
                    foreach (ClassViewModel _class in MainViewModel.Instance.Parameters.Classes)
                    {
                        foreach (TeacherViewModel teacher in ModelUtils.GetTeachersFromClassAndSubject(_class, mainSubject.ParentSubject ?? mainSubject, period))
                        {
                            string filename = Path.Combine(directory, string.Format("Notes période {0} - {1} - {2} - {3}.xlsx", period.Number, mainSubject.Name, _class.Name,
                                string.IsNullOrEmpty(teacher.FirstName) ? teacher.LastName : $"{teacher.LastName} {teacher.FirstName}"));
                            File.Delete(filename);

                            ExcelPackage excel = new ExcelPackage(new FileInfo(filename));

                            int maxCoeff1 = int.MinValue;
                            int maxCoeff2 = int.MinValue;
                            int maxCoeff4 = int.MinValue;

                            Dictionary<int, List<MarkModel>> marksMap = MarkModel.Read(MainViewModel.Instance.Parameters.Subjects.Select(s => s.Id),
                                _class.Students.Select(s => s.Id), teacher.Id, _class.Id, period.Id, period.Year);

                            foreach (StudentViewModel student in _class.Students)
                            {
                                IEnumerable<MarkModel> marks = marksMap[student.Id];
                                int coeff1 = marks.Count(m => m.Coefficient == 1 && m.IdSubject == mainSubject.Id);
                                int coeff2 = marks.Count(m => m.Coefficient == 2 && m.IdSubject == mainSubject.Id);
                                int coeff4 = marks.Count(m => m.Coefficient == 4 && m.IdSubject == mainSubject.Id);
                                if (coeff1 > maxCoeff1)
                                {
                                    maxCoeff1 = coeff1;
                                }
                                if (coeff2 > maxCoeff2)
                                {
                                    maxCoeff2 = coeff2;
                                }
                                if (coeff4 > maxCoeff4)
                                {
                                    maxCoeff4 = coeff4;
                                }
                            }

                            ExcelWorksheet workSheet = excel.Workbook.Worksheets.Add("Feuil1");
                            workSheet.Cells[1, 1].Value = "type";
                            workSheet.Cells[1, 2].Value = "MRK_PER";
                            workSheet.Cells[2, 1].Value = "année";
                            workSheet.Cells[2, 2].Value = period.Year;
                            workSheet.Cells[3, 1].Value = "période";
                            workSheet.Cells[3, 2].Value = period.Number;
                            workSheet.Cells[4, 1].Value = "professeur";
                            workSheet.Cells[4, 2].Value = string.IsNullOrEmpty(teacher.FirstName) ? teacher.LastName : $"{teacher.LastName} {teacher.FirstName}";
                            workSheet.Cells[5, 1].Value = "matière";
                            workSheet.Cells[5, 2].Value = mainSubject.Name;
                            workSheet.Cells[6, 1].Value = "élève / coefficient";

                            for (int i = 0; i < maxCoeff1; i++)
                            {
                                workSheet.Cells[6, 2 + i].Value = 1;
                            }
                            for (int i = 0; i < maxCoeff2; i++)
                            {
                                workSheet.Cells[6, 2 + maxCoeff1 + i].Value = 2;
                            }
                            for (int i = 0; i < maxCoeff4; i++)
                            {
                                workSheet.Cells[6, 2 + maxCoeff1 + maxCoeff2 + i].Value = 4;
                            }

                            int row = 7;
                            foreach (StudentViewModel student in _class.Students)
                            {
                                workSheet.Cells[row, 1].Value = $"{student.LastName} {student.FirstName}";
                                IEnumerable<MarkModel> marks = marksMap[student.Id];
                                int i = 0;
                                foreach (MarkModel mark in marks.Where(m => m.Coefficient == 1 && m.IdSubject == mainSubject.Id))
                                {
                                    workSheet.Cells[row, 2 + i].Value = mark.Mark;
                                    i++;
                                }
                                i = 0;
                                foreach (MarkModel mark in marks.Where(m => m.Coefficient == 2 && m.IdSubject == mainSubject.Id))
                                {
                                    workSheet.Cells[row, 2 + maxCoeff1 + i].Value = mark.Mark;
                                    i++;
                                }
                                i = 0;
                                foreach (MarkModel mark in marks.Where(m => m.Coefficient == 4 && m.IdSubject == mainSubject.Id))
                                {
                                    workSheet.Cells[row, 2 + maxCoeff1 + maxCoeff2 + i].Value = mark.Mark;
                                    i++;
                                }
                                row++;
                            }

                            workSheet.Cells.AutoFitColumns();

                            excel.Save();
                            MainViewModel.Instance.Models.PeriodModels.Add(Path.GetFileName(filename));
                        }
                    }
                    classCount++;
                    _updatePeriodModels(50 + (subjectCount * 750 / MainViewModel.Instance.Parameters.Subjects.Count)
                        + (750 / MainViewModel.Instance.Parameters.Subjects.Count * classCount / MainViewModel.Instance.Parameters.Classes.Count));
                }
                subjectCount++;
                _updatePeriodModels(50 + (subjectCount * 750 / MainViewModel.Instance.Parameters.Subjects.Count));
            }
            classCount = 0;
            foreach (ClassViewModel _class in MainViewModel.Instance.Parameters.Classes)
            {
                foreach (TeacherViewModel teacher in MainViewModel.Instance.Parameters.Teachers)
                {
                    string filename = Path.Combine(directory, string.Format("Notes période {0} - toutes les matières - {1} - {2}.xlsx", period.Number, _class.Name,
                        string.IsNullOrEmpty(teacher.FirstName) ? teacher.LastName : $"{teacher.LastName} {teacher.FirstName}"));
                    File.Delete(filename);

                    ExcelPackage excel = new ExcelPackage(new FileInfo(filename));

                    ExcelWorksheet workSheet = excel.Workbook.Worksheets.Add("Feuil1");
                    workSheet.Cells[1, 1].Value = "type";
                    workSheet.Cells[1, 2].Value = "MRK_PER";
                    workSheet.Cells[2, 1].Value = "année";
                    workSheet.Cells[2, 2].Value = period.Year;
                    workSheet.Cells[3, 1].Value = "période";
                    workSheet.Cells[3, 2].Value = period.Number;
                    workSheet.Cells[4, 1].Value = "professeur";
                    workSheet.Cells[4, 2].Value = string.IsNullOrEmpty(teacher.FirstName) ? teacher.LastName : $"{teacher.LastName} {teacher.FirstName}";
                    workSheet.Cells[5, 1].Value = "matière";
                    workSheet.Cells[6, 1].Value = "élève / coefficient";

                    int column = 0;
                    List<SubjectViewModel> subjects = new List<SubjectViewModel>();
                    foreach (SubjectViewModel subject in teacher.Subjects.OrderBy(s => s.Order))
                    {
                        if (subject.ChildrenSubjects.Any())
                        {
                            subjects.AddRange(subject.ChildrenSubjects.OrderBy(s => s.Order));
                        }
                        else
                        {
                            subjects.Add(subject);
                        }
                    }

                    foreach (SubjectViewModel subject in subjects)
                    {
                        workSheet.Cells[5, 2 + column].Value = subject.Name;

                        int maxCoeff1 = int.MinValue;
                        int maxCoeff2 = int.MinValue;
                        int maxCoeff4 = int.MinValue;

                        Dictionary<int, List<MarkModel>> marksMap = MarkModel.Read(MainViewModel.Instance.Parameters.Subjects.Select(s => s.Id),
                            _class.Students.Select(s => s.Id), teacher.Id, _class.Id, period.Id, period.Year);

                        foreach (StudentViewModel student in _class.Students)
                        {
                            IEnumerable<MarkModel> marks = marksMap[student.Id];
                            int coeff1 = marks.Count(m => m.Coefficient == 1 && m.IdSubject == subject.Id);
                            int coeff2 = marks.Count(m => m.Coefficient == 2 && m.IdSubject == subject.Id);
                            int coeff4 = marks.Count(m => m.Coefficient == 4 && m.IdSubject == subject.Id);
                            if (coeff1 > maxCoeff1)
                            {
                                maxCoeff1 = coeff1;
                            }
                            if (coeff2 > maxCoeff2)
                            {
                                maxCoeff2 = coeff2;
                            }
                            if (coeff4 > maxCoeff4)
                            {
                                maxCoeff4 = coeff4;
                            }
                        }

                        for (int i = 0; i < maxCoeff1; i++)
                        {
                            workSheet.Cells[6, 2 + i + column].Value = 1;
                        }
                        for (int i = 0; i < maxCoeff2; i++)
                        {
                            workSheet.Cells[6, 2 + maxCoeff1 + i + column].Value = 2;
                        }
                        for (int i = 0; i < maxCoeff4; i++)
                        {
                            workSheet.Cells[6, 2 + maxCoeff1 + maxCoeff2 + i + column].Value = 4;
                        }

                        int row = 7;
                        foreach (StudentViewModel student in _class.Students)
                        {
                            workSheet.Cells[row, 1].Value = $"{student.LastName} {student.FirstName}";
                            IEnumerable<MarkModel> marks = marksMap[student.Id];
                            int i = 0;
                            foreach (MarkModel mark in marks.Where(m => m.Coefficient == 1 && m.IdSubject == subject.Id))
                            {
                                workSheet.Cells[row, 2 + i + column].Value = mark.Mark;
                                i++;
                            }
                            i = 0;
                            foreach (MarkModel mark in marks.Where(m => m.Coefficient == 2 && m.IdSubject == subject.Id))
                            {
                                workSheet.Cells[row, 2 + maxCoeff1 + i + column].Value = mark.Mark;
                                i++;
                            }
                            i = 0;
                            foreach (MarkModel mark in marks.Where(m => m.Coefficient == 4 && m.IdSubject == subject.Id))
                            {
                                workSheet.Cells[row, 2 + maxCoeff1 + maxCoeff2 + i + column].Value = mark.Mark;
                                i++;
                            }
                            row++;
                        }
                        if (maxCoeff1 + maxCoeff2 + maxCoeff4 > 0)
                        {
                            column += (maxCoeff1 + maxCoeff2 + maxCoeff4);
                        }
                        else
                        {
                            workSheet.Cells[6, 2 + column].Value = 1;
                            column++;
                        }
                    }

                    workSheet.Cells.AutoFitColumns();
                    excel.Save();
                    MainViewModel.Instance.Models.PeriodModels.Add(Path.GetFileName(filename));
                }
                classCount++;
                _updatePeriodModels(800 + (classCount * 200 / MainViewModel.Instance.Parameters.Classes.Count));
            }
        }

        private static void ExportTrimesterGeneralCommentsModels(int trimester, string directory, UpdateTrimesterModelsDelegate _updateTrimesterModelsDispatch)
        {
            int classCount = 0;
            foreach (ClassViewModel _class in MainViewModel.Instance.Parameters.Classes)
            {
                string filename = Path.Combine(directory, $"Appréciations générales trimestre {trimester} - {_class.Name}.xlsx");
                File.Delete(filename);

                ExcelPackage excel = new ExcelPackage(new FileInfo(filename));

                ExcelWorksheet workSheet = excel.Workbook.Worksheets.Add("Feuil1");
                workSheet.Cells[1, 1].Value = "type";
                workSheet.Cells[1, 2].Value = "APP_GEN_TRM";
                workSheet.Cells[2, 1].Value = "année";
                workSheet.Cells[2, 2].Value = _class.Year;
                workSheet.Cells[3, 1].Value = "trimestre";
                workSheet.Cells[3, 2].Value = trimester;
                workSheet.Cells[4, 1].Value = "élève";
                workSheet.Cells[4, 2].Value = "appréciation d'études";
                workSheet.Cells[4, 3].Value = "appréciation du préfet de division";

                int row = 5;
                foreach (StudentViewModel student in _class.Students)
                {
                    workSheet.Cells[row, 1].Value = $"{student.LastName} {student.FirstName}";
                    TrimesterCommentModel trimesterComment = TrimesterCommentModel.Read(trimester, student);
                    if (trimesterComment != null)
                    {
                        workSheet.Cells[row, 2].Value = trimesterComment.MainTeacherComment;
                        workSheet.Cells[row, 3].Value = trimesterComment.DivisionPrefectComment;
                    }
                    row++;
                }

                workSheet.Column(1).AutoFit();
                workSheet.Column(2).Width = 50;
                workSheet.Column(2).Style.WrapText = true;
                workSheet.Column(3).Width = 50;
                workSheet.Column(3).Style.WrapText = true;

                excel.Save();

                MainViewModel.Instance.Models.TrimesterModels.Add(Path.GetFileName(filename));

                classCount++;
                _updateTrimesterModelsDispatch(900 + classCount * 100 / MainViewModel.Instance.Parameters.Classes.Count);
            }
        }

        private static void ExportTrimesterSubjectCommentsModels(int trimester, string directory, UpdateTrimesterModelsDelegate _updateTrimesterModelsDispatch)
        {
            PeriodViewModel period = MainViewModel.Instance.Models.Periods.OrderBy(p => p.Number).FirstOrDefault(p => p.Trimester == trimester);

            int subjectCount = 0;
            foreach (SubjectViewModel subject in MainViewModel.Instance.Parameters.Subjects.Where(s => s.ParentSubject == null))
            {
                foreach (ClassViewModel _class in MainViewModel.Instance.Parameters.Classes)
                {
                    foreach (TeacherViewModel teacher in ModelUtils.GetTeachersFromClassAndSubject(_class, subject, period))
                    {
                        string filename = Path.Combine(directory, string.Format("Appréciations trimestre {0} - {1} - {2} - {3}.xlsx",
                            trimester, string.IsNullOrEmpty(teacher.FirstName) ? teacher.LastName : $"{teacher.LastName} {teacher.FirstName}", subject.Name, _class.Name));
                        File.Delete(filename);

                        ExcelPackage excel = new ExcelPackage(new FileInfo(filename));

                        ExcelWorksheet workSheet = excel.Workbook.Worksheets.Add("Feuil1");
                        workSheet.Cells[1, 1].Value = "type";
                        workSheet.Cells[1, 2].Value = "APP_MAT_TRM";
                        workSheet.Cells[2, 1].Value = "année";
                        workSheet.Cells[2, 2].Value = subject.Year;
                        workSheet.Cells[3, 1].Value = "trimestre";
                        workSheet.Cells[3, 2].Value = trimester;
                        workSheet.Cells[4, 1].Value = "professeur";
                        workSheet.Cells[4, 2].Value = string.IsNullOrEmpty(teacher.FirstName) ? teacher.LastName : $"{teacher.LastName} {teacher.FirstName}";
                        workSheet.Cells[5, 1].Value = "élève / matière";
                        workSheet.Cells[5, 2].Value = subject.Name;

                        int row = 6;
                        foreach (StudentViewModel student in _class.Students)
                        {
                            workSheet.Cells[row, 1].Value = $"{student.LastName} {student.FirstName}";
                            TrimesterSubjectCommentModel trimesterSubjectComment = TrimesterSubjectCommentModel.Read(trimester, student, subject);
                            if (trimesterSubjectComment != null)
                            {
                                workSheet.Cells[row, 2].Value = trimesterSubjectComment.Comment;
                            }
                            row++;
                        }

                        IExcelDataValidationInt _textValidation = workSheet.Cells.DataValidation.AddTextLengthDataValidation();
                        _textValidation.ShowErrorMessage = true;
                        _textValidation.ErrorStyle = ExcelDataValidationWarningStyle.warning;
                        _textValidation.ErrorTitle = "Commentaire trop long";
                        _textValidation.Error = "Le commentaire ne doit pas dépasser 120 caractères.";
                        _textValidation.Formula.Value = 0;
                        _textValidation.Formula2.Value = 120;

                        workSheet.Column(1).AutoFit();
                        workSheet.Column(2).Width = 100;
                        workSheet.Column(2).Style.WrapText = true;

                        excel.Save();

                        MainViewModel.Instance.Models.TrimesterModels.Add(Path.GetFileName(filename));
                    }
                }

                subjectCount++;
                _updateTrimesterModelsDispatch(subjectCount * 700 / MainViewModel.Instance.Parameters.Subjects.Count);
            }
            int classCount = 0;
            foreach (ClassViewModel _class in MainViewModel.Instance.Parameters.Classes)
            {
                foreach (TeacherViewModel teacher in MainViewModel.Instance.Parameters.Teachers)
                {
                    string filename = Path.Combine(directory, string.Format("Appréciations trimestre {0} - toutes les matières - {1} - {2}.xlsx", trimester,
                        string.IsNullOrEmpty(teacher.FirstName) ? teacher.LastName : $"{teacher.LastName} {teacher.FirstName}", _class.Name));
                    File.Delete(filename);

                    ExcelPackage excel = new ExcelPackage(new FileInfo(filename));

                    ExcelWorksheet workSheet = excel.Workbook.Worksheets.Add("Feuil1");
                    workSheet.Cells[1, 1].Value = "type";
                    workSheet.Cells[1, 2].Value = "APP_MAT_TRM";
                    workSheet.Cells[2, 1].Value = "année";
                    workSheet.Cells[2, 2].Value = _class.Year;
                    workSheet.Cells[3, 1].Value = "trimestre";
                    workSheet.Cells[3, 2].Value = trimester;
                    workSheet.Cells[4, 1].Value = "professeur";
                    workSheet.Cells[4, 2].Value = string.IsNullOrEmpty(teacher.FirstName) ? teacher.LastName : $"{teacher.LastName} {teacher.FirstName}";
                    workSheet.Cells[5, 1].Value = "élève / matière";

                    int j = 2;
                    foreach (SubjectViewModel subject in teacher.Subjects.OrderBy(s => s.Order))
                    {
                        if (!subject.ChildrenSubjects.Any())
                        {
                            workSheet.Cells[5, j].Value = subject.Name;
                            workSheet.Column(j).Width = 50;
                            workSheet.Column(j).Style.WrapText = true;
                        }
                        j++;
                    }
                    int row = 6;
                    foreach (StudentViewModel student in _class.Students)
                    {
                        workSheet.Cells[row++, 1].Value = $"{student.LastName} {student.FirstName}";
                    }

                    IExcelDataValidationInt _textValidation = workSheet.Cells.DataValidation.AddTextLengthDataValidation();
                    _textValidation.ShowErrorMessage = true;
                    _textValidation.ErrorStyle = ExcelDataValidationWarningStyle.warning;
                    _textValidation.ErrorTitle = "Commentaire trop long";
                    _textValidation.Error = "Le commentaire ne doit pas dépasser 120 caractères.";
                    _textValidation.Formula.Value = 0;
                    _textValidation.Formula2.Value = 120;

                    workSheet.Column(1).AutoFit();
                    excel.Save();

                    MainViewModel.Instance.Models.TrimesterModels.Add(Path.GetFileName(filename));
                }

                classCount++;
                _updateTrimesterModelsDispatch(700 + classCount * 200 / MainViewModel.Instance.Parameters.Classes.Count);
            }
        }

        public static void ExportTrimesterModels(int trimester, UpdateTrimesterModelsDelegate _updateTrimesterModelsDispatch)
        {
            try
            {
                string directory = FileUtils.SelectDirectory(Settings.Settings.Instance.LastSelectedDirectoryTrimesterModels, "LastSelectedDirectoryTrimesterModels");

                if (!string.IsNullOrEmpty(directory))
                {
                    MainViewModel.Instance.Models.TrimesterModels.Clear();

                    ExportTrimesterSubjectCommentsModels(trimester, directory, _updateTrimesterModelsDispatch);
                    ExportTrimesterGeneralCommentsModels(trimester, directory, _updateTrimesterModelsDispatch);

                    MainViewModel.Instance.Models.TrimesterModelsPath = directory;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public static void ExportSemiTrimesterModels(SemiTrimesterViewModel semiTrimester, UpdateSemiTrimesterModelsDelegate _updateSemiTrimesterModelsDispatch)
        {
            try
            {
                string directory = FileUtils.SelectDirectory(Settings.Settings.Instance.LastSelectedDirectorySemiTrimesterModels, "LastSelectedDirectorySemiTrimesterModels");

                if (!string.IsNullOrEmpty(directory))
                {
                    MainViewModel.Instance.Models.SemiTrimesterModels.Clear();

                    int classCount = 0;
                    foreach (ClassViewModel _class in MainViewModel.Instance.Parameters.Classes)
                    {
                        string filename = Path.Combine(directory, $"Appréciations demi-trimestre {(semiTrimester.Name.ToUpper().StartsWith("A") || semiTrimester.Name.ToUpper().StartsWith("O") ? "d'" : "de ")}{semiTrimester.Name} - {_class.Name}.xlsx");
                        File.Delete(filename);

                        ExcelPackage excel = new ExcelPackage(new FileInfo(filename));

                        ExcelWorksheet workSheet = excel.Workbook.Worksheets.Add("Feuil1");
                        workSheet.Cells[1, 1].Value = "type";
                        workSheet.Cells[1, 2].Value = "APP_GEN_DTM";
                        workSheet.Cells[2, 1].Value = "année";
                        workSheet.Cells[2, 2].Value = _class.Year;
                        workSheet.Cells[3, 1].Value = "demi-trimestre";
                        workSheet.Cells[3, 2].Value = semiTrimester.Name;
                        workSheet.Cells[4, 1].Value = "élève";
                        workSheet.Cells[4, 2].Value = "professeur principal";
                        workSheet.Cells[4, 3].Value = "chef d'établissement";

                        int row = 5;
                        foreach (StudentViewModel student in _class.Students)
                        {
                            workSheet.Cells[row, 1].Value = $"{student.LastName} {student.FirstName}";
                            SemiTrimesterCommentModel semiTrimesterComment = SemiTrimesterCommentModel.Read(semiTrimester, student);
                            if (semiTrimesterComment != null)
                            {
                                workSheet.Cells[row, 2].Value = semiTrimesterComment.MainTeacherComment;
                                workSheet.Cells[row, 3].Value = semiTrimesterComment.DivisionPrefectComment;
                            }
                            row++;
                        }

                        workSheet.Column(1).AutoFit();
                        workSheet.Column(2).Width = 50;
                        workSheet.Column(2).Style.WrapText = true;
                        workSheet.Column(3).Width = 50;
                        workSheet.Column(3).Style.WrapText = true;

                        IExcelDataValidationInt _textValidation = workSheet.Cells.DataValidation.AddTextLengthDataValidation();
                        _textValidation.ShowErrorMessage = true;
                        _textValidation.ErrorStyle = ExcelDataValidationWarningStyle.warning;
                        _textValidation.ErrorTitle = "Commentaire trop long";
                        _textValidation.Error = "Le commentaire ne doit pas dépasser 180 caractères.";
                        _textValidation.Formula.Value = 0;
                        _textValidation.Formula2.Value = 180;

                        excel.Save();

                        MainViewModel.Instance.Models.SemiTrimesterModels.Add(Path.GetFileName(filename));

                        classCount++;
                        _updateSemiTrimesterModelsDispatch(classCount * 1000 / MainViewModel.Instance.Parameters.Classes.Count);
                    }

                    MainViewModel.Instance.Models.SemiTrimesterModelsPath = directory;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private static StudentViewModel GetStudentFromName(string name)
        {
            return MainViewModel.Instance.Parameters.Students.FirstOrDefault(s => $"{s.LastName} {s.FirstName}" == name);
        }

        private static TeacherViewModel GetTeacherFromName(string name)
        {
            TeacherViewModel teacher = null;

            int lowerCase;
            for (lowerCase = 0; lowerCase < name.Length; lowerCase++)
            {
                if (name[lowerCase] >= 'a' && name[lowerCase] <= 'z')
                {
                    break;
                }
            }
            if (lowerCase < name.Length)
            {
                lowerCase = name.Substring(0, lowerCase).LastIndexOf(' ');
                string lastName = name.Substring(0, lowerCase);
                string firstName = name.Substring(lowerCase + 1);

                teacher = MainViewModel.Instance.Parameters.Teachers.FirstOrDefault(t => t.FirstName == firstName && t.LastName == lastName);
            }
            else
            {
                teacher = MainViewModel.Instance.Parameters.Teachers.FirstOrDefault(t => t.LastName == name);
            }

            return teacher;
        }

        public static void Import(string filename)
        {
            ExcelPackage excel = new ExcelPackage(new FileInfo(filename));
            ExcelWorksheet workSheet = excel.Workbook.Worksheets[0];
            switch (workSheet.Cells[1, 2].Text)
            {
                case "APP_GEN_PER":
                    ImportPeriodComments(filename, workSheet);
                    break;
                case "MRK_PER":
                    ImportPeriodMarks(filename, workSheet);
                    break;
                case "APP_GEN_DTM":
                    ImportSemiTrimesterComments(filename, workSheet);
                    break;
                case "APP_MAT_TRM":
                    ImportTrimesterSubjectComments(filename, workSheet);
                    break;
                case "APP_GEN_TRM":
                    ImportTrimesterGeneralComments(filename, workSheet);
                    break;
                default:
                    MessageBox.Show("Format non reconnu.", "Echec", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }
        }

        private static bool ImportPeriodComments(string filename, ExcelWorksheet workSheet)
        {
            int year = (int)(double)workSheet.Cells[2, 2].Value;
            PeriodViewModel period = MainViewModel.Instance.Parameters.Periods.FirstOrDefault(p => p.Number == (int)(double)workSheet.Cells[3, 2].Value);

            if (year == 0 || period == null)
            {
                return false;
            }

            List<PeriodCommentModel> periodComments = new List<PeriodCommentModel>();

            int i = 5;
            while (workSheet.Cells[i, 1].Value != null)
            {
                if (workSheet.Cells[i, 2].Value != null || workSheet.Cells[i, 3].Value != null)
                {
                    StudentViewModel student = GetStudentFromName(workSheet.Cells[i, 1].Text);

                    PeriodCommentModel periodComment = new PeriodCommentModel()
                    {
                        IdPeriod = period.Id,
                        IdStudent = student.Id,
                        Year = year,
                    };

                    if (workSheet.Cells[i, 2].Value != null)
                    {
                        switch (workSheet.Cells[i, 2].Text)
                        {
                            case "1":
                                periodComment.StudiesReport = 1;
                                break;
                            case "2":
                                periodComment.StudiesReport = 2;
                                break;
                            case "3":
                                periodComment.StudiesReport = 3;
                                break;
                            case "A":
                                periodComment.StudiesReport = 4;
                                break;
                            default:
                                break;
                        }
                    }
                    if (workSheet.Cells[i, 3].Value != null)
                    {
                        switch (workSheet.Cells[i, 3].Text)
                        {
                            case "1":
                                periodComment.DisciplineReport = 1;
                                break;
                            case "2":
                                periodComment.DisciplineReport = 2;
                                break;
                            case "3":
                                periodComment.DisciplineReport = 3;
                                break;
                            case "A":
                                periodComment.DisciplineReport = 4;
                                break;
                            default:
                                break;
                        }
                    }
                    periodComments.Add(periodComment);
                }
                i++;
            }

            PeriodCommentModel.Save(periodComments, year);

            return true;
        }

        private static bool ImportSemiTrimesterComments(string filename, ExcelWorksheet workSheet)
        {
            int year = (int)(double)workSheet.Cells[2, 2].Value;
            SemiTrimesterViewModel semiTrimester = MainViewModel.Instance.Reports.SemiTrimesters.FirstOrDefault(s => s.Name == workSheet.Cells[3, 2].Text);

            if (year == 0 || semiTrimester == null)
            {
                return false;
            }

            int i = 5;
            while (workSheet.Cells[i, 1].Value != null)
            {
                if (workSheet.Cells[i, 2].Value != null || workSheet.Cells[i, 3].Value != null)
                {
                    StudentViewModel student = GetStudentFromName(workSheet.Cells[i, 1].Text);

                    SemiTrimesterCommentModel semiTrimesterComment = new SemiTrimesterCommentModel()
                    {
                        IdSemiTrimester = semiTrimester.Id,
                        IdStudent = student.Id,
                        Year = year,
                    };

                    if (workSheet.Cells[i, 2].Value != null)
                    {
                        semiTrimesterComment.MainTeacherComment = workSheet.Cells[i, 2].Text;
                    }
                    if (workSheet.Cells[i, 3].Value != null)
                    {
                        semiTrimesterComment.DivisionPrefectComment = workSheet.Cells[i, 3].Text;
                    }

                    SemiTrimesterCommentModel.Save(semiTrimesterComment);
                }
                i++;
            }

            return true;
        }

        private static bool ImportTrimesterSubjectComments(string filename, ExcelWorksheet workSheet)
        {
            int year = (int)(double)workSheet.Cells[2, 2].Value;
            int trimester = (int)(double)workSheet.Cells[3, 2].Value;
            TeacherViewModel teacher = GetTeacherFromName(workSheet.Cells[4, 2].Text);
            SubjectViewModel subject = MainViewModel.Instance.Parameters.Subjects.FirstOrDefault(s => s.Name == workSheet.Cells[5, 2].Text);

            if (year == 0 || trimester == 0 || teacher == null || subject == null)
            {
                return false;
            }

            int i = 6;
            while (workSheet.Cells[i, 1].Value != null)
            {
                int j = 2;
                while (workSheet.Cells[5, j].Value != null)
                {
                    if (MainViewModel.Instance.Parameters.Subjects.Count(s => s.Name == workSheet.Cells[5, j].Text) > 1)
                    {
                        subject = MainViewModel.Instance.Parameters.Subjects.FirstOrDefault(s => s.Name == workSheet.Cells[5, j].Text && teacher.Subjects.Contains(s));
                        if (subject == null)
                        {
                            foreach (SubjectViewModel mainSubject in teacher.Subjects)
                            {
                                subject = mainSubject.ChildrenSubjects.FirstOrDefault(s => s.Name == workSheet.Cells[5, j].Text);
                                if (subject != null)
                                {
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        subject = MainViewModel.Instance.Parameters.Subjects.FirstOrDefault(s => s.Name == workSheet.Cells[5, j].Text);
                    }

                    if (workSheet.Cells[i, 2].Value != null)
                    {
                        StudentViewModel student = GetStudentFromName(workSheet.Cells[i, 1].Text);

                        TrimesterSubjectCommentModel trimesterSubjectComment = new TrimesterSubjectCommentModel()
                        {
                            Trimester = trimester,
                            IdStudent = student.Id,
                            IdSubject = subject.Id,
                            Comment = workSheet.Cells[i, j].Text,
                            Year = year,
                        };

                        TrimesterSubjectCommentModel.Save(trimesterSubjectComment);
                    }
                    j++;
                }
                i++;
            }

            return true;
        }

        private static bool ImportTrimesterComments(string filename, ExcelWorksheet workSheet)
        {
            int year = (int)(double)workSheet.Cells[2, 2].Value;
            int trimester = (int)(double)workSheet.Cells[3, 2].Value;

            if (year == 0 || trimester == 0)
            {
                return false;
            }

            int i = 5;
            while (workSheet.Cells[i, 1].Value != null)
            {
                if (workSheet.Cells[i, 2].Value != null || workSheet.Cells[i, 3].Value != null)
                {
                    StudentViewModel student = GetStudentFromName(workSheet.Cells[i, 1].Text);

                    TrimesterCommentModel trimesterComment = new TrimesterCommentModel()
                    {
                        Trimester = trimester,
                        IdStudent = student.Id,
                        Year = year,
                    };

                    if (workSheet.Cells[i, 2].Value != null)
                    {
                        trimesterComment.MainTeacherComment = workSheet.Cells[i, 2].Text;
                    }
                    if (workSheet.Cells[i, 3].Value != null)
                    {
                        trimesterComment.DivisionPrefectComment = workSheet.Cells[i, 3].Text;
                    }

                    TrimesterCommentModel.Save(trimesterComment);
                }
                i++;
            }

            return true;
        }

        private static bool ImportPeriodMarks(string filename, ExcelWorksheet workSheet)
        {
            int year = (int)(double)workSheet.Cells[2, 2].Value;
            PeriodViewModel period = MainViewModel.Instance.Parameters.Periods.FirstOrDefault(p => p.Number == (int)(double)workSheet.Cells[3, 2].Value);
            TeacherViewModel teacher = GetTeacherFromName(workSheet.Cells[4, 2].Text);

            SubjectViewModel subject = MainViewModel.Instance.Parameters.Subjects.FirstOrDefault(s => s.Name == workSheet.Cells[5, 2].Text);

            if (year == 0 || period == null || teacher == null || subject == null)
            {
                return false;
            }

            int i = 2;
            while (workSheet.Cells[6, i].Value != null)
            {
                i++;
            }
            int[] coefficients = new int[i - 2];
            for (i = 0; i < coefficients.Length; i++)
            {
                coefficients[i] = (int)(double)workSheet.Cells[6, i + 2].Value;
            }

            i = 7;
            List<MarkModel> clearMarks = new List<MarkModel>();
            List<MarkModel> marks = new List<MarkModel>();

            while (workSheet.Cells[i, 1].Value != null)
            {
                StudentViewModel student = GetStudentFromName(workSheet.Cells[i, 1].Text);

                int j = 2;
                while (j < coefficients.Length + 2)
                {
                    if (workSheet.Cells[5, j].Value != null)
                    {
                        if (MainViewModel.Instance.Parameters.Subjects.Count(s => s.Name == workSheet.Cells[5, j].Text) > 1)
                        {
                            subject = MainViewModel.Instance.Parameters.Subjects.FirstOrDefault(s => s.Name == workSheet.Cells[5, j].Text && teacher.Subjects.Contains(s));
                            if (subject == null)
                            {
                                foreach (SubjectViewModel mainSubject in teacher.Subjects)
                                {
                                    subject = mainSubject.ChildrenSubjects.FirstOrDefault(s => s.Name == workSheet.Cells[5, j].Text);
                                    if (subject != null)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            subject = MainViewModel.Instance.Parameters.Subjects.FirstOrDefault(s => s.Name == workSheet.Cells[5, j].Text);
                        }
                        if (subject == null)
                        {
                            MessageBox.Show($"{filename} : matière invalide '{workSheet.Cells[5, j].Text}'");
                            return false;
                        }
                        clearMarks.Add(new MarkModel()
                        {
                            IdClass = student.Class.Id,
                            IdPeriod = period.Id,
                            IdStudent = student.Id,
                            IdSubject = subject.Id,
                            IdTeacher = teacher.Id,
                        });
                    }

                    if (workSheet.Cells[i, j].Value != null)
                    {
                        try
                        {
                            MarkModel mark = new MarkModel()
                            {
                                Coefficient = j < coefficients.Length + 2 ? coefficients[j - 2] : 1,
                                Order = j - 2,
                                IdClass = student.Class.Id,
                                IdPeriod = period.Id,
                                IdStudent = student.Id,
                                IdSubject = subject.Id,
                                IdTeacher = teacher.Id,
                                Mark = (decimal)(double)workSheet.Cells[i, j].Value,
                                Year = year,
                            };
                            marks.Add(mark);
                        }
                        catch
                        {
                        }
                    }
                    j++;
                }

                i++;
            }

            MarkModel.Clear(clearMarks, year);
            MarkModel.Save(marks, year);

            return true;
        }

        private static string RowColumnToCell(int row, int column)
        {
            column--;
            return $"{(column >= 26 ? ALPHABET[column / 26 - 1].ToString() + ALPHABET[column % 26].ToString() : ALPHABET[column].ToString())}{row}";
        }

        public static void ExportPeriodSummary(string directory, ClassViewModel _class, PeriodViewModel period)
        {
            string filename = Path.Combine(directory, $"Bulletin de période {period.Number} de {_class.Name} (résumé).xlsx");
            File.Delete(filename);

            ExcelPackage excel = new ExcelPackage(new FileInfo(filename));

            ExcelWorksheet workSheet = excel.Workbook.Worksheets.Add("Feuil1");

            int i;
            int j = 2;
            foreach (SubjectViewModel subject in _class.Level.Subjects)
            {
                if (!subject.ChildrenSubjects.Any())
                {
                    workSheet.Cells[1, j].Value = subject.Name;
                    workSheet.Cells[2, j].Value = subject.Coefficient;
                    workSheet.Cells[3, j].Value = subject.Option ? "X" : "";
                    workSheet.Column(j + 1).Hidden = true;
                    workSheet.Column(j + 2).Hidden = true;
                    j += 3;
                }
                else
                {
                    foreach (SubjectViewModel subject2 in subject.ChildrenSubjects)
                    {
                        workSheet.Cells[1, j].Value = subject2.Name;
                        workSheet.Cells[2, j].Value = subject2.Coefficient;
                        workSheet.Cells[3, j].Value = subject2.Option ? "X" : "";
                        workSheet.Column(j + 1).Hidden = true;
                        workSheet.Column(j + 2).Hidden = true;
                        j += 3;
                    }
                }
            }
            workSheet.Row(2).Hidden = true;
            workSheet.Row(3).Hidden = true;
            workSheet.Column(j + 1).Hidden = true;
            workSheet.Column(j + 2).Hidden = true;
            workSheet.Cells[1, j].Value = "Général";

            double value;
            i = 4;
            foreach (StudentViewModel student in _class.Students)
            {
                workSheet.Cells[i, 1].Value = $"{student.LastName} {student.FirstName}";
                j = 2;
                foreach (SubjectViewModel subject in _class.Level.Subjects)
                {
                    if (!subject.ChildrenSubjects.Any())
                    {
                        value = MarkModel.ReadPeriodTrimesterSubjectAverage(period, student, subject);
                        if (value == double.MinValue)
                        {
                            workSheet.Cells[i, j].Value = "";
                        }
                        else
                        {
                            workSheet.Cells[i, j].Value = value;
                        }
                        workSheet.Cells[i + 1, j + 1].Formula = string.Format("IF({0}=\"\",IF({1}=\"\",0,{1}*{2}),IF({1}=\"\",0,MAX(0,{1}*{2}-10)))", RowColumnToCell(3, j), RowColumnToCell(i, j), RowColumnToCell(2, j));
                        workSheet.Cells[i + 2, j + 2].Formula = string.Format("IF({0}=\"\",0,IF({1}=\"\",{2},0))", RowColumnToCell(i, j), RowColumnToCell(3, j), RowColumnToCell(2, j));
                        j += 3;
                    }
                    else
                    {
                        foreach (SubjectViewModel subject2 in subject.ChildrenSubjects)
                        {
                            value = MarkModel.ReadPeriodTrimesterSubjectAverage(period, student, subject2);
                            if (value == double.MinValue)
                            {
                                workSheet.Cells[i, j].Value = "";
                            }
                            else
                            {
                                workSheet.Cells[i, j].Value = value;
                            }
                            workSheet.Cells[i + 1, j + 1].Formula = string.Format("IF({0}=\"\",IF({1}=\"\",0,{1}*{2}),IF({1}=\"\",0,MAX(0,{1}*{2}-10)))", RowColumnToCell(3, j), RowColumnToCell(i, j), RowColumnToCell(2, j));
                            workSheet.Cells[i + 2, j + 2].Formula = string.Format("IF({0}=\"\",0,IF({1}=\"\",{2},0))", RowColumnToCell(i, j), RowColumnToCell(3, j), RowColumnToCell(2, j));
                            j += 3;
                        }
                    }
                }
                workSheet.Cells[i + 1, j + 1].Formula = string.Format("SUM({0}:{1})", RowColumnToCell(i + 1, 3), RowColumnToCell(i + 1, j - 1));
                workSheet.Cells[i + 2, j + 1].Formula = string.Format("SUM({0}:{1})", RowColumnToCell(i + 2, 3), RowColumnToCell(i + 2, j - 1));
                workSheet.Cells[i, j].Formula = string.Format("IF({1}=0,\"\",{0}/{1})", RowColumnToCell(i + 1, j + 1), RowColumnToCell(i + 2, j + 1));
                workSheet.Cells[i, j + 2].Formula = string.Format("IF({0}=\"\",0,1)", RowColumnToCell(i, j));

                workSheet.Row(i + 1).Hidden = true;
                workSheet.Row(i + 2).Hidden = true;
                i += 3;
            }

            workSheet.Cells[i, 1].Value = "Général";
            j = 2;
            foreach (SubjectViewModel subject in _class.Level.Subjects)
            {
                if (!subject.ChildrenSubjects.Any())
                {
                    workSheet.Cells[i, j + 1].Formula = string.Format("SUM({0}:{1})", RowColumnToCell(3, j + 1), RowColumnToCell(i - 1, j + 1));
                    workSheet.Cells[i, j + 2].Formula = string.Format("SUM({0}:{1})", RowColumnToCell(3, j + 2), RowColumnToCell(i - 1, j + 2));
                    workSheet.Cells[i, j].Formula = string.Format("IF({1}=0,\"\",{0}/{1})", RowColumnToCell(i, j + 1), RowColumnToCell(i, j + 2));
                    j += 3;
                }
                else
                {
                    foreach (SubjectViewModel subject2 in subject.ChildrenSubjects)
                    {
                        workSheet.Cells[i, j + 1].Formula = string.Format("SUM({0}:{1})", RowColumnToCell(3, j + 1), RowColumnToCell(i - 1, j + 1));
                        workSheet.Cells[i, j + 2].Formula = string.Format("SUM({0}:{1})", RowColumnToCell(3, j + 2), RowColumnToCell(i - 1, j + 2));
                        workSheet.Cells[i, j].Formula = string.Format("IF({1}=0,\"\",{0}/{1})", RowColumnToCell(i, j + 1), RowColumnToCell(i, j + 2));
                        j += 3;
                    }
                }
            }
            workSheet.Cells[i, j + 1].Formula = string.Format("SUM({0}:{1})", RowColumnToCell(3, j), RowColumnToCell(i - 1, j));
            workSheet.Cells[i, j + 2].Formula = string.Format("SUM({0}:{1})", RowColumnToCell(3, j + 2), RowColumnToCell(i - 1, j + 2));
            workSheet.Cells[i, j].Formula = string.Format("IF({1}=0,\"\",{0}/{1})", RowColumnToCell(i, j + 1), RowColumnToCell(i, j + 2));

            for (int i2 = 1; i2 < i + 1; i2++)
            {
                for (int j2 = 1; j2 < j + 3; j2++)
                {
                    workSheet.Cells[i2, j2].Style.Numberformat.Format = "0.0";
                    workSheet.Cells[i2, j2].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[i2, j2].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[i2, j2].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[i2, j2].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                }
            }

            workSheet.Column(1).AutoFit();
            excel.Save();

            MainViewModel.Instance.Reports.PeriodReports.Insert(MainViewModel.Instance.Reports.PeriodReports.Count(r => r.Contains("résumé")), Path.GetFileName(filename));
        }

        private static bool ImportTrimesterGeneralComments(string filename, ExcelWorksheet workSheet)
        {
            int year = (int)(double)workSheet.Cells[2, 2].Value;
            int trimester = (int)(double)workSheet.Cells[3, 2].Value;

            if (year == 0 || trimester == 0)
            {
                return false;
            }

            int i = 5;
            while (workSheet.Cells[i, 1].Value != null)
            {
                if (workSheet.Cells[i, 2].Value != null || workSheet.Cells[i, 3].Value != null)
                {
                    StudentViewModel student = GetStudentFromName(workSheet.Cells[i, 1].Text);

                    TrimesterCommentModel trimesterComment = new TrimesterCommentModel()
                    {
                        Trimester = trimester,
                        IdStudent = student.Id,
                        Year = year,
                    };

                    if (workSheet.Cells[i, 2].Value != null)
                    {
                        trimesterComment.MainTeacherComment = workSheet.Cells[i, 2].Text;
                    }
                    if (workSheet.Cells[i, 3].Value != null)
                    {
                        trimesterComment.DivisionPrefectComment = workSheet.Cells[i, 3].Text;
                    }

                    TrimesterCommentModel.Save(trimesterComment);
                }
                i++;
            }

            return true;
        }

        public static void ExportTrimesterSummary(string directory, ClassViewModel _class, int trimester)
        {
            string filename = Path.Combine(directory, $"Bulletin de trimestre {trimester} de {_class.Name} (résumé).xlsx");
            File.Delete(filename);

            ExcelPackage excel = new ExcelPackage(new FileInfo(filename));

            ExcelWorksheet workSheet = excel.Workbook.Worksheets.Add("Feuil1");

            int i;
            int j = 2;
            foreach (SubjectViewModel subject in _class.Level.Subjects)
            {
                if (!subject.ChildrenSubjects.Any())
                {
                    workSheet.Cells[1, j].Value = subject.Name;
                    workSheet.Cells[2, j].Value = subject.Coefficient;
                    workSheet.Cells[3, j].Value = subject.Option ? "X" : "";
                    workSheet.Column(j + 1).Hidden = true;
                    workSheet.Column(j + 2).Hidden = true;
                    j += 3;
                }
                else
                {
                    foreach (SubjectViewModel subject2 in subject.ChildrenSubjects)
                    {
                        workSheet.Cells[1, j].Value = subject2.Name;
                        workSheet.Cells[2, j].Value = subject2.Coefficient;
                        workSheet.Cells[3, j].Value = subject2.Option ? "X" : "";
                        workSheet.Column(j + 1).Hidden = true;
                        workSheet.Column(j + 2).Hidden = true;
                        j += 3;
                    }
                }
            }
            workSheet.Row(2).Hidden = true;
            workSheet.Row(3).Hidden = true;
            workSheet.Column(j + 1).Hidden = true;
            workSheet.Column(j + 2).Hidden = true;
            workSheet.Cells[1, j].Value = "Général";

            double value;
            i = 4;
            foreach (StudentViewModel student in _class.Students)
            {
                workSheet.Cells[i, 1].Value = $"{student.LastName} {student.FirstName}";
                j = 2;
                foreach (SubjectViewModel subject in _class.Level.Subjects)
                {
                    if (!subject.ChildrenSubjects.Any())
                    {
                        value = MarkModel.ReadTrimesterSubjectAverage(trimester, student, subject);
                        if (value == double.MinValue)
                        {
                            workSheet.Cells[i, j].Value = "";
                        }
                        else
                        {
                            workSheet.Cells[i, j].Value = value;
                        }
                        workSheet.Cells[i + 1, j + 1].Formula = string.Format("IF({0}=\"\",IF({1}=\"\",0,{1}*{2}),IF({1}=\"\",0,MAX(0,{1}*{2}-10)))", RowColumnToCell(3, j), RowColumnToCell(i, j), RowColumnToCell(2, j));
                        workSheet.Cells[i + 2, j + 2].Formula = string.Format("IF({0}=\"\",0,IF({1}=\"\",{2},0))", RowColumnToCell(i, j), RowColumnToCell(3, j), RowColumnToCell(2, j));
                        j += 3;
                    }
                    else
                    {
                        foreach (SubjectViewModel subject2 in subject.ChildrenSubjects)
                        {
                            value = MarkModel.ReadTrimesterSubjectAverage(trimester, student, subject2);
                            if (value == double.MinValue)
                            {
                                workSheet.Cells[i, j].Value = "";
                            }
                            else
                            {
                                workSheet.Cells[i, j].Value = value;
                            }
                            workSheet.Cells[i + 1, j + 1].Formula = string.Format("IF({0}=\"\",IF({1}=\"\",0,{1}*{2}),IF({1}=\"\",0,MAX(0,{1}*{2}-10)))", RowColumnToCell(3, j), RowColumnToCell(i, j), RowColumnToCell(2, j));
                            workSheet.Cells[i + 2, j + 2].Formula = string.Format("IF({0}=\"\",0,IF({1}=\"\",{2},0))", RowColumnToCell(i, j), RowColumnToCell(3, j), RowColumnToCell(2, j));
                            j += 3;
                        }
                    }
                }
                workSheet.Cells[i + 1, j + 1].Formula = string.Format("SUM({0}:{1})", RowColumnToCell(i + 1, 3), RowColumnToCell(i + 1, j - 1));
                workSheet.Cells[i + 2, j + 1].Formula = string.Format("SUM({0}:{1})", RowColumnToCell(i + 2, 3), RowColumnToCell(i + 2, j - 1));
                workSheet.Cells[i, j].Formula = string.Format("IF({1}=0,\"\",{0}/{1})", RowColumnToCell(i + 1, j + 1), RowColumnToCell(i + 2, j + 1));
                workSheet.Cells[i, j + 2].Formula = string.Format("IF({0}=\"\",0,1)", RowColumnToCell(i, j));

                workSheet.Row(i + 1).Hidden = true;
                workSheet.Row(i + 2).Hidden = true;
                i += 3;
            }

            workSheet.Cells[i, 1].Value = "Général";
            j = 2;
            foreach (SubjectViewModel subject in _class.Level.Subjects)
            {
                if (!subject.ChildrenSubjects.Any())
                {
                    workSheet.Cells[i, j + 1].Formula = string.Format("SUM({0}:{1})", RowColumnToCell(3, j + 1), RowColumnToCell(i - 1, j + 1));
                    workSheet.Cells[i, j + 2].Formula = string.Format("SUM({0}:{1})", RowColumnToCell(3, j + 2), RowColumnToCell(i - 1, j + 2));
                    workSheet.Cells[i, j].Formula = string.Format("IF({1}=0,\"\",{0}/{1})", RowColumnToCell(i, j + 1), RowColumnToCell(i, j + 2));
                    j += 3;
                }
                else
                {
                    foreach (SubjectViewModel subject2 in subject.ChildrenSubjects)
                    {
                        workSheet.Cells[i, j + 1].Formula = string.Format("SUM({0}:{1})", RowColumnToCell(3, j + 1), RowColumnToCell(i - 1, j + 1));
                        workSheet.Cells[i, j + 2].Formula = string.Format("SUM({0}:{1})", RowColumnToCell(3, j + 2), RowColumnToCell(i - 1, j + 2));
                        workSheet.Cells[i, j].Formula = string.Format("IF({1}=0,\"\",{0}/{1})", RowColumnToCell(i, j + 1), RowColumnToCell(i, j + 2));
                        j += 3;
                    }
                }
            }
            workSheet.Cells[i, j + 1].Formula = string.Format("SUM({0}:{1})", RowColumnToCell(3, j), RowColumnToCell(i - 1, j));
            workSheet.Cells[i, j + 2].Formula = string.Format("SUM({0}:{1})", RowColumnToCell(3, j + 2), RowColumnToCell(i - 1, j + 2));
            workSheet.Cells[i, j].Formula = string.Format("IF({1}=0,\"\",{0}/{1})", RowColumnToCell(i, j + 1), RowColumnToCell(i, j + 2));

            for (int i2 = 1; i2 < i + 1; i2++)
            {
                for (int j2 = 1; j2 < j + 3; j2++)
                {
                    workSheet.Cells[i2, j2].Style.Numberformat.Format = "0.0";
                    workSheet.Cells[i2, j2].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[i2, j2].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[i2, j2].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[i2, j2].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                }
            }

            workSheet.Column(1).AutoFit();
            excel.Save();

            MainViewModel.Instance.Reports.TrimesterReports.Insert(MainViewModel.Instance.Reports.TrimesterReports.Count(r => r.Contains("résumé")), Path.GetFileName(filename));
        }
    }
}

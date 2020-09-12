using Microsoft.Reporting.WebForms;
using Notation.Models;
using Notation.Properties;
using Notation.Reports;
using Notation.ViewModels;
using Notation.Views;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;

namespace Notation.Utils
{
    public class SSRSUtils_SemiTrimester
    {
        public ClassViewModel Class { get; set; }
        public double ClassAverage { get; set; }
        public double ClassMinAverage { get; set; }
        public double ClassMaxAverage { get; set; }
        public Dictionary<SubjectViewModel, double> ClassSubjectAverages { get; set; }
        public Dictionary<SubjectViewModel, double> ClassSubjectMinAverages { get; set; }
        public Dictionary<SubjectViewModel, double> ClassSubjectMaxAverages { get; set; }
        public Dictionary<StudentViewModel, double> StudentAverages { get; set; }

        public SSRSUtils_SemiTrimester()
        {
            ClassSubjectAverages = new Dictionary<SubjectViewModel, double>();
            ClassSubjectMinAverages = new Dictionary<SubjectViewModel, double>();
            ClassSubjectMaxAverages = new Dictionary<SubjectViewModel, double>();
            StudentAverages = new Dictionary<StudentViewModel, double>();
        }
    }

    public class SSRSUtils_Trimester
    {
        public ClassViewModel Class { get; set; }
        public double ClassAverage { get; set; }
        public double ClassMinAverage { get; set; }
        public double ClassMaxAverage { get; set; }
        public Dictionary<SubjectViewModel, double> ClassSubjectAverages { get; set; }
        public Dictionary<SubjectViewModel, double> ClassSubjectMinAverages { get; set; }
        public Dictionary<SubjectViewModel, double> ClassSubjectMaxAverages { get; set; }
        public Dictionary<StudentViewModel, double> StudentAverages { get; set; }

        public SSRSUtils_Trimester()
        {
            ClassSubjectAverages = new Dictionary<SubjectViewModel, double>();
            ClassSubjectMinAverages = new Dictionary<SubjectViewModel, double>();
            ClassSubjectMaxAverages = new Dictionary<SubjectViewModel, double>();
            StudentAverages = new Dictionary<StudentViewModel, double>();
        }
    }

    public class SSRSUtils_Year
    {
        public ClassViewModel Class { get; set; }
        public double ClassAverage { get; set; }
        public double ClassMinAverage { get; set; }
        public double ClassMaxAverage { get; set; }
        public Dictionary<SubjectViewModel, double> ClassSubjectAverages { get; set; }
        public Dictionary<SubjectViewModel, double> ClassSubjectMinAverages { get; set; }
        public Dictionary<SubjectViewModel, double> ClassSubjectMaxAverages { get; set; }
        public Dictionary<StudentViewModel, double> StudentAverages { get; set; }

        public SSRSUtils_Year()
        {
            ClassSubjectAverages = new Dictionary<SubjectViewModel, double>();
            ClassSubjectMinAverages = new Dictionary<SubjectViewModel, double>();
            ClassSubjectMaxAverages = new Dictionary<SubjectViewModel, double>();
            StudentAverages = new Dictionary<StudentViewModel, double>();
        }
    }

    public static class SSRSUtils
    {
        static public void CreatePeriodReport(PeriodViewModel period, MainWindow.UpdatePeriodReportsDelegate _updatePeriodReportsDispatch)
        {
            string directory = FileUtils.SelectDirectory(Settings.Default.LastSelectedDirectoryPeriodReports, "LastSelectedDirectoryPeriodReports");

            if (!string.IsNullOrEmpty(directory))
            {
                try
                {
                    MainViewModel.Instance.Reports.PeriodReports.Clear();

                    int studentCount = 0;

                    Dictionary<int, string> students = new Dictionary<int, string>();
                    foreach (StudentViewModel student in MainViewModel.Instance.Parameters.Students)
                    {
                        students[student.Id] = $"{student.LastName}{student.FirstName}";
                    }

                    IEnumerable<IGrouping<int, MarkViewModel>> studentGroups = MarkModel.Read(MainViewModel.Instance.SelectedYear, period.Id).GroupBy(m => m.IdStudent);
                    foreach (IGrouping<int, MarkViewModel> studentGroup in studentGroups.Where(s => MainViewModel.Instance.Parameters.Students.Any(s2 => s2.Id == s.Key)).OrderBy(s => students[s.Key]))
                    {
                        StudentViewModel student = MainViewModel.Instance.Parameters.Students.FirstOrDefault(s => s.Id == studentGroup.Key);
                        if (student != null)
                        {
                            try
                            {
                                GeneratePeriodReport(studentGroup.ToList(), directory, student, period, student.Class);
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show(e.Message);
                            }
                        }
                        studentCount++;
                        _updatePeriodReportsDispatch(studentCount * 950 / studentGroups.Count());
                    }

                    CreatePeriodReportSummary(directory, period);
                    _updatePeriodReportsDispatch(1000);

                    MainViewModel.Instance.Reports.PeriodReportsPath = directory;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        static private void GeneratePeriodReport(IEnumerable<MarkViewModel> marks, string directory, StudentViewModel student, PeriodViewModel period, ClassViewModel _class)
        {
            string filename = Path.Combine(directory, $"Bulletin de période {period.Number} de {_class.Name} de {student.LastName} {student.FirstName}.pdf");

            ReportViewer report = new ReportViewer();
            report.LocalReport.ReportPath = @".\Reports\BulletinPeriode.rdlc";

            BulletinPeriodeHeaderDataSource bulletinPeriodeHeader = new BulletinPeriodeHeaderDataSource()
            {
                BirthDate = $"né le {student.BirthDate.ToShortDateString()}",
                FirstName = student.FirstName,
                LastName = student.LastName,
                Header1 = $"Année {student.Year} - {student.Year + 1}",
                Header2 = $"Période du {period.FromDate.ToShortDateString()} au {period.ToDate.ToShortDateString()}",
                Header3 = $"Classe de {_class.Name}\tEffectif {_class.Students.Count}",
            };

            PeriodCommentViewModel periodComment = PeriodCommentModel.Read(period, student);
            if (periodComment != null)
            {
                bulletinPeriodeHeader.StudiesReport = (int)periodComment.StudiesReport;
                bulletinPeriodeHeader.DisciplineReport = (int)periodComment.DisciplineReport;
            }

            double average = MarkModel.ReadPeriodTrimesterAverage(period, student);
            if (average != double.MinValue)
            {
                bulletinPeriodeHeader.TrimesterAverage = average.ToString("0.0");
            }

            PeriodViewModel lastPeriod = ModelUtils.GetPreviousPeriod(period);
            if (lastPeriod != null)
            {
                double lastAverage = MarkModel.ReadPeriodTrimesterAverage(lastPeriod, student);
                if (lastAverage != double.MinValue)
                {
                    bulletinPeriodeHeader.LastAverage = lastAverage.ToString("0.0");
                    bulletinPeriodeHeader.Tendency = average > lastAverage ? "↗" : average < lastAverage ? "↘" : "→";
                }
            }

            List<BulletinPeriodeLineDataSource> bulletinPeriodeLines = new List<BulletinPeriodeLineDataSource>();

            IEnumerable<int> coefficients = marks.Select(m => m.Coefficient).Distinct().OrderBy(c => c);
            Dictionary<int, string> coefficientLibelles = new Dictionary<int, string>() { { 1, "Leçons" }, { 2, "Devoirs" }, { 4, "Examens" } };

            int i = 1;
            foreach (int coefficient in coefficients)
            {
                switch (i)
                {
                    case 1:
                        bulletinPeriodeHeader.Column1Libelle = string.Format("{0}oefficient {1}", coefficientLibelles.ContainsKey(coefficient) ? coefficientLibelles[coefficient] + " - c" : "C", coefficient);
                        break;
                    case 2:
                        bulletinPeriodeHeader.Column2Libelle = string.Format("{0}oefficient {1}", coefficientLibelles.ContainsKey(coefficient) ? coefficientLibelles[coefficient] + " - c" : "C", coefficient);
                        break;
                    case 3:
                        bulletinPeriodeHeader.Column3Libelle = string.Format("{0}oefficient {1}", coefficientLibelles.ContainsKey(coefficient) ? coefficientLibelles[coefficient] + " - c" : "C", coefficient);
                        break;
                }
                i++;
            }

            foreach (SubjectViewModel subject in _class.Level.Subjects.Where(s => s.ParentSubject == null).OrderBy(s => s.Order))
            {
                BulletinPeriodeLineDataSource bulletinPeriodeLine = new BulletinPeriodeLineDataSource()
                {
                    IsChildSubject = false,
                    Subject = subject.Name.ToUpper(),
                };
                if (subject.Option)
                {
                    bulletinPeriodeLine.Coefficient = $"(option coeff. {subject.Coefficient})";
                }
                else
                {
                    bulletinPeriodeLine.Coefficient = $"(coeff. {subject.Coefficient})";
                }
                TeacherViewModel teacher = ModelUtils.GetTeacherFromClassAndSubject(student.Class, subject);
                if (teacher != null)
                {
                    bulletinPeriodeLine.Teacher = $"{teacher.Title} {(!string.IsNullOrEmpty(teacher.FirstName) ? teacher.FirstName.Substring(0, 1) : "")}. {teacher.LastName}";
                }
                i = 1;
                foreach (int coefficient in coefficients)
                {
                    string marksStr = "";
                    foreach (MarkViewModel mark in marks.Where(m => m.IdSubject == subject.Id && m.Coefficient == coefficient))
                    {
                        if (!string.IsNullOrEmpty(marksStr))
                        {
                            marksStr += "    ";
                        }
                        marksStr += mark.Mark.ToString();
                    }
                    switch (i)
                    {
                        case 1:
                            bulletinPeriodeLine.Marks1 = marksStr;
                            break;
                        case 2:
                            bulletinPeriodeLine.Marks2 = marksStr;
                            break;
                        case 3:
                            bulletinPeriodeLine.Marks3 = marksStr;
                            break;
                    }
                    i++;
                }
                bulletinPeriodeLines.Add(bulletinPeriodeLine);

                foreach (SubjectViewModel subject2 in subject.ChildrenSubjects.OrderBy(s => s.Order))
                {
                    bulletinPeriodeLine = new BulletinPeriodeLineDataSource()
                    {
                        IsChildSubject = true,
                        Subject = subject2.Name.ToUpper(),
                    };
                    if (subject.Option)
                    {
                        bulletinPeriodeLine.Coefficient = $"(option coeff. {subject2.Coefficient})";
                    }
                    else
                    {
                        bulletinPeriodeLine.Coefficient = $"(coeff. {subject2.Coefficient})";
                    }

                    foreach (int coefficient in coefficients)
                    {
                        string marksStr = "";
                        foreach (MarkViewModel mark in marks.Where(m => m.IdSubject == subject2.Id && m.Coefficient == coefficient))
                        {
                            if (!string.IsNullOrEmpty(marksStr))
                            {
                                marksStr += "    ";
                            }
                            marksStr += mark.Mark.ToString();
                        }
                        switch (coefficient)
                        {
                            case 1:
                                bulletinPeriodeLine.Marks1 = marksStr;
                                break;
                            case 2:
                                bulletinPeriodeLine.Marks2 = marksStr;
                                break;
                            case 3:
                                bulletinPeriodeLine.Marks3 = marksStr;
                                break;
                        }
                        i++;
                    }
                    bulletinPeriodeLines.Add(bulletinPeriodeLine);
                }
            }

            ReportDataSource dataSource = new ReportDataSource("HeaderDataSet", new List<BulletinPeriodeHeaderDataSource>() { bulletinPeriodeHeader });
            report.LocalReport.DataSources.Add(dataSource);

            dataSource = new ReportDataSource("LineDataSet", bulletinPeriodeLines);
            report.LocalReport.DataSources.Add(dataSource);

            byte[] bytes = report.LocalReport.Render("PDF", null, out string mimeType, out string encoding, out string extension, out string[] streamIds, out Warning[] warnings);

            using (FileStream fileStream = new FileStream(filename, FileMode.Create))
            {
                fileStream.Write(bytes, 0, bytes.Length);
            }

            MainViewModel.Instance.Reports.PeriodReports.Add(Path.GetFileName(filename));
        }

        static public void CreatePeriodReportSummary(string directory, PeriodViewModel period)
        {
            if (!string.IsNullOrEmpty(directory))
            {
                try
                {
                    foreach (ClassViewModel _class in MainViewModel.Instance.Parameters.Classes.OrderBy(c => c.Order))
                    {
                        ExportUtils.ExportPeriodSummary(directory, _class, period);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        static public void CreateSemiTrimesterReport(SemiTrimesterViewModel semiTrimester, MainWindow.UpdateSemiTrimesterReportsDelegate _updateSemiTrimesterReportsDispatch)
        {
            string directory = FileUtils.SelectDirectory(Settings.Default.LastSelectedDirectorySemiTrimesterReports, "LastSelectedDirectorySemiTrimesterReports");

            if (!string.IsNullOrEmpty(directory))
            {
                try
                {
                    MainViewModel.Instance.Reports.SemiTrimesterReports.Clear();

                    int classCount = 0;
                    foreach (ClassViewModel _class in MainViewModel.Instance.Parameters.Classes)
                    {
                        SSRSUtils_SemiTrimester SSRSUtils_SemiTrimester = new SSRSUtils_SemiTrimester();
                        foreach (SubjectViewModel subject in _class.Level.Subjects)
                        {
                            if (subject.ChildrenSubjects.Any())
                            {
                                double average = MarkModel.ReadSemiTrimesterClassMainSubjectAverage(semiTrimester, _class, subject);
                                SSRSUtils_SemiTrimester.ClassSubjectAverages.Add(subject, average);
                                MarkModel.ReadSemiTrimesterMinMaxMainSubjectAverage(semiTrimester, _class, subject, out double minAverage, out double maxAverage);
                                SSRSUtils_SemiTrimester.ClassSubjectMinAverages.Add(subject, minAverage);
                                SSRSUtils_SemiTrimester.ClassSubjectMaxAverages.Add(subject, maxAverage);
                                foreach (SubjectViewModel subject2 in subject.ChildrenSubjects.OrderBy(s => s.Order))
                                {
                                    average = MarkModel.ReadSemiTrimesterClassSubjectAverage(semiTrimester, _class, subject2);
                                    SSRSUtils_SemiTrimester.ClassSubjectAverages.Add(subject2, average);
                                    MarkModel.ReadSemiTrimesterMinMaxSubjectAverage(semiTrimester, _class, subject2, out minAverage, out maxAverage);
                                    SSRSUtils_SemiTrimester.ClassSubjectMinAverages.Add(subject2, minAverage);
                                    SSRSUtils_SemiTrimester.ClassSubjectMaxAverages.Add(subject2, maxAverage);
                                }
                            }
                            else
                            {
                                double average = MarkModel.ReadSemiTrimesterClassSubjectAverage(semiTrimester, _class, subject);
                                SSRSUtils_SemiTrimester.ClassSubjectAverages.Add(subject, average);
                                MarkModel.ReadSemiTrimesterMinMaxSubjectAverage(semiTrimester, _class, subject, out double minAverage, out double maxAverage);
                                SSRSUtils_SemiTrimester.ClassSubjectMinAverages.Add(subject, minAverage);
                                SSRSUtils_SemiTrimester.ClassSubjectMaxAverages.Add(subject, maxAverage);
                            }
                        }
                        SSRSUtils_SemiTrimester.ClassAverage = MarkModel.ReadSemiTrimesterClassAverage(semiTrimester, _class);
                        MarkModel.ReadSemiTrimesterMinMaxAverage(semiTrimester, _class, out double classMinAverage, out double classMaxAverage);
                        SSRSUtils_SemiTrimester.ClassMinAverage = classMinAverage;
                        SSRSUtils_SemiTrimester.ClassMaxAverage = classMaxAverage;
                        foreach (StudentViewModel student in _class.Students)
                        {
                            double average = MarkModel.ReadSemiTrimesterAverage(semiTrimester, student);
                            if (average != double.MinValue)
                            {
                                SSRSUtils_SemiTrimester.StudentAverages[student] = average;
                            }
                        }
                        int studentCount = 0;
                        foreach (StudentViewModel student in SSRSUtils_SemiTrimester.StudentAverages.Keys)
                        {
                            try
                            {
                                GenerateSemiTrimesterReport(directory, semiTrimester, student, _class, SSRSUtils_SemiTrimester);
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show(e.Message);
                            }

                            studentCount++;
                            _updateSemiTrimesterReportsDispatch(studentCount * 1000 / SSRSUtils_SemiTrimester.StudentAverages.Count
                                + classCount * 1000 / SSRSUtils_SemiTrimester.StudentAverages.Count / MainViewModel.Instance.Parameters.Classes.Count);
                        }
                        classCount++;
                    }

                    MainViewModel.Instance.Reports.SemiTrimesterReportsPath = directory;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        private static void GenerateSemiTrimesterReport(string directory, SemiTrimesterViewModel semiTrimester, StudentViewModel student, ClassViewModel _class, SSRSUtils_SemiTrimester SSRSUtils_SemiTrimester)
        {
            string filename = Path.Combine(directory, $"Bulletin de demi-trimestre de {semiTrimester.Name} de {_class.Name} de {student.LastName} {student.FirstName}.pdf");

            ReportViewer report = new ReportViewer();
            report.LocalReport.ReportPath = @".\Reports\BulletinDemiTrimestre.rdlc";

            BulletinDemiTrimestreHeaderDataSource bulletinDemiTrimestreHeader = new BulletinDemiTrimestreHeaderDataSource()
            {
                BirthDate = $"né le {student.BirthDate.ToShortDateString()}",
                FirstName = student.FirstName,
                LastName = student.LastName,
                Header1 = $"Année {student.Year} - {student.Year + 1}",
                Header2 = $"Période du {semiTrimester.FromDate.ToShortDateString()} au {semiTrimester.ToDate.ToShortDateString()}",
                Header3 = $"Classe de {_class.Name}\tEffectif {_class.Students.Count}",                
            };

            SemiTrimesterCommentViewModel semiTrimesterComment = SemiTrimesterCommentModel.Read(semiTrimester, student);
            if (semiTrimesterComment != null)
            {
                bulletinDemiTrimestreHeader.MainTeacherReport = semiTrimesterComment.MainTeacherComment;
                bulletinDemiTrimestreHeader.DivisionPrefectReport = semiTrimesterComment.DivisionPrefectComment;
            }

            bulletinDemiTrimestreHeader.MainTeacherReportHeader = "Appréciation du professeur principal" +
                (_class.MainTeacher != null ? string.Format(" {0} {1} {2}", _class.MainTeacher.Title, !string.IsNullOrEmpty(_class.MainTeacher.FirstName) ? _class.MainTeacher.FirstName : "", _class.MainTeacher.LastName) : "");
            bulletinDemiTrimestreHeader.DivisionPrefectReportHeader = "Appréciation du préfet de division" +
                (!string.IsNullOrEmpty(MainViewModel.Instance.Parameters.YearParameters.DivisionPrefect) ? " " + MainViewModel.Instance.Parameters.YearParameters.DivisionPrefect : "");

            double average = MarkModel.ReadSemiTrimesterAverage(semiTrimester, student);
            if (average != double.MinValue)
            {
                bulletinDemiTrimestreHeader.Average = average.ToString("0.0");
            }
            if (SSRSUtils_SemiTrimester.ClassAverage != double.MinValue)
            {
                bulletinDemiTrimestreHeader.ClassAverage = SSRSUtils_SemiTrimester.ClassAverage.ToString("0.0");
            }
            if (SSRSUtils_SemiTrimester.ClassMinAverage != double.MaxValue)
            {
                bulletinDemiTrimestreHeader.ClassMinAverage = SSRSUtils_SemiTrimester.ClassMinAverage.ToString("0.0");
            }
            if (SSRSUtils_SemiTrimester.ClassMaxAverage != double.MinValue)
            {
                bulletinDemiTrimestreHeader.ClassMaxAverage = SSRSUtils_SemiTrimester.ClassMaxAverage.ToString("0.0");
            }
            int ranking = MarkModel.ReadSemiTrimesterRanking(student, SSRSUtils_SemiTrimester.StudentAverages);
            bulletinDemiTrimestreHeader.Ranking = $"{ranking}/{SSRSUtils_SemiTrimester.StudentAverages.Count}";

            List<BulletinDemiTrimestreLineDataSource> bulletinDemiTrimestreLines = new List<BulletinDemiTrimestreLineDataSource>();

            foreach (SubjectViewModel subject in _class.Level.Subjects.Where(s => s.ParentSubject == null))
            {
                BulletinDemiTrimestreLineDataSource bulletinDemiTrimestreLine = new BulletinDemiTrimestreLineDataSource()
                {
                    IsChildSubject = false,
                    Subject = subject.Name.ToUpper(),
                    ClassAverage = SSRSUtils_SemiTrimester.ClassSubjectAverages[subject] != double.MinValue ? SSRSUtils_SemiTrimester.ClassSubjectAverages[subject].ToString("0.0") : "",
                    ClassMinAverage = SSRSUtils_SemiTrimester.ClassSubjectMinAverages[subject] != double.MaxValue ? SSRSUtils_SemiTrimester.ClassSubjectMinAverages[subject].ToString("0.0") : "",
                    ClassMaxAverage = SSRSUtils_SemiTrimester.ClassSubjectMaxAverages[subject] != double.MinValue ? SSRSUtils_SemiTrimester.ClassSubjectMaxAverages[subject].ToString("0.0") : "",
                };
                if (subject.Option)
                {
                    bulletinDemiTrimestreLine.Coefficient = $"(option coeff. {subject.Coefficient})";
                }
                else
                {
                    bulletinDemiTrimestreLine.Coefficient = $"(coeff. {subject.Coefficient})";
                }
                TeacherViewModel teacher = ModelUtils.GetTeacherFromClassAndSubject(student.Class, subject);
                if (teacher != null)
                {
                    bulletinDemiTrimestreLine.Teacher = $"{teacher.Title} {(!string.IsNullOrEmpty(teacher.FirstName) ? teacher.FirstName.Substring(0, 1) : "")}. {teacher.LastName}";
                }
                average = double.MinValue;
                if (subject.ChildrenSubjects.Any())
                {
                    average = MarkModel.ReadSemiTrimesterMainSubjectAverage(semiTrimester, student, subject);
                }
                else
                {
                    average = MarkModel.ReadSemiTrimesterSubjectAverage(semiTrimester, student, subject);
                }
                if (average != double.MinValue)
                {
                    bulletinDemiTrimestreLine.Average = average.ToString("0.0");
                }
                bulletinDemiTrimestreLines.Add(bulletinDemiTrimestreLine);

                foreach (SubjectViewModel subject2 in subject.ChildrenSubjects.OrderBy(s => s.Order))
                {
                    bulletinDemiTrimestreLine = new BulletinDemiTrimestreLineDataSource()
                    {
                        IsChildSubject = true,
                        Subject = subject2.Name.ToUpper(),
                        ClassAverage = SSRSUtils_SemiTrimester.ClassSubjectAverages[subject2] != double.MinValue ? SSRSUtils_SemiTrimester.ClassSubjectAverages[subject2].ToString("0.0") : "",
                        ClassMinAverage = SSRSUtils_SemiTrimester.ClassSubjectMinAverages[subject2] != double.MaxValue ? SSRSUtils_SemiTrimester.ClassSubjectMinAverages[subject2].ToString("0.0") : "",
                        ClassMaxAverage = SSRSUtils_SemiTrimester.ClassSubjectMaxAverages[subject2] != double.MinValue ? SSRSUtils_SemiTrimester.ClassSubjectMaxAverages[subject2].ToString("0.0") : "",
                    };
                    if (subject.Option)
                    {
                        bulletinDemiTrimestreLine.Coefficient = $"(option coeff. {subject2.Coefficient})";
                    }
                    else
                    {
                        bulletinDemiTrimestreLine.Coefficient = $"(coeff. {subject2.Coefficient})";
                    }
                    average = MarkModel.ReadSemiTrimesterSubjectAverage(semiTrimester, student, subject2);
                    if (average != double.MinValue)
                    {
                        bulletinDemiTrimestreLine.Average = average.ToString("0.0");
                    }

                    bulletinDemiTrimestreLines.Add(bulletinDemiTrimestreLine);
                }
            }

            ReportDataSource dataSource = new ReportDataSource("HeaderDataSet", new List<BulletinDemiTrimestreHeaderDataSource>() { bulletinDemiTrimestreHeader });
            report.LocalReport.DataSources.Add(dataSource);

            dataSource = new ReportDataSource("LineDataSet", bulletinDemiTrimestreLines);
            report.LocalReport.DataSources.Add(dataSource);

            byte[] bytes = report.LocalReport.Render("PDF", null, out string mimeType, out string encoding, out string extension, out string[] streamIds, out Warning[] warnings);

            using (FileStream fileStream = new FileStream(filename, FileMode.Create))
            {
                fileStream.Write(bytes, 0, bytes.Length);
            }

            MainViewModel.Instance.Reports.SemiTrimesterReports.Add(Path.GetFileName(filename));
        }

        static public void CreateTrimesterReport(int trimester, MainWindow.UpdateTrimesterReportsDelegate _updateTrimesterReportsDispatch)
        {
            string directory = FileUtils.SelectDirectory(Settings.Default.LastSelectedDirectoryTrimesterReports, "LastSelectedDirectoryTrimesterReports");

            if (!string.IsNullOrEmpty(directory))
            {
                try
                {
                    MainViewModel.Instance.Reports.TrimesterReports.Clear();

                    int classCount = 0;
                    foreach (ClassViewModel _class in MainViewModel.Instance.Parameters.Classes)
                    {
                        SSRSUtils_Trimester SSRSUtils_Trimester = new SSRSUtils_Trimester();
                        foreach (SubjectViewModel subject in _class.Level.Subjects)
                        {
                            if (subject.ChildrenSubjects.Any())
                            {
                                double average = MarkModel.ReadTrimesterClassMainSubjectAverage(trimester, _class, subject);
                                SSRSUtils_Trimester.ClassSubjectAverages.Add(subject, average);
                                MarkModel.ReadTrimesterMinMaxMainSubjectAverage(trimester, _class, subject, out double minAverage, out double maxAverage);
                                SSRSUtils_Trimester.ClassSubjectMinAverages.Add(subject, minAverage);
                                SSRSUtils_Trimester.ClassSubjectMaxAverages.Add(subject, maxAverage);
                                foreach (SubjectViewModel subject2 in subject.ChildrenSubjects.OrderBy(s => s.Order))
                                {
                                    average = MarkModel.ReadTrimesterClassSubjectAverage(trimester, _class, subject2);
                                    SSRSUtils_Trimester.ClassSubjectAverages.Add(subject2, average);
                                    MarkModel.ReadTrimesterMinMaxSubjectAverage(trimester, _class, subject2, out minAverage, out maxAverage);
                                    SSRSUtils_Trimester.ClassSubjectMinAverages.Add(subject2, minAverage);
                                    SSRSUtils_Trimester.ClassSubjectMaxAverages.Add(subject2, maxAverage);
                                }
                            }
                            else
                            {
                                double average = MarkModel.ReadTrimesterClassSubjectAverage(trimester, _class, subject);
                                SSRSUtils_Trimester.ClassSubjectAverages.Add(subject, average);
                                MarkModel.ReadTrimesterMinMaxSubjectAverage(trimester, _class, subject, out double minAverage, out double maxAverage);
                                SSRSUtils_Trimester.ClassSubjectMinAverages.Add(subject, minAverage);
                                SSRSUtils_Trimester.ClassSubjectMaxAverages.Add(subject, maxAverage);
                            }
                        }
                        SSRSUtils_Trimester.ClassAverage = MarkModel.ReadTrimesterClassAverage(trimester, _class);
                        MarkModel.ReadTrimesterMinMaxAverage(trimester, _class, out double classMinAverage, out double classMaxAverage);
                        SSRSUtils_Trimester.ClassMinAverage = classMinAverage;
                        SSRSUtils_Trimester.ClassMaxAverage = classMaxAverage;
                        foreach (StudentViewModel student in _class.Students)
                        {
                            SSRSUtils_Trimester.StudentAverages[student] = MarkModel.ReadTrimesterAverage(trimester, student);
                        }
                        int studentCount = 0;
                        foreach (StudentViewModel student in _class.Students.Where(s => SSRSUtils_Trimester.StudentAverages[s] != double.MinValue))
                        {
                            try
                            {
                                GenerateTrimesterReport(directory, trimester, student, _class, SSRSUtils_Trimester);
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show(e.Message);
                            }

                            studentCount++;
                            _updateTrimesterReportsDispatch(studentCount * 1000 / SSRSUtils_Trimester.StudentAverages.Count
                                + classCount * 1000 / SSRSUtils_Trimester.StudentAverages.Count / MainViewModel.Instance.Parameters.Classes.Count);
                        }
                    }

                    MainViewModel.Instance.Reports.TrimesterReportsPath = directory;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        private static void GenerateTrimesterReport(string directory, int trimester, StudentViewModel student, ClassViewModel _class, SSRSUtils_Trimester SSRSUtils_Trimester)
        {
            string filename = Path.Combine(directory, $"Bulletin de trimestre {trimester} de {_class.Name} de {student.LastName} {student.FirstName}.pdf");

            ReportViewer report = new ReportViewer();
            report.LocalReport.ReportPath = @".\Reports\BulletinTrimestre.rdlc";

            BulletinTrimestreHeaderDataSource bulletinTrimestreHeader = new BulletinTrimestreHeaderDataSource()
            {
                BirthDate = $"né le {student.BirthDate.ToShortDateString()}",
                FirstName = student.FirstName,
                LastName = student.LastName,
                Header1 = $"DU {NumberUtils.GetRankString(trimester)} TRIMESTRE",
                Header2 = $"Année {student.Year} - {student.Year + 1}",
                Header3 = $"Classe de {_class.Name}\tEffectif {_class.Students.Count}",
            };

            TrimesterCommentViewModel trimesterComment = TrimesterCommentModel.Read(trimester, student);
            if (trimesterComment != null)
            {
                bulletinTrimestreHeader.MainTeacherReport = trimesterComment.MainTeacherComment;
                bulletinTrimestreHeader.DivisionPrefectReport = trimesterComment.DivisionPrefectComment;
            }

            bulletinTrimestreHeader.MainTeacherReportHeader = "Appréciation du professeur principal" +
                (_class.MainTeacher != null ? $" {_class.MainTeacher.Title} {(!string.IsNullOrEmpty(_class.MainTeacher.FirstName) ? _class.MainTeacher.FirstName : "")} {_class.MainTeacher.LastName}" : "");
            bulletinTrimestreHeader.DivisionPrefectReportHeader = "Appréciation du préfet de division" +
                (!string.IsNullOrEmpty(MainViewModel.Instance.Parameters.YearParameters.DivisionPrefect) ? " " + MainViewModel.Instance.Parameters.YearParameters.DivisionPrefect : "");

            double average = MarkModel.ReadTrimesterAverage(trimester, student);
            if (average != double.MinValue)
            {
                bulletinTrimestreHeader.Average = average.ToString("0.0");
            }
            if (SSRSUtils_Trimester.ClassAverage != double.MinValue)
            {
                bulletinTrimestreHeader.ClassAverage = SSRSUtils_Trimester.ClassAverage.ToString("0.0");
            }
            if (SSRSUtils_Trimester.ClassMinAverage != double.MaxValue)
            {
                bulletinTrimestreHeader.ClassMinAverage = SSRSUtils_Trimester.ClassMinAverage.ToString("0.0");
            }
            if (SSRSUtils_Trimester.ClassMaxAverage != double.MinValue)
            {
                bulletinTrimestreHeader.ClassMaxAverage = SSRSUtils_Trimester.ClassMaxAverage.ToString("0.0");
            }
            int ranking = MarkModel.ReadtrimesterRanking(student, SSRSUtils_Trimester.StudentAverages);
            bulletinTrimestreHeader.Ranking = $"{ranking}/{SSRSUtils_Trimester.StudentAverages.Count}";

            List<BulletinTrimestreLineDataSource> bulletinTrimestreLines = new List<BulletinTrimestreLineDataSource>();

            foreach (SubjectViewModel subject in _class.Level.Subjects.Where(s => s.ParentSubject == null))
            {
                BulletinTrimestreLineDataSource bulletinTrimestreLine = new BulletinTrimestreLineDataSource()
                {
                    IsChildSubject = false,
                    Subject = subject.Name.ToUpper(),
                    ClassAverage = SSRSUtils_Trimester.ClassSubjectAverages[subject] != double.MinValue ? SSRSUtils_Trimester.ClassSubjectAverages[subject].ToString("0.0") : "",
                    ClassMinAverage = SSRSUtils_Trimester.ClassSubjectMinAverages[subject] != double.MaxValue ? SSRSUtils_Trimester.ClassSubjectMinAverages[subject].ToString("0.0") : "",
                    ClassMaxAverage = SSRSUtils_Trimester.ClassSubjectMaxAverages[subject] != double.MinValue ? SSRSUtils_Trimester.ClassSubjectMaxAverages[subject].ToString("0.0") : "",
                };
                if (subject.Option)
                {
                    bulletinTrimestreLine.Coefficient = $"(option coeff. {subject.Coefficient})";
                }
                else
                {
                    bulletinTrimestreLine.Coefficient = $"(coeff. {subject.Coefficient})";
                }
                TeacherViewModel teacher = ModelUtils.GetTeacherFromClassAndSubject(student.Class, subject);
                if (teacher != null)
                {
                    bulletinTrimestreLine.Teacher = $"{teacher.Title} {(!string.IsNullOrEmpty(teacher.FirstName) ? teacher.FirstName.Substring(0, 1) : "")}. {teacher.LastName}";
                }
                average = double.MinValue;
                if (subject.ChildrenSubjects.Any())
                {
                    average = MarkModel.ReadTrimesterMainSubjectAverage(trimester, student, subject);
                }
                else
                {
                    average = MarkModel.ReadTrimesterSubjectAverage(trimester, student, subject);
                }
                if (average != double.MinValue)
                {
                    bulletinTrimestreLine.Average = average.ToString("0.0");
                }
                TrimesterSubjectCommentViewModel trimesterSubjectComment = TrimesterSubjectCommentModel.Read(trimester, student, subject);
                if (trimesterSubjectComment != null)
                {
                    bulletinTrimestreLine.SubjectComment = trimesterSubjectComment.Comment;
                }
                bulletinTrimestreLines.Add(bulletinTrimestreLine);

                foreach (SubjectViewModel subject2 in subject.ChildrenSubjects.OrderBy(s => s.Order))
                {
                    bulletinTrimestreLine = new BulletinTrimestreLineDataSource()
                    {
                        IsChildSubject = true,
                        Subject = subject2.Name.ToUpper(),
                        ClassAverage = SSRSUtils_Trimester.ClassSubjectAverages[subject2] != double.MinValue ? SSRSUtils_Trimester.ClassSubjectAverages[subject2].ToString("0.0") : "",
                        ClassMinAverage = SSRSUtils_Trimester.ClassSubjectMinAverages[subject2] != double.MaxValue ? SSRSUtils_Trimester.ClassSubjectMinAverages[subject2].ToString("0.0") : "",
                        ClassMaxAverage = SSRSUtils_Trimester.ClassSubjectMaxAverages[subject2] != double.MinValue ? SSRSUtils_Trimester.ClassSubjectMaxAverages[subject2].ToString("0.0") : "",
                    };
                    if (subject.Option)
                    {
                        bulletinTrimestreLine.Coefficient = $"(option coeff. {subject2.Coefficient})";
                    }
                    else
                    {
                        bulletinTrimestreLine.Coefficient = $"(coeff. {subject2.Coefficient})";
                    }
                    average = MarkModel.ReadTrimesterSubjectAverage(trimester, student, subject2);
                    if (average != double.MinValue)
                    {
                        bulletinTrimestreLine.Average = average.ToString("0.0");
                    }
                    trimesterSubjectComment = TrimesterSubjectCommentModel.Read(trimester, student, subject2);
                    if (trimesterSubjectComment != null)
                    {
                        bulletinTrimestreLine.SubjectComment = trimesterSubjectComment.Comment;
                    }

                    bulletinTrimestreLines.Add(bulletinTrimestreLine);
                }
            }

            ReportDataSource dataSource = new ReportDataSource("HeaderDataSet", new List<BulletinTrimestreHeaderDataSource>() { bulletinTrimestreHeader });
            report.LocalReport.DataSources.Add(dataSource);

            dataSource = new ReportDataSource("LineDataSet", bulletinTrimestreLines);
            report.LocalReport.DataSources.Add(dataSource);

            byte[] bytes = report.LocalReport.Render("PDF", null, out string mimeType, out string encoding, out string extension, out string[] streamIds, out Warning[] warnings);

            using (FileStream fileStream = new FileStream(filename, FileMode.Create))
            {
                fileStream.Write(bytes, 0, bytes.Length);
            }

            MainViewModel.Instance.Reports.TrimesterReports.Add(Path.GetFileName(filename));
        }

        static public void CreateYearReport(int year, MainWindow.UpdateYearReportsDelegate _updateYearReportsDispatch)
        {
            string directory = FileUtils.SelectDirectory(Settings.Default.LastSelectedDirectoryYearReports, "LastSelectedDirectoryYearReports");

            if (!string.IsNullOrEmpty(directory))
            {
                try
                {
                    MainViewModel.Instance.Reports.YearReports.Clear();

                    int classCount = 0;
                    foreach (ClassViewModel _class in MainViewModel.Instance.Parameters.Classes)
                    {
                        SSRSUtils_Year SSRSUtils_Year = new SSRSUtils_Year();
                        foreach (SubjectViewModel subject in _class.Level.Subjects)
                        {
                            if (subject.ChildrenSubjects.Any())
                            {
                                double average = MarkModel.ReadYearClassMainSubjectAverage(year, _class, subject);
                                SSRSUtils_Year.ClassSubjectAverages.Add(subject, average);
                                MarkModel.ReadYearMinMaxMainSubjectAverage(year, _class, subject, out double minAverage, out double maxAverage);
                                SSRSUtils_Year.ClassSubjectMinAverages.Add(subject, minAverage);
                                SSRSUtils_Year.ClassSubjectMaxAverages.Add(subject, maxAverage);
                                foreach (SubjectViewModel subject2 in subject.ChildrenSubjects.OrderBy(s => s.Order))
                                {
                                    average = MarkModel.ReadYearClassSubjectAverage(year, _class, subject2);
                                    SSRSUtils_Year.ClassSubjectAverages.Add(subject2, average);
                                    MarkModel.ReadYearMinMaxSubjectAverage(year, _class, subject2, out minAverage, out maxAverage);
                                    SSRSUtils_Year.ClassSubjectMinAverages.Add(subject2, minAverage);
                                    SSRSUtils_Year.ClassSubjectMaxAverages.Add(subject2, maxAverage);
                                }
                            }
                            else
                            {
                                double average = MarkModel.ReadYearClassSubjectAverage(year, _class, subject);
                                SSRSUtils_Year.ClassSubjectAverages.Add(subject, average);
                                MarkModel.ReadYearMinMaxSubjectAverage(year, _class, subject, out double minAverage, out double maxAverage);
                                SSRSUtils_Year.ClassSubjectMinAverages.Add(subject, minAverage);
                                SSRSUtils_Year.ClassSubjectMaxAverages.Add(subject, maxAverage);
                            }
                        }
                        SSRSUtils_Year.ClassAverage = MarkModel.ReadYearClassAverage(year, _class);
                        MarkModel.ReadYearMinMaxAverage(year, _class, out double classMinAverage, out double classMaxAverage);
                        SSRSUtils_Year.ClassMinAverage = classMinAverage;
                        SSRSUtils_Year.ClassMaxAverage = classMaxAverage;
                        foreach (StudentViewModel student in _class.Students)
                        {
                            SSRSUtils_Year.StudentAverages[student] = MarkModel.ReadYearAverage(year, student);
                        }
                        int studentCount = 0;
                        foreach (StudentViewModel student in _class.Students.Where(s => SSRSUtils_Year.StudentAverages[s] != double.MinValue))
                        {
                            try
                            {
                                GenerateYearReport(directory, year, student, _class, SSRSUtils_Year);
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show(e.Message);
                            }

                            studentCount++;
                            _updateYearReportsDispatch(studentCount * 1000 / SSRSUtils_Year.StudentAverages.Count
                                + classCount * 1000 / SSRSUtils_Year.StudentAverages.Count / MainViewModel.Instance.Parameters.Classes.Count);
                        }
                    }

                    MainViewModel.Instance.Reports.YearReportsPath = directory;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        private static void GenerateYearReport(string directory, int year, StudentViewModel student, ClassViewModel _class, SSRSUtils_Year SSRSUtils_Year)
        {
            string filename = Path.Combine(directory, $"Bulletin de l'année {year}-{year + 1} de {_class.Name} de {student.LastName} {student.FirstName}.pdf");

            ReportViewer report = new ReportViewer();
            report.LocalReport.ReportPath = @".\Reports\BulletinAnnuel.rdlc";

            BulletinAnnuelHeaderDataSource bulletinAnnuelHeader = new BulletinAnnuelHeaderDataSource()
            {
                BirthDate = $"né le {student.BirthDate.ToShortDateString()}",
                FirstName = student.FirstName,
                LastName = student.LastName,
                Header1 = $"Année {student.Year} - {student.Year + 1}",
                Header2 = $"Classe de {_class.Name}\tEffectif {_class.Students.Count}",
            };

            double average = MarkModel.ReadYearAverage(year, student);
            if (average != double.MinValue)
            {
                bulletinAnnuelHeader.Average = average.ToString("0.0");
            }
            if (SSRSUtils_Year.ClassAverage != double.MinValue)
            {
                bulletinAnnuelHeader.ClassAverage = SSRSUtils_Year.ClassAverage.ToString("0.0");
            }
            if (SSRSUtils_Year.ClassMinAverage != double.MaxValue)
            {
                bulletinAnnuelHeader.ClassMinAverage = SSRSUtils_Year.ClassMinAverage.ToString("0.0");
            }
            if (SSRSUtils_Year.ClassMaxAverage != double.MinValue)
            {
                bulletinAnnuelHeader.ClassMaxAverage = SSRSUtils_Year.ClassMaxAverage.ToString("0.0");
            }
            int ranking = MarkModel.ReadYearRanking(student, SSRSUtils_Year.StudentAverages);
            bulletinAnnuelHeader.Ranking = $"{ranking}/{SSRSUtils_Year.StudentAverages.Count}";

            List<BulletinAnnuelLineDataSource> bulletinAnnuelLines = new List<BulletinAnnuelLineDataSource>();

            foreach (SubjectViewModel subject in _class.Level.Subjects.Where(s => s.ParentSubject == null))
            {
                BulletinAnnuelLineDataSource bulletinAnnuelLine = new BulletinAnnuelLineDataSource()
                {
                    IsChildSubject = false,
                    Subject = subject.Name.ToUpper(),
                    ClassAverage = SSRSUtils_Year.ClassSubjectAverages[subject] != double.MinValue ? SSRSUtils_Year.ClassSubjectAverages[subject].ToString("0.0") : "",
                    ClassMinAverage = SSRSUtils_Year.ClassSubjectMinAverages[subject] != double.MaxValue ? SSRSUtils_Year.ClassSubjectMinAverages[subject].ToString("0.0") : "",
                    ClassMaxAverage = SSRSUtils_Year.ClassSubjectMaxAverages[subject] != double.MinValue ? SSRSUtils_Year.ClassSubjectMaxAverages[subject].ToString("0.0") : "",
                };
                if (subject.Option)
                {
                    bulletinAnnuelLine.Coefficient = $"(option coeff. {subject.Coefficient})";
                }
                else
                {
                    bulletinAnnuelLine.Coefficient = $"(coeff. {subject.Coefficient})";
                }
                TeacherViewModel teacher = ModelUtils.GetTeacherFromClassAndSubject(student.Class, subject);
                if (teacher != null)
                {
                    bulletinAnnuelLine.Teacher = $"{teacher.Title} {(!string.IsNullOrEmpty(teacher.FirstName) ? teacher.FirstName.Substring(0, 1) : "")}. {teacher.LastName}";
                }
                average = double.MinValue;
                if (subject.ChildrenSubjects.Any())
                {
                    average = MarkModel.ReadYearMainSubjectAverage(year, student, subject);
                }
                else
                {
                    average = MarkModel.ReadYearSubjectAverage(year, student, subject);
                }
                if (average != double.MinValue)
                {
                    bulletinAnnuelLine.Average = average.ToString("0.0");
                }
                if (subject.ChildrenSubjects.Any())
                {
                    average = MarkModel.ReadTrimesterMainSubjectAverage(1, student, subject);
                }
                else
                {
                    average = MarkModel.ReadTrimesterSubjectAverage(1, student, subject);
                }
                if (average != double.MinValue)
                {
                    bulletinAnnuelLine.Marks1 = average.ToString("0.0");
                }
                if (subject.ChildrenSubjects.Any())
                {
                    average = MarkModel.ReadTrimesterMainSubjectAverage(2, student, subject);
                }
                else
                {
                    average = MarkModel.ReadTrimesterSubjectAverage(2, student, subject);
                }
                if (average != double.MinValue)
                {
                    bulletinAnnuelLine.Marks2 = average.ToString("0.0");
                }
                if (subject.ChildrenSubjects.Any())
                {
                    average = MarkModel.ReadTrimesterMainSubjectAverage(3, student, subject);
                }
                else
                {
                    average = MarkModel.ReadTrimesterSubjectAverage(3, student, subject);
                }
                if (average != double.MinValue)
                {
                    bulletinAnnuelLine.Marks3 = average.ToString("0.0");
                }
                bulletinAnnuelLines.Add(bulletinAnnuelLine);

                foreach (SubjectViewModel subject2 in subject.ChildrenSubjects.OrderBy(s => s.Order))
                {
                    bulletinAnnuelLine = new BulletinAnnuelLineDataSource()
                    {
                        IsChildSubject = true,
                        Subject = subject2.Name.ToUpper(),
                        ClassAverage = SSRSUtils_Year.ClassSubjectAverages[subject2] != double.MinValue ? SSRSUtils_Year.ClassSubjectAverages[subject2].ToString("0.0") : "",
                        ClassMinAverage = SSRSUtils_Year.ClassSubjectMinAverages[subject2] != double.MaxValue ? SSRSUtils_Year.ClassSubjectMinAverages[subject2].ToString("0.0") : "",
                        ClassMaxAverage = SSRSUtils_Year.ClassSubjectMaxAverages[subject2] != double.MinValue ? SSRSUtils_Year.ClassSubjectMaxAverages[subject2].ToString("0.0") : "",
                    };
                    if (subject.Option)
                    {
                        bulletinAnnuelLine.Coefficient = $"(option coeff. {subject2.Coefficient})";
                    }
                    else
                    {
                        bulletinAnnuelLine.Coefficient = $"(coeff. {subject2.Coefficient})";
                    }
                    average = MarkModel.ReadYearSubjectAverage(year, student, subject2);
                    if (average != double.MinValue)
                    {
                        bulletinAnnuelLine.Average = average.ToString("0.0");
                    }
                    average = MarkModel.ReadTrimesterSubjectAverage(1, student, subject2);
                    if (average != double.MinValue)
                    {
                        bulletinAnnuelLine.Marks1 = average.ToString("0.0");
                    }
                    average = MarkModel.ReadTrimesterSubjectAverage(2, student, subject2);
                    if (average != double.MinValue)
                    {
                        bulletinAnnuelLine.Marks2 = average.ToString("0.0");
                    }
                    average = MarkModel.ReadTrimesterSubjectAverage(3, student, subject2);
                    if (average != double.MinValue)
                    {
                        bulletinAnnuelLine.Marks3 = average.ToString("0.0");
                    }

                    bulletinAnnuelLines.Add(bulletinAnnuelLine);
                }
            }

            average = MarkModel.ReadTrimesterAverage(1, student);
            if (average != double.MinValue)
            {
                bulletinAnnuelHeader.Average1 = average.ToString("0.0");
            }
            average = MarkModel.ReadTrimesterAverage(2, student);
            if (average != double.MinValue)
            {
                bulletinAnnuelHeader.Average2 = average.ToString("0.0");
            }
            average = MarkModel.ReadTrimesterAverage(3, student);
            if (average != double.MinValue)
            {
                bulletinAnnuelHeader.Average3 = average.ToString("0.0");
            }

            ReportDataSource dataSource = new ReportDataSource("HeaderDataSet", new List<BulletinAnnuelHeaderDataSource>() { bulletinAnnuelHeader });
            report.LocalReport.DataSources.Add(dataSource);

            dataSource = new ReportDataSource("LineDataSet", bulletinAnnuelLines);
            report.LocalReport.DataSources.Add(dataSource);

            byte[] bytes = report.LocalReport.Render("PDF", null, out string mimeType, out string encoding, out string extension, out string[] streamIds, out Warning[] warnings);

            using (FileStream fileStream = new FileStream(filename, FileMode.Create))
            {
                fileStream.Write(bytes, 0, bytes.Length);
            }

            MainViewModel.Instance.Reports.YearReports.Add(Path.GetFileName(filename));
        }
    }
}

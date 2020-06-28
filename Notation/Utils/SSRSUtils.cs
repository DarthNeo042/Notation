using Microsoft.Reporting.WebForms;
using Notation.Models;
using Notation.Reports;
using Notation.ViewModels;
using Notation.Views;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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

    public static class SSRSUtils
    {
        static public void CreatePeriodReport(PeriodViewModel period)
        {
            string directory = FileUtils.SelectDirectory();

            if (!string.IsNullOrEmpty(directory))
            {
                Progress progress = new Progress();
                try
                {
                    progress.Count = MainViewModel.Instance.Parameters.Students.Count;
                    progress.Show();

                    foreach (IGrouping<int, MarkViewModel> studentGroup in MarkModel.Read(MainViewModel.Instance.SelectedYear, period.Id).GroupBy(m => m.IdStudent))
                    {
                        StudentViewModel student = MainViewModel.Instance.Parameters.Students.FirstOrDefault(s => s.Id == studentGroup.Key);
                        if (student != null)
                        {
                            progress.Text = string.Format("{0} {1}", student.LastName, student.FirstName);

                            try
                            {
                                GeneratePeriodReport(studentGroup.ToList(), directory, student, period, student.Class);
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show(e.Message);
                            }
                        }
                    }

                    //CreatePeriodReportSummary(directory);

                    progress.Close();
                    Process.Start("explorer", string.Format("/root,{0}", directory));
                }
                catch (Exception e)
                {
                    progress.Close();
                    MessageBox.Show(e.Message);
                }
            }
        }

        static private void GeneratePeriodReport(IEnumerable<MarkViewModel> marks, string directory, StudentViewModel student, PeriodViewModel period, ClassViewModel _class)
        {
            string filename = Path.Combine(directory, string.Format("Bulletin de période {0} de {1} de {2} {3}.pdf", period.Number, _class.Name, student.LastName, student.FirstName));

            ReportViewer report = new ReportViewer();
            report.LocalReport.ReportPath = @".\Reports\BulletinPeriode.rdlc";

            BulletinPeriodeHeaderDataSource bulletinPeriodeHeader = new BulletinPeriodeHeaderDataSource()
            {
                BirthDate = string.Format("né le {0}", student.BirthDate.ToShortDateString()),
                FirstName = student.FirstName,
                LastName = student.LastName,
                Header1 = string.Format("Année {0} - {1}", student.Year, student.Year + 1),
                Header2 = string.Format("Période du {0} au {1}", period.FromDate.ToShortDateString(), period.ToDate.ToShortDateString()),
                Header3 = string.Format("Classe de {0}\tEffectif {1}", _class.Name, _class.Students.Count),
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
                    bulletinPeriodeLine.Coefficient = string.Format("(option coeff. {0})", subject.Coefficient);
                }
                else
                {
                    bulletinPeriodeLine.Coefficient = string.Format("(coeff. {0})", subject.Coefficient);
                }
                TeacherViewModel teacher = ModelUtils.GetTeacherFromClassAndSubject(student.Class, subject);
                if (teacher != null)
                {
                    bulletinPeriodeLine.Teacher = string.Format("{0} {1}. {2}", teacher.Title, !string.IsNullOrEmpty(teacher.FirstName) ? teacher.FirstName.Substring(0, 1) : "", teacher.LastName);
                }
                i = 1;
                foreach (int coefficient in coefficients)
                {
                    string marksStr = "";
                    foreach (MarkViewModel mark in marks.Where(m => m.IdSubject == subject.Id && m.Coefficient == coefficient))
                    {
                        if (!string.IsNullOrEmpty(marksStr))
                        {
                            marksStr += "\t";
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
                        bulletinPeriodeLine.Coefficient = string.Format("(option coeff. {0})", subject2.Coefficient);
                    }
                    else
                    {
                        bulletinPeriodeLine.Coefficient = string.Format("(coeff. {0})", subject2.Coefficient);
                    }

                    foreach (int coefficient in coefficients)
                    {
                        string marksStr = "";
                        foreach (MarkViewModel mark in marks.Where(m => m.IdSubject == subject2.Id && m.Coefficient == coefficient))
                        {
                            if (!string.IsNullOrEmpty(marksStr))
                            {
                                marksStr += "\t";
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
        }

        static public void CreateSemiTrimesterReport(SemiTrimesterViewModel semiTrimester)
        {
            string directory = FileUtils.SelectDirectory();

            if (!string.IsNullOrEmpty(directory))
            {
                Progress progress = new Progress();
                try
                {
                    progress.Count = MainViewModel.Instance.Parameters.Students.Count;
                    progress.Show();

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
                                foreach (SubjectViewModel subject2 in subject.ChildrenSubjects)
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
                            SSRSUtils_SemiTrimester.StudentAverages[student] = MarkModel.ReadSemiTrimesterAverage(semiTrimester, student);
                        }
                        foreach (StudentViewModel student in _class.Students)
                        {
                            progress.Text = string.Format("{0} {1}", student.LastName, student.FirstName);

                            try
                            {
                                GenerateSemiTrimesterReport(directory, semiTrimester, student, _class, SSRSUtils_SemiTrimester);
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show(e.Message);
                            }
                        }
                    }

                    //CreateSemiTrimesterReportSummary(directory);

                    progress.Close();
                    Process.Start("explorer", string.Format("/root,{0}", directory));
                }
                catch (Exception e)
                {
                    progress.Close();
                    MessageBox.Show(e.Message);
                }
            }
        }

        private static void GenerateSemiTrimesterReport(string directory, SemiTrimesterViewModel semiTrimester, StudentViewModel student, ClassViewModel _class, SSRSUtils_SemiTrimester SSRSUtils_SemiTrimester)
        {
            string filename = Path.Combine(directory, string.Format("Bulletin de demi-trimestre de {0} de {1} de {2} {3}.pdf", semiTrimester.Name, _class.Name, student.LastName, student.FirstName));

            ReportViewer report = new ReportViewer();
            report.LocalReport.ReportPath = @".\Reports\BulletinDemiTrimestre.rdlc";

            BulletinDemiTrimestreHeaderDataSource bulletinDemiTrimestreHeader = new BulletinDemiTrimestreHeaderDataSource()
            {
                BirthDate = string.Format("né le {0}", student.BirthDate.ToShortDateString()),
                FirstName = student.FirstName,
                LastName = student.LastName,
                Header1 = string.Format("Année {0} - {1}", student.Year, student.Year + 1),
                Header2 = string.Format("Période du {0} au {1}", semiTrimester.FromDate.ToShortDateString(), semiTrimester.ToDate.ToShortDateString()),
                Header3 = string.Format("Classe de {0}\tEffectif {1}", _class.Name, _class.Students.Count),                
            };

            SemiTrimesterCommentViewModel semiTrimesterComment = SemiTrimesterCommentModel.Read(semiTrimester, student);
            if (semiTrimesterComment != null)
            {
                bulletinDemiTrimestreHeader.MainTeacherReport = semiTrimesterComment.MainTeacherComment;
                bulletinDemiTrimestreHeader.DivisionPrefectReport = semiTrimesterComment.DivisionPrefectComment;
            }

            bulletinDemiTrimestreHeader.MainTeacherReportHeader = "Appréciation du professeur principal" +
                (_class.MainTeacher != null ? string.Format(" {0} {1} {2}", _class.MainTeacher.Title, !string.IsNullOrEmpty(_class.MainTeacher.FirstName) ? _class.MainTeacher.FirstName : "", _class.MainTeacher.LastName) : "");
            //bulletinDemiTrimestreHeader.DivisionPrefectReportHeader = "Appréciation du préfet de division" +
            //    (!string.IsNullOrEmpty(MainViewModel.Instance.Parameters.BaseParameters.) ? " " + MainViewModel.Singleton.Parameters.Parameters.DivisionPrefect : "");

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
                    bulletinDemiTrimestreLine.Coefficient = string.Format("(option coeff. {0})", subject.Coefficient);
                }
                else
                {
                    bulletinDemiTrimestreLine.Coefficient = string.Format("(coeff. {0})", subject.Coefficient);
                }
                TeacherViewModel teacher = ModelUtils.GetTeacherFromClassAndSubject(student.Class, subject);
                if (teacher != null)
                {
                    bulletinDemiTrimestreLine.Teacher = string.Format("{0} {1}. {2}", teacher.Title, !string.IsNullOrEmpty(teacher.FirstName) ? teacher.FirstName.Substring(0, 1) : "", teacher.LastName);
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

                foreach (SubjectViewModel subject2 in subject.ChildrenSubjects)
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
                        bulletinDemiTrimestreLine.Coefficient = string.Format("(option coeff. {0})", subject2.Coefficient);
                    }
                    else
                    {
                        bulletinDemiTrimestreLine.Coefficient = string.Format("(coeff. {0})", subject2.Coefficient);
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
        }

        static public void CreateTrimesterReport(int trimester)
        {
            string directory = FileUtils.SelectDirectory();

            if (!string.IsNullOrEmpty(directory))
            {
                Progress progress = new Progress();
                try
                {
                    progress.Count = MainViewModel.Instance.Parameters.Students.Count;
                    progress.Show();

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
                                foreach (SubjectViewModel subject2 in subject.ChildrenSubjects)
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
                        foreach (StudentViewModel student in _class.Students)
                        {
                            progress.Text = string.Format("{0} {1}", student.LastName, student.FirstName);

                            try
                            {
                                GenerateTrimesterReport(directory, trimester, student, _class, SSRSUtils_Trimester);
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show(e.Message);
                            }
                        }
                    }

                    //CreateSemiTrimesterReportSummary(directory);

                    progress.Close();
                    Process.Start("explorer", string.Format("/root,{0}", directory));
                }
                catch (Exception e)
                {
                    progress.Close();
                    MessageBox.Show(e.Message);
                }
            }
        }

        private static void GenerateTrimesterReport(string directory, int trimester, StudentViewModel student, ClassViewModel _class, SSRSUtils_Trimester SSRSUtils_Trimester)
        {
            string filename = Path.Combine(directory, string.Format("Bulletin de trimestre {0} de {1} de {2} {3}.pdf", trimester, _class.Name, student.LastName, student.FirstName));

            ReportViewer report = new ReportViewer();
            report.LocalReport.ReportPath = @".\Reports\BulletinTrimestre.rdlc";

            BulletinDemiTrimestreHeaderDataSource bulletinDemiTrimestreHeader = new BulletinDemiTrimestreHeaderDataSource()
            {
                BirthDate = string.Format("né le {0}", student.BirthDate.ToShortDateString()),
                FirstName = student.FirstName,
                LastName = student.LastName,
                Header1 = string.Format("DU {0} TRIMESTRE", NumberUtils.GetRankString(trimester)),
                Header2 = string.Format("Année {0} - {1}", student.Year, student.Year + 1),
                Header3 = string.Format("Classe de {0}\tEffectif {1}", _class.Name, _class.Students.Count),
            };

            TrimesterCommentViewModel trimesterComment = TrimesterCommentModel.Read(trimester, student);
            if (trimesterComment != null)
            {
                bulletinDemiTrimestreHeader.MainTeacherReport = trimesterComment.MainTeacherComment;
                bulletinDemiTrimestreHeader.DivisionPrefectReport = trimesterComment.DivisionPrefectComment;
            }

            bulletinDemiTrimestreHeader.MainTeacherReportHeader = "Appréciation du professeur principal" +
                (_class.MainTeacher != null ? string.Format(" {0} {1} {2}", _class.MainTeacher.Title, !string.IsNullOrEmpty(_class.MainTeacher.FirstName) ? _class.MainTeacher.FirstName : "", _class.MainTeacher.LastName) : "");
            //bulletinDemiTrimestreHeader.DivisionPrefectReportHeader = "Appréciation du préfet de division" +
            //    (!string.IsNullOrEmpty(MainViewModel.Instance.Parameters.BaseParameters.) ? " " + MainViewModel.Singleton.Parameters.Parameters.DivisionPrefect : "");

            double average = MarkModel.ReadTrimesterAverage(trimester, student);
            if (average != double.MinValue)
            {
                bulletinDemiTrimestreHeader.Average = average.ToString("0.0");
            }
            if (SSRSUtils_Trimester.ClassAverage != double.MinValue)
            {
                bulletinDemiTrimestreHeader.ClassAverage = SSRSUtils_Trimester.ClassAverage.ToString("0.0");
            }
            if (SSRSUtils_Trimester.ClassMinAverage != double.MaxValue)
            {
                bulletinDemiTrimestreHeader.ClassMinAverage = SSRSUtils_Trimester.ClassMinAverage.ToString("0.0");
            }
            if (SSRSUtils_Trimester.ClassMaxAverage != double.MinValue)
            {
                bulletinDemiTrimestreHeader.ClassMaxAverage = SSRSUtils_Trimester.ClassMaxAverage.ToString("0.0");
            }
            int ranking = MarkModel.ReadtrimesterRanking(student, SSRSUtils_Trimester.StudentAverages);
            bulletinDemiTrimestreHeader.Ranking = $"{ranking}/{SSRSUtils_Trimester.StudentAverages.Count}";

            List<BulletinDemiTrimestreLineDataSource> bulletinDemiTrimestreLines = new List<BulletinDemiTrimestreLineDataSource>();

            foreach (SubjectViewModel subject in _class.Level.Subjects.Where(s => s.ParentSubject == null))
            {
                BulletinDemiTrimestreLineDataSource bulletinDemiTrimestreLine = new BulletinDemiTrimestreLineDataSource()
                {
                    IsChildSubject = false,
                    Subject = subject.Name.ToUpper(),
                    ClassAverage = SSRSUtils_Trimester.ClassSubjectAverages[subject] != double.MinValue ? SSRSUtils_Trimester.ClassSubjectAverages[subject].ToString("0.0") : "",
                    ClassMinAverage = SSRSUtils_Trimester.ClassSubjectMinAverages[subject] != double.MaxValue ? SSRSUtils_Trimester.ClassSubjectMinAverages[subject].ToString("0.0") : "",
                    ClassMaxAverage = SSRSUtils_Trimester.ClassSubjectMaxAverages[subject] != double.MinValue ? SSRSUtils_Trimester.ClassSubjectMaxAverages[subject].ToString("0.0") : "",
                };
                if (subject.Option)
                {
                    bulletinDemiTrimestreLine.Coefficient = string.Format("(option coeff. {0})", subject.Coefficient);
                }
                else
                {
                    bulletinDemiTrimestreLine.Coefficient = string.Format("(coeff. {0})", subject.Coefficient);
                }
                TeacherViewModel teacher = ModelUtils.GetTeacherFromClassAndSubject(student.Class, subject);
                if (teacher != null)
                {
                    bulletinDemiTrimestreLine.Teacher = string.Format("{0} {1}. {2}", teacher.Title, !string.IsNullOrEmpty(teacher.FirstName) ? teacher.FirstName.Substring(0, 1) : "", teacher.LastName);
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
                    bulletinDemiTrimestreLine.Average = average.ToString("0.0");
                }
                bulletinDemiTrimestreLines.Add(bulletinDemiTrimestreLine);

                foreach (SubjectViewModel subject2 in subject.ChildrenSubjects)
                {
                    bulletinDemiTrimestreLine = new BulletinDemiTrimestreLineDataSource()
                    {
                        IsChildSubject = true,
                        Subject = subject2.Name.ToUpper(),
                        ClassAverage = SSRSUtils_Trimester.ClassSubjectAverages[subject2] != double.MinValue ? SSRSUtils_Trimester.ClassSubjectAverages[subject2].ToString("0.0") : "",
                        ClassMinAverage = SSRSUtils_Trimester.ClassSubjectMinAverages[subject2] != double.MaxValue ? SSRSUtils_Trimester.ClassSubjectMinAverages[subject2].ToString("0.0") : "",
                        ClassMaxAverage = SSRSUtils_Trimester.ClassSubjectMaxAverages[subject2] != double.MinValue ? SSRSUtils_Trimester.ClassSubjectMaxAverages[subject2].ToString("0.0") : "",
                    };
                    if (subject.Option)
                    {
                        bulletinDemiTrimestreLine.Coefficient = string.Format("(option coeff. {0})", subject2.Coefficient);
                    }
                    else
                    {
                        bulletinDemiTrimestreLine.Coefficient = string.Format("(coeff. {0})", subject2.Coefficient);
                    }
                    average = MarkModel.ReadTrimesterSubjectAverage(trimester, student, subject2);
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
        }
    }
}

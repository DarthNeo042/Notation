using Notation.Models;
using Notation.ViewModels;
using Notation.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace Notation.Utils
{
    public class HTMLUtils_SemiTrimester
    {
        public ClassViewModel Class { get; set; }
        public double ClassAverage { get; set; }
        public double ClassMinAverage { get; set; }
        public double ClassMaxAverage { get; set; }
        public Dictionary<SubjectViewModel, double> ClassSubjectAverages { get; set; }
        public Dictionary<SubjectViewModel, double> ClassSubjectMinAverages { get; set; }
        public Dictionary<SubjectViewModel, double> ClassSubjectMaxAverages { get; set; }
        public Dictionary<StudentViewModel, double> StudentAverages { get; set; }

        public HTMLUtils_SemiTrimester()
        {
            ClassSubjectAverages = new Dictionary<SubjectViewModel, double>();
            ClassSubjectMinAverages = new Dictionary<SubjectViewModel, double>();
            ClassSubjectMaxAverages = new Dictionary<SubjectViewModel, double>();
            StudentAverages = new Dictionary<StudentViewModel, double>();
        }
    }

    public class HTMLUtils_Trimester
    {
        public ClassViewModel Class { get; set; }
        public double ClassAverage { get; set; }
        public double ClassMinAverage { get; set; }
        public double ClassMaxAverage { get; set; }
        public Dictionary<SubjectViewModel, double> ClassSubjectAverages { get; set; }
        public Dictionary<SubjectViewModel, double> ClassSubjectMinAverages { get; set; }
        public Dictionary<SubjectViewModel, double> ClassSubjectMaxAverages { get; set; }
        public Dictionary<StudentViewModel, double> StudentAverages { get; set; }

        public HTMLUtils_Trimester()
        {
            ClassSubjectAverages = new Dictionary<SubjectViewModel, double>();
            ClassSubjectMinAverages = new Dictionary<SubjectViewModel, double>();
            ClassSubjectMaxAverages = new Dictionary<SubjectViewModel, double>();
            StudentAverages = new Dictionary<StudentViewModel, double>();
        }
    }

    public class HTMLUtils_Year
    {
        public ClassViewModel Class { get; set; }
        public double ClassAverage { get; set; }
        public double ClassMinAverage { get; set; }
        public double ClassMaxAverage { get; set; }
        public Dictionary<SubjectViewModel, double> ClassSubjectAverages { get; set; }
        public Dictionary<SubjectViewModel, double> ClassSubjectMinAverages { get; set; }
        public Dictionary<SubjectViewModel, double> ClassSubjectMaxAverages { get; set; }
        public Dictionary<StudentViewModel, double> StudentAverages { get; set; }

        public HTMLUtils_Year()
        {
            ClassSubjectAverages = new Dictionary<SubjectViewModel, double>();
            ClassSubjectMinAverages = new Dictionary<SubjectViewModel, double>();
            ClassSubjectMaxAverages = new Dictionary<SubjectViewModel, double>();
            StudentAverages = new Dictionary<StudentViewModel, double>();
        }
    }
    public static class HTMLUtils
    {
        private const string T1 = "\t";
        private const string T2 = "\t\t";
        private const string T3 = "\t\t\t";
        private const string T4 = "\t\t\t\t";
        private const string T5 = "\t\t\t\t\t";
        private const string T6 = "\t\t\t\t\t\t";
        private const string T7 = "\t\t\t\t\t\t\t";
        private const string T8 = "\t\t\t\t\t\t\t\t";
        private const string T9 = "\t\t\t\t\t\t\t\t\t";
        private const string T10 = "\t\t\t\t\t\t\t\t\t\t";
        private const string T11 = "\t\t\t\t\t\t\t\t\t\t\t";

        static private void WriteHeader(string filename)
        {
            File.AppendAllText(filename,
                "<html>\r\n"
                + T1 + "<head>\r\n"
                + T2 + "<style>\r\n"
                + T3 + "page { width:19.4cm; height:28.3cm; margin:7mm 8mm 7mm 8mm; }\r\n"
                + T3 + "@media print { body { width:19.4cm; height:28.3cm; margin:7mm 8mm 7mm 8mm; } }\r\n"
                + T3 + "span { padding-left:0.5mm; padding-right:0.5mm; }\r\n"
                + T3 + "td { font-family:Cambria; }\r\n"
                + T3 + "td.Calibri17B_C { font-family:Calibri; font-size:17; text-align:center; font-weight:bold; }\r\n"
                + T3 + "td.Calibri14_L { font-family:Calibri; font-size:14; }\r\n"
                + T3 + "td.Calibri12_C { font-family:Calibri; font-size:12; text-align:center; }\r\n"
                + T3 + "td.Cambria27_C { font-size:27; text-align:center; }\r\n"
                + T3 + "td.Cambria21_R { font-size:21; text-align:right; }\r\n"
                + T3 + "td.Cambria19_C { font-size:19; text-align:center; }\r\n"
                + T3 + "td.Cambria16B_C { font-size:16; text-align:center; font-weight:bold; }\r\n"
                + T3 + "td.Cambria16_C { font-size:16; text-align:center; }\r\n"
                + T3 + "td.Cambria15_C { font-size:15; text-align:center; }\r\n"
                + T3 + "td.Cambria13_R { font-size:13; text-align:right; }\r\n"
                + T3 + "td.Cambria12BI_R { font-size:12; text-align:right; font-weight:bold; font-style:italic; }\r\n"
                + T3 + "td.Cambria12I_L { font-size:12; font-style:italic; }\r\n"
                + T3 + "td.Cambria12I_C { font-size:12; text-align:center; font-style:italic; }\r\n"
                + T3 + "td.Cambria12I_R { font-size:12; text-align:right; font-style:italic; }\r\n"
                + T3 + "td.Cambria10B_L { font-size:10; font-weight:bold; }\r\n"
                + T3 + "td.Cambria10B_C { font-size:10; text-align:center; font-weight:bold; }\r\n"
                + T3 + "span.Cambria12I_R { font-size:12; text-align:right; font-style:italic; }\r\n"
                + T3 + "span.Cambria10B_L { font-size:10; font-weight:bold; }\r\n"
                + T3 + "span.Cambria9I_L { font-size:9; font-style:italic; }\r\n"
                + T3 + "span.Cambria9I_R { font-size:9; text-align:right; font-style:italic; }\r\n"
                + T2 + "</style>\r\n"
                + T1 + "</head>\r\n"
                + T1 + "<body>\r\n");
        }

        static private void WriteFooter(string filename)
        {
            File.AppendAllText(filename,
                T2 + "<table style=\"height:6mm;width:194mm;margin-top:2mm;border-collapse:collapse\">\r\n"
                + T3 + "<tr>\r\n"
                + T4 + "<td class=\"Calibri12_C\">\r\n"
                + T5 + "Collège privé Saint-Martin - La Placelière - 44 690 CHÂTEAU-THÉBAUD - tél. 02 40 56 85 26 - mail : 44e.laplaceliere@fsspx.fr\r\n"
                + T4 + "</td>\r\n"
                + T3 + "</tr>\r\n"
                + T2 + "</table>\r\n"
                + T1 + "</body>\r\n"
                + "</html>");
        }

        static private void CopyIcone(string directory)
        {
            File.Copy(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "HTML", "Icon.png"),
                Path.Combine(directory, "Icon.png"), true);
        }

        static public void CreatePeriodReport(PeriodViewModel period, MainWindow.UpdatePeriodReportsDelegate _updatePeriodReportsDispatch)
        {
            string directory = FileUtils.SelectDirectory(Settings.Settings.Instance.LastSelectedDirectoryPeriodReports, "LastSelectedDirectoryPeriodReports");

            if (!string.IsNullOrEmpty(directory))
            {
                try
                {
                    CopyIcone(directory);

                    MainViewModel.Instance.Reports.PeriodReports.Clear();

                    int studentCount = 0;

                    Dictionary<int, string> students = new Dictionary<int, string>();
                    foreach (StudentViewModel student in MainViewModel.Instance.Parameters.Students)
                    {
                        students[student.Id] = $"{student.LastName}{student.FirstName}";
                    }

                    IEnumerable<IGrouping<int, MarkModel>> studentGroups = MarkModel.Read(MainViewModel.Instance.SelectedYear, period.Id).GroupBy(m => m.IdStudent);
                    foreach (IGrouping<int, MarkModel> studentGroup in studentGroups.Where(s => MainViewModel.Instance.Parameters.Students.Any(s2 => s2.Id == s.Key)).OrderBy(s => students[s.Key]))
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

        static private void GeneratePeriodReport(IEnumerable<MarkModel> marks, string directory, StudentViewModel student, PeriodViewModel period, ClassViewModel _class)
        {
            string filename = Path.Combine(directory, $"Bulletin de période {period.Number} de {_class.Name} de {student.LastName} {student.FirstName}.html");

            File.Delete(filename);
            WriteHeader(filename);

            File.AppendAllText(filename,
                T2 + "<table style=\"width:194mm;border-collapse:collapse\">\r\n"
                + T3 + "<tr>\r\n"
                + T4 + "<td>\r\n"
                + T5 + "<img src=\"icon.png\" style=\"width:35mm;height:35mm\">\r\n"
                + T4 + "</td>\r\n"
                + T4 + "<td style=\"width:10mm\">\r\n"
                + T4 + "</td>\r\n"
                + T4 + "<td style=\"width:104mm\">\r\n"
                + T5 + "<table width=\"100%\" style=\"border-collapse:collapse\">\r\n"
                + T6 + "<tr style=\"height:8.5mm\">\r\n"
                + T7 + "<td class=\"Cambria27_C\">\r\n"
                + T8 + "BULLETIN DE PÉRIODE\r\n"
                + T7 + "</td>\r\n"
                + T6 + "</tr>\r\n"
                + T6 + "<tr style=\"height:7mm\">\r\n"
                + T7 + "<td class=\"Cambria19_C\">\r\n"
                + T8 + $"Année {student.Year} - {student.Year + 1}\r\n"
                + T7 + "</td>\r\n"
                + T6 + "</tr>\r\n"
                + T6 + "<tr style=\"height:7mm\">\r\n"
                + T7 + "<td class=\"Cambria19_C\">\r\n"
                + T8 + $"Période du {period.FromDate.ToShortDateString()} au {period.ToDate.ToShortDateString()}\r\n"
                + T7 + "</td>\r\n"
                + T6 + "</tr>\r\n"
                + T6 + "<tr style=\"height:6mm\">\r\n"
                + T7 + "<td class=\"Cambria15_C\">\r\n"
                + T8 + $"Classe de {_class.Name} - Effectif {_class.Students.Count}\r\n"
                + T7 + "</td>\r\n"
                + T6 + "</tr>\r\n"
                + T6 + "<tr style=\"height:6.5mm\">\r\n"
                + T6 + "</tr>\r\n"
                + T5 + "</table>\r\n"
                + T4 + "</td>\r\n"
                + T4 + "<td style=\"width:45mm\">\r\n"
                + T5 + "<table width=\"100%\" style=\"border-collapse:collapse\">\r\n"
                + T6 + "<tr style=\"height:7.5mm\">\r\n"
                + T7 + "<td class=\"Cambria21_R\">\r\n"
                + T8 + $"{student.LastName}\r\n"
                + T7 + "</td>\r\n"
                + T6 + "</tr>\r\n"
                + T6 + "<tr style=\"height:7.5mm\">\r\n"
                + T7 + "<td class=\"Cambria21_R\">\r\n"
                + T8 + $"{student.FirstName}\r\n"
                + T7 + "</td>\r\n"
                + T6 + "</tr>\r\n"
                + T6 + "<tr style=\"height:5mm\">\r\n"
                + T7 + "<td class=\"Cambria13_R\">\r\n"
                + T8 + $"né le {student.BirthDate.ToShortDateString()}\r\n"
                + T7 + "</td>\r\n"
                + T6 + "</tr>\r\n"
                + T6 + "<tr style=\"height:15mm\">\r\n"
                + T6 + "</tr>\r\n"
                + T5 + "</table>\r\n"
                + T4 + "</td>\r\n"
                + T3 + "</tr>\r\n"
                + T2 + "</table>\r\n");

            File.AppendAllText(filename,
                 T2 + "<table style=\"border:1px solid black;margin-top:1mm;width:194mm\">\r\n"
                 + T3 + "<tr>\r\n"
                 + T4 + "<td>\r\n"
                 + T5 + $"<table style=\"width:192mm;border-collapse:collapse\">\r\n"
                 + T6 + "<tr style=\"height:5mm\">\r\n"
                 + T7 + "<td class=\"Cambria10B_L\" style=\"width:31mm;border:1px solid black\">\r\n"
                 + T8 + "<span>Disciplines</span>\r\n"
                 + T7 + "</td>\r\n");

            IEnumerable<int> coefficients = marks.Select(m => (int)m.Coefficient).Distinct().OrderBy(c => c);
            Dictionary<int, string> coefficientLibelles = new Dictionary<int, string>() { { 1, "Leçons" }, { 2, "Devoirs" }, { 4, "Examens" } };

            foreach (int coefficient in coefficients)
            {
                File.AppendAllText(filename,
                    T7 + "<td class=\"Cambria10B_C\" style=\"width:81mm;border:1px solid black\">\r\n"
                    + T8 + $"{(coefficientLibelles.ContainsKey(coefficient) ? coefficientLibelles[coefficient] + " - c" : "C")}oefficient {coefficient}\r\n"
                    + T7 + "</td>\r\n");
            }
            File.AppendAllText(filename,
                T6 + "</tr>\r\n");

            foreach (SubjectViewModel subject in _class.Level.Subjects.Where(s => s.ParentSubject == null).OrderBy(s => s.Order))
            {
                TeacherViewModel teacher = ModelUtils.GetTeacherFromClassAndSubject(student.Class, subject, period);
                File.AppendAllText(filename,
                    T6 + $"<tr style=\"height:11mm\">\r\n"
                    + T7 + "<td style=\"width:31mm;border:1px solid black\">\r\n"
                    + T8 + "<span class=\"Cambria10B_L\">\r\n"
                    + T9 + $"{subject.Name.ToUpper()}\r\n"
                    + T8 + "</span><br/>\r\n"
                    + T8 + "<span class=\"Cambria9I_L\">\r\n"
                    + T9 + $"({(subject.Option ? "option " : "")}coeff. {subject.Coefficient})\r\n"
                    + T8 + "</span><br/>\r\n"
                    + T8 + "<span class=\"Cambria9I_L\">\r\n"
                    + T9 + $"{(teacher != null ? $"{teacher.Title} {(!string.IsNullOrEmpty(teacher.FirstName) ? teacher.FirstName.Substring(0, 1) : "")}. {teacher.LastName}" : "")}\r\n"
                    + T8 + "</span><br/>\r\n"
                    + T7 + "</td>\r\n");
                foreach (int coefficient in coefficients)
                {
                    string marksStr = "";
                    foreach (MarkModel mark in marks.Where(m => m.IdSubject == subject.Id && m.Coefficient == coefficient))
                    {
                        if (!string.IsNullOrEmpty(marksStr))
                        {
                            marksStr += "&nbsp;&nbsp;&nbsp;&nbsp;";
                        }
                        marksStr += ((double)mark.Mark).ToString();
                    }
                    File.AppendAllText(filename, 
                        T7 + "<td class=\"Calibri14_L\" style=\"width:81mm;border:1px solid black\">\r\n"
                        + T8 + $"<span>{marksStr}</span>\r\n"
                        + T7 + "</td>\r\n");
                }
                File.AppendAllText(filename,
                    T6 + "</tr>\r\n");

                foreach (SubjectViewModel subject2 in subject.ChildrenSubjects.OrderBy(s => s.Order))
                {
                    File.AppendAllText(filename, 
                        T6 + "<tr style=\"height:8mm;border:1px solid black\">\r\n"
                        + T7 + "<td style=\"width:31mm;text-align:right;border:1px solid black\">\r\n"
                        + T8 + "<span class=\"Cambria12I_R\">\r\n"
                        + T9 + $"{subject2.Name.ToUpper()}\r\n"
                        + T8 + "</span><br/>\r\n"
                        + T8 + "<span class=\"Cambria9I_R\">\r\n"
                        + T9 + $"({(subject2.Option ? "option " : "")}coeff. {subject2.Coefficient})\r\n"
                        + T8 + "</span><br/>\r\n"
                        + T7 + "</td>\r\n");
                    foreach (int coefficient in coefficients)
                    {
                        string marksStr = "";
                        foreach (MarkModel mark in marks.Where(m => m.IdSubject == subject2.Id && m.Coefficient == coefficient))
                        {
                            if (!string.IsNullOrEmpty(marksStr))
                            {
                                marksStr += "&nbsp;&nbsp;&nbsp;&nbsp;";
                            }
                            marksStr += ((double)mark.Mark).ToString();
                        }
                        File.AppendAllText(filename,
                            T7 + "<td class=\"Calibri14_L\" style=\"width:81mm;border:1px solid black\">\r\n"
                            + T8 + $"<span>{marksStr}</span>\r\n"
                            + T7 + "</td>\r\n");
                    }
                    File.AppendAllText(filename, 
                        T6 + "</tr>\r\n");
                }
            }

            File.AppendAllText(filename, 
                T5 + "</table>\r\n"
                + T4 + "</td>\r\n"
                + T3 + "</tr>\r\n"
                + T2 + "</table>\r\n");

            double average = MarkModel.ReadPeriodTrimesterAverage(period, student);
            PeriodViewModel lastPeriod = ModelUtils.GetPreviousPeriod(period);
            double lastAverage = lastPeriod != null ? MarkModel.ReadPeriodTrimesterAverage(lastPeriod, student) : double.MinValue;

            File.AppendAllText(filename,
                T2 + "<table style=\"border:1px solid black;margin-top:2mm;margin-left:19mm;width:156mm\">\r\n"
                + T3 + "<tr>\r\n"
                + T4 + "<td>\r\n"
                + T5 + "<table style=\"border-collapse:collapse\">\r\n"
                + T6 + "<tr style=\"height:6.5mm\">\r\n"
                + T7 + "<td class=\"Cambria12BI_R\" style=\"width:129mm;border:1px solid black\">\r\n"
                + T8 + "<span>Moyenne des notes cumulées depuis le début du trimestre</span>\r\n"
                + T7 + "</td>\r\n"
                + T7 + "<td class=\"Cambria16B_C\" style=\"width:25mm;border:1px solid black\">\r\n"
                + T8 + $"{(average != double.MinValue ? average.ToString("0.0") : "")}\r\n"
                + T7 + "</td>\r\n"
                + T6 + "</tr>\r\n"
                + T6 + "<tr style=\"height:6.5mm\">\r\n"
                + T7 + "<td class=\"Cambria12BI_R\" style=\"width:129mm;border:1px solid black\">\r\n"
                + T8 + "<span>Rappel de la dernière moyenne obtenue</span>\r\n"
                + T7 + "</td>\r\n"
                + T7 + "<td class=\"Cambria16_C\" style=\"width:25mm;border:1px solid black\">\r\n"
                + T8 + $"{(lastAverage != double.MinValue ? lastAverage.ToString("0.0") : "")}\r\n"
                + T7 + "</td>\r\n"
                + T6 + "</tr>\r\n"
                + T6 + "<tr style=\"height:6.5mm\">\r\n"
                + T7 + "<td class=\"Cambria12BI_R\" style=\"width:129mm;border:1px solid black\">\r\n"
                + T8 + "<span>Tendance</span>\r\n"
                + T7 + "</td>\r\n"
                + T7 + "<td class=\"Calibri17B_C\" style=\"width:25mm;border:1px solid black\">\r\n"
                + $"{(lastPeriod != null ? (T8 + (average > lastAverage ? "↗" : average < lastAverage ? "↘" : "→")) : "")}\r\n"
                + T7 + "</td>\r\n"
                + T6 + "</tr>\r\n"
                + T5 + "</table>\r\n"
                + T4 + "</td>\r\n"
                + T3 + "</tr>\r\n"
                + T2 + "</table>\r\n");

            PeriodCommentModel periodComment = PeriodCommentModel.Read(period, student);

            File.AppendAllText(filename,
                T2 + "<table style=\"margin-top:2mm;border-collapse:collapse;width:194mm\">\r\n"
                + T3 + "<tr>\r\n"
                + T4 + "<td>\r\n"
                + T5 + "<table style=\"height:34.5mm;border:1px solid black\">\r\n"
                + T6 + "<tr>\r\n"
                + T7 + "<td style=\"height:32.5mm\">\r\n"
                + T8 + "<table style=\"height:32.5mm;border-collapse:collapse\">\r\n"
                + T9 + "<tr style=\"6.5mm;border:1px solid black\">\r\n"
                + T10 + "<td class=\"Cambria12BI_R\" style=\"width:63mm;border:1px solid black\">\r\n"
                + T11 + "<span>Compte-rendu des études</span>\r\n"
                + T10 + "</td>\r\n"
                + T10 + "<td style=\"width:10mm;border:1px solid black\">\r\n"
                + T10 + "</td>\r\n"
                + T9 + "</tr>\r\n"
                + T9 + "<tr style=\"height:6.5mm;border:1px solid black\">\r\n"
                + T10 + "<td class=\"Cambria12I_R\" style=\"width:63mm;border:1px solid black\">\r\n"
                + T11 + "<span>1er degré - bien</span>\r\n"
                + T10 + "</td>\r\n"
                + T10 + "<td class=\"Cambria12I_C\" style=\"width:10mm;border:1px solid black\">\r\n"
                + ((periodComment?.StudiesReport ?? 0) == 1 ? T11 + "X\r\n" : "")
                + T10 + "</td>\r\n"
                + T9 + "</tr>\r\n"
                + T9 + "<tr style=\"height:6.5mm;border:1px solid black\">\r\n"
                + T10 + "<td class=\"Cambria12I_R\" style=\"width:63mm;border:1px solid black\">\r\n"
                + T11 + "<span>2ème degré - doit progresser</span>\r\n"
                + T10 + "</td>\r\n"
                + T10 + "<td class=\"Cambria12I_C\" style=\"width:10mm;border:1px solid black\">\r\n"
                + ((periodComment?.StudiesReport ?? 0) == 2 ? T11 + "X\r\n" : "")
                + T10 + "</td>\r\n"
                + T9 + "</tr>\r\n"
                + T9 + "<tr style=\"height:6.5mm;border:1px solid black\">\r\n"
                + T10 + "<td class=\"Cambria12I_R\" style=\"width:63mm;border:1px solid black\">\r\n"
                + T11 + "<span>3ème degré - insuffisant</span>\r\n"
                + T10 + "</td>\r\n"
                + T10 + "<td class=\"Cambria12I_C\" style=\"width:10mm;border:1px solid black\">\r\n"
                + ((periodComment?.StudiesReport ?? 0) == 3 ? T11 + "X\r\n" : "")
                + T10 + "</td>\r\n"
                + T9 + "</tr>\r\n"
                + T9 + "<tr style=\"height:6.5mm;border:1px solid black\">\r\n"
                + T10 + "<td class=\"Cambria12I_R\" style=\"width:63mm;border:1px solid black\">\r\n"
                + T11 + "<span>Avertissement</span>\r\n"
                + T10 + "</td>\r\n"
                + T10 + "<td class=\"Cambria12I_C\" style=\"width:10mm;border:1px solid black\">\r\n"
                + ((periodComment?.StudiesReport ?? 0) == 4 ? T11 + "X\r\n" : "")
                + T10 + "</td>\r\n"
                + T9 + "</tr>\r\n"
                + T8 + "</table>\r\n"
                + T7 + "</td>\r\n"
                + T6 + "</tr>\r\n"
                + T5 + "</table>\r\n"
                + T4 + "</td>\r\n"
                + T4 + "<td style=\"width:1.5mm\">\r\n"
                + T4 + "</td>\r\n"
                + T4 + "<td>\r\n"
                + T5 + "<table style=\"height:34.5mm;border:1px solid black\">\r\n"
                + T6 + "<tr>\r\n"
                + T7 + "<td style=\"height:32.5mm;\" width=\"100%\">\r\n"
                + T8 + "<table style=\"height:32.5mm;border-collapse:collapse\">\r\n"
                + T9 + "<tr style=\"height:6.5mm;border:1px solid black\">\r\n"
                + T10 + "<td class=\"Cambria12BI_R\" style=\"width:63mm;border:1px solid black\">\r\n"
                + T11 + "<span>Compte-rendu de discipline</span>\r\n"
                + T10 + "</td>\r\n"
                + T10 + "<td style=\"width:10mm;border:1px solid black\">\r\n"
                + T10 + "</td>\r\n"
                + T9 + "</tr>\r\n"
                + T9 + "<tr style=\"height:6.5mm;border:1px solid black\">\r\n"
                + T10 + "<td class=\"Cambria12I_R\" style=\"width:63mm;border:1px solid black\">\r\n"
                + T11 + "<span>1er degré - bien</span>\r\n"
                + T10 + "</td>\r\n"
                + T10 + "<td class=\"Cambria12I_C\" style=\"width:10mm;border:1px solid black\">\r\n"
                + ((periodComment?.DisciplineReport ?? 0) == 1 ? T11 + "X\r\n" : "")
                + T10 + "</td>\r\n"
                + T9 + "</tr>\r\n"
                + T9 + "<tr style=\"height:6.5mm;border:1px solid black\">\r\n"
                + T10 + "<td class=\"Cambria12I_R\" style=\"width:63mm;border:1px solid black\">\r\n"
                + T11 + "<span>2ème degré - doit progresser</span>\r\n"
                + T10 + "</td>\r\n"
                + T10 + "<td class=\"Cambria12I_C\" style=\"width:10mm;border:1px solid black\">\r\n"
                + ((periodComment?.DisciplineReport ?? 0) == 2 ? T11 + "X\r\n" : "")
                + T10 + "</td>\r\n"
                + T9 + "</tr>\r\n"
                + T9 + "<tr style=\"height:6.5mm;border:1px solid black\">\r\n"
                + T10 + "<td class=\"Cambria12I_R\" style=\"width:63mm;border:1px solid black\">\r\n"
                + T11 + "<span>3ème degré - insuffisant</span>\r\n"
                + T10 + "</td>\r\n"
                + T10 + "<td class=\"Cambria12I_C\" style=\"width:10mm;border:1px solid black\">\r\n"
                + ((periodComment?.DisciplineReport ?? 0) == 3 ? T11 + "X\r\n" : "")
                + T10 + "</td>\r\n"
                + T9 + "</tr>\r\n"
                + T9 + "<tr style=\"height:6.5mm;border:1px solid black\">\r\n"
                + T10 + "<td class=\"Cambria12I_R\" style=\"width:63mm;border:1px solid black\">\r\n"
                + T11 + "<span>Avertissement</span>\r\n"
                + T10 + "</td>\r\n"
                + T10 + "<td class=\"Cambria12I_C\" style=\"width:10mm;border:1px solid black\">\r\n"
                + ((periodComment?.DisciplineReport ?? 0) == 4 ? T11 + "X\r\n" : "")
                + T10 + "</td>\r\n"
                + T9 + "</tr>\r\n"
                + T8 + "</table>\r\n"
                + T7 + "</td>\r\n"
                + T6 + "</tr>\r\n"
                + T5 + "</table>\r\n"
                + T4 + "</td>\r\n"
                + T4 + "<td style=\"width:1.5mm\">\r\n"
                + T4 + "</td>\r\n"
                + T4 + "<td>\r\n"
                + T5 + "<table style=\"height:34.5mm;border:1px solid black\">\r\n"
                + T6 + "<tr>\r\n"
                + T7 + "<td style=\"height:32.5mm;\" width=\"100%\">\r\n"
                + T8 + "<table style=\"height:32.5mm;border-collapse:collapse\">\r\n"
                + T9 + "<tr style=\"height:6.5mm;border:1px solid black\">\r\n"
                + T10 + "<td class=\"Cambria12I_L\" style=\"width:39mm;border:1px solid black\">\r\n"
                + T11 + "<span>Signature des parents :</span>\r\n"
                + T10 + "</td>\r\n"
                + T9 + "</tr>\r\n"
                + T9 + "<tr style=\"height:26mm;border:1px solid black\">\r\n"
                + T10 + "<td style=\"width:39mm;border:1px solid black\">\r\n"
                + T10 + "</td>\r\n"
                + T9 + "</tr>\r\n"
                + T8 + "</table>\r\n"
                + T7 + "</td>\r\n"
                + T6 + "</tr>\r\n"
                + T5 + "</table>\r\n"
                + T4 + "</td>\r\n"
                + T3 + "</tr>\r\n"
                + T2 + "</table>\r\n");

            WriteFooter(filename);

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
    }
}

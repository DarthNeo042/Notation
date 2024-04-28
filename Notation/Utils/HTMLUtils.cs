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
        private const string T12 = "\t\t\t\t\t\t\t\t\t\t\t\t";

        static public void MergeHTML(string directory, IEnumerable<string> filenames, string mergeFilename)
        {
            using (FileStream mergeFile = new FileStream(mergeFilename, FileMode.OpenOrCreate))
            {
                using (StreamWriter mergeWriter = new StreamWriter(mergeFile))
                {
                    WriteHeader(mergeWriter);

                    bool first = true;
                    foreach (string filename in filenames)
                    {
                        if (first)
                        {
                            first = false;
                        }
                        else
                        {
                            mergeWriter.Write(T2 + "<div class=\"pagebreak\">\r\n"
                                + T2 + "</div>\r\n");
                        }
                        bool header = true;
                        foreach (string line in File.ReadAllLines(Path.Combine(directory, filename)))
                        {
                            if (header)
                            {
                                header = !line.EndsWith("<body>");
                            }
                            else if (line.EndsWith("</body>"))
                            {
                                break;
                            }
                            else
                            {
                                mergeWriter.WriteLine(line);
                            }
                        }
                    }

                    WriteFooter(mergeWriter);
                }
            }
        }

        static private void WriteHeader(StreamWriter writer)
        {
            writer.Write("<html>\r\n"
                + T1 + "<head>\r\n"
                + T2 + "<style>\r\n"
                + T3 + "page { width:19.4cm; height:28.3cm; margin:7mm 8mm 7mm 8mm; }\r\n"
                + T3 + "@media print { body { width:19.4cm; height:28.3cm; margin:0mm 8mm 7mm 8mm; } .pagebreak { clear: both; page-break-after: always; } }\r\n"
                + T3 + "span { padding-left:0.5mm; padding-right:0.5mm; }\r\n"
                + T3 + "td { font-family:Cambria; }\r\n"
                + T3 + "td.Calibri17B_C { font-family:Calibri; font-size:17; text-align:center; font-weight:bold; }\r\n"
                + T3 + "td.Calibri14_L { font-family:Calibri; font-size:14; }\r\n"
                + T3 + "td.Calibri12_C { font-family:Calibri; font-size:12; text-align:center; }\r\n"
                + T3 + "td.Cambria27_C { font-size:27; text-align:center; }\r\n"
                + T3 + "td.Cambria21_R { font-size:21; text-align:right; }\r\n"
                + T3 + "td.Cambria19_C { font-size:19; text-align:center; }\r\n"
                + T3 + "td.Cambria17B_C { font-size:17; text-align:center; font-weight:bold; }\r\n"
                + T3 + "td.Cambria16B_C { font-size:16; text-align:center; font-weight:bold; }\r\n"
                + T3 + "td.Cambria16_C { font-size:16; text-align:center; }\r\n"
                + T3 + "td.Cambria15_C { font-size:15; text-align:center; }\r\n"
                + T3 + "td.Cambria15I_C { font-size:15; text-align:center; font-style:italic; }\r\n"
                + T3 + "td.Cambria15B_C { font-size:15; text-align:center; font-weight:bold; }\r\n"
                + T3 + "td.Cambria15B_R { font-size:15; text-align:right; font-weight:bold; }\r\n"
                + T3 + "td.Cambria13_R { font-size:13; text-align:right; }\r\n"
                + T3 + "td.Cambria12BI_R { font-size:12; text-align:right; font-weight:bold; font-style:italic; }\r\n"
                + T3 + "td.Cambria12I_L { font-size:12; font-style:italic; }\r\n"
                + T3 + "td.Cambria12_C { font-size:12; text-align:center; }\r\n"
                + T3 + "td.Cambria12I_C { font-size:12; text-align:center; font-style:italic; }\r\n"
                + T3 + "td.Cambria12I_R { font-size:12; text-align:right; font-style:italic; }\r\n"
                + T3 + "td.Cambria11_C { font-size:11; text-align:center; }\r\n"
                + T3 + "td.Cambria10_C { font-size:10; text-align:center; }\r\n"
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

        static private void WriteAdressFooter(StreamWriter writer)
        {
            writer.Write(T2 + "<table style=\"height:6mm;width:194mm;margin-top:2mm;border-collapse:collapse\">\r\n"
                + T3 + "<tr>\r\n"
                + T4 + "<td class=\"Calibri12_C\">\r\n"
                + T5 + "Collège privé Saint-Martin - La Placelière - 44 690 CHÂTEAU-THÉBAUD - tél. 02 40 56 85 26 - mail : 44e.laplaceliere@fsspx.fr\r\n"
                + T4 + "</td>\r\n"
                + T3 + "</tr>\r\n"
                + T2 + "</table>\r\n");
        }

        static private void WriteFooter(StreamWriter writer)
        {
            writer.Write(T1 + "</body>\r\n"
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
                        _updatePeriodReportsDispatch(studentCount * 600 / studentGroups.Count());
                    }

                    CreatePeriodReportSummary(directory, period, _updatePeriodReportsDispatch);

                    _updatePeriodReportsDispatch(800);

                    string filename = Path.Combine(directory, $"Bulletin de période {period.Number} (regroupement).html");
                    File.Delete(filename);

                    MergeHTML(directory, MainViewModel.Instance.Reports.PeriodReports, filename);

                    MainViewModel.Instance.Reports.PeriodReports.Insert(0, Path.GetFileName(filename));

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
            using (FileStream file = new FileStream(filename, FileMode.OpenOrCreate))
            {
                using (StreamWriter writer = new StreamWriter(file))
                {
                    WriteHeader(writer);

                    writer.Write(T2 + "<table style=\"height:7mm;border-collapse:collapse\">\r\n"
                        + T3 + "<tr>\r\n"
                        + T4 + "<td>\r\n"
                        + T4 + "</td>\r\n"
                        + T3 + "<tr>\r\n"
                        + T2 + "</table>\r\n"
                        + T2 + "<table style=\"width:194mm;border-collapse:collapse\">\r\n"
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

                    writer.Write(T2 + "<table style=\"border:1px solid black;margin-top:1mm\">\r\n"
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
                        writer.Write(T7 + $"<td class=\"Cambria10B_C\" style=\"width:{162 / coefficients.Count()}mm;border:1px solid black\">\r\n"
                            + T8 + $"{(coefficientLibelles.ContainsKey(coefficient) ? coefficientLibelles[coefficient] + " - c" : "C")}oefficient {coefficient}\r\n"
                            + T7 + "</td>\r\n");
                    }
                    writer.Write(T6 + "</tr>\r\n");

                    foreach (SubjectViewModel subject in _class.Level.Subjects.Where(s => s.ParentSubject == null).OrderBy(s => s.Order))
                    {
                        TeacherViewModel teacher = ModelUtils.GetTeacherFromClassAndSubject(student.Class, subject, period);
                        writer.Write(T6 + $"<tr style=\"height:11mm\">\r\n"
                            + T7 + "<td style=\"border:1px solid black\">\r\n"
                            + T8 + "<span class=\"Cambria10B_L\">\r\n"
                            + T9 + $"{subject.Name.ToUpper()}\r\n"
                            + T8 + "</span><br/>\r\n"
                            + T8 + "<span class=\"Cambria9I_L\">\r\n"
                            + T9 + $"({(subject.Option ? "option " : "")}coeff. {subject.Coefficient})\r\n"
                            + T8 + "</span><br/>\r\n"
                            + T8 + "<span class=\"Cambria9I_L\">\r\n"
                            + T9 + $"{(teacher != null ? $"{teacher.Title} {(!string.IsNullOrEmpty(teacher.FirstName) ? teacher.FirstName.Substring(0, 1) : "")}. {teacher.LastName}" : "")}\r\n"
                            + T8 + "</span>\r\n"
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
                            writer.Write(T7 + "<td class=\"Calibri14_L\" style=\"border:1px solid black\">\r\n"
                                + T8 + $"<span>{marksStr}</span>\r\n"
                                + T7 + "</td>\r\n");
                        }
                        writer.Write(T6 + "</tr>\r\n");

                        foreach (SubjectViewModel subject2 in subject.ChildrenSubjects.OrderBy(s => s.Order))
                        {
                            writer.Write(T6 + "<tr style=\"height:8mm;border:1px solid black\">\r\n"
                                + T7 + "<td style=\"text-align:right;border:1px solid black\">\r\n"
                                + T8 + "<span class=\"Cambria12I_R\">\r\n"
                                + T9 + $"{subject2.Name.ToUpper()}\r\n"
                                + T8 + "</span><br/>\r\n"
                                + T8 + "<span class=\"Cambria9I_R\">\r\n"
                                + T9 + $"({(subject2.Option ? "option " : "")}coeff. {subject2.Coefficient})\r\n"
                                + T8 + "</span>\r\n"
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
                                writer.Write(T7 + "<td class=\"Calibri14_L\" style=\"border:1px solid black\">\r\n"
                                    + T8 + $"<span>{marksStr}</span>\r\n"
                                    + T7 + "</td>\r\n");
                            }
                            writer.Write(T6 + "</tr>\r\n");
                        }
                    }

                    writer.Write(T5 + "</table>\r\n"
                        + T4 + "</td>\r\n"
                        + T3 + "</tr>\r\n"
                        + T2 + "</table>\r\n");

                    double average = MarkModel.ReadPeriodTrimesterAverage(period, student);
                    PeriodViewModel lastPeriod = ModelUtils.GetPreviousPeriod(period);
                    double lastAverage = lastPeriod != null ? MarkModel.ReadPeriodTrimesterAverage(lastPeriod, student) : double.MinValue;

                    writer.Write(T2 + "<table style=\"border:1px solid black;margin-top:2mm;margin-left:19mm;width:156mm\">\r\n"
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
                        + T7 + "<td class=\"Cambria12BI_R\" style=\"border:1px solid black\">\r\n"
                        + T8 + "<span>Rappel de la dernière moyenne obtenue</span>\r\n"
                        + T7 + "</td>\r\n"
                        + T7 + "<td class=\"Cambria16_C\" style=\"border:1px solid black\">\r\n"
                        + T8 + $"{(lastAverage != double.MinValue ? lastAverage.ToString("0.0") : "")}\r\n"
                        + T7 + "</td>\r\n"
                        + T6 + "</tr>\r\n"
                        + T6 + "<tr style=\"height:6.5mm\">\r\n"
                        + T7 + "<td class=\"Cambria12BI_R\" style=\"border:1px solid black\">\r\n"
                        + T8 + "<span>Tendance</span>\r\n"
                        + T7 + "</td>\r\n"
                        + T7 + "<td class=\"Calibri17B_C\" style=\"border:1px solid black\">\r\n"
                        + $"{(lastPeriod != null ? (T8 + (average > lastAverage ? "↗" : average < lastAverage ? "↘" : "→")) : "")}\r\n"
                        + T7 + "</td>\r\n"
                        + T6 + "</tr>\r\n"
                        + T5 + "</table>\r\n"
                        + T4 + "</td>\r\n"
                        + T3 + "</tr>\r\n"
                        + T2 + "</table>\r\n");

                    PeriodCommentModel periodComment = PeriodCommentModel.Read(period, student);

                    writer.Write(T2 + "<table style=\"margin-top:2mm;border-collapse:collapse;width:194mm\">\r\n"
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
                        + T10 + "<td class=\"Cambria12I_R\" style=\"border:1px solid black\">\r\n"
                        + T11 + "<span>1er degré - bien</span>\r\n"
                        + T10 + "</td>\r\n"
                        + T10 + "<td class=\"Cambria12I_C\" style=\"border:1px solid black\">\r\n"
                        + ((periodComment?.StudiesReport ?? 0) == 1 ? T11 + "X\r\n" : "")
                        + T10 + "</td>\r\n"
                        + T9 + "</tr>\r\n"
                        + T9 + "<tr style=\"height:6.5mm;border:1px solid black\">\r\n"
                        + T10 + "<td class=\"Cambria12I_R\" style=\"border:1px solid black\">\r\n"
                        + T11 + "<span>2ème degré - doit progresser</span>\r\n"
                        + T10 + "</td>\r\n"
                        + T10 + "<td class=\"Cambria12I_C\" style=\"border:1px solid black\">\r\n"
                        + ((periodComment?.StudiesReport ?? 0) == 2 ? T11 + "X\r\n" : "")
                        + T10 + "</td>\r\n"
                        + T9 + "</tr>\r\n"
                        + T9 + "<tr style=\"height:6.5mm;border:1px solid black\">\r\n"
                        + T10 + "<td class=\"Cambria12I_R\" style=\"border:1px solid black\">\r\n"
                        + T11 + "<span>3ème degré - insuffisant</span>\r\n"
                        + T10 + "</td>\r\n"
                        + T10 + "<td class=\"Cambria12I_C\" style=\"border:1px solid black\">\r\n"
                        + ((periodComment?.StudiesReport ?? 0) == 3 ? T11 + "X\r\n" : "")
                        + T10 + "</td>\r\n"
                        + T9 + "</tr>\r\n"
                        + T9 + "<tr style=\"height:6.5mm;border:1px solid black\">\r\n"
                        + T10 + "<td class=\"Cambria12I_R\" style=\"border:1px solid black\">\r\n"
                        + T11 + "<span>Avertissement</span>\r\n"
                        + T10 + "</td>\r\n"
                        + T10 + "<td class=\"Cambria12I_C\" style=\"border:1px solid black\">\r\n"
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
                        + T10 + "<td class=\"Cambria12I_R\" style=\"border:1px solid black\">\r\n"
                        + T11 + "<span>1er degré - bien</span>\r\n"
                        + T10 + "</td>\r\n"
                        + T10 + "<td class=\"Cambria12I_C\" style=\"border:1px solid black\">\r\n"
                        + ((periodComment?.DisciplineReport ?? 0) == 1 ? T11 + "X\r\n" : "")
                        + T10 + "</td>\r\n"
                        + T9 + "</tr>\r\n"
                        + T9 + "<tr style=\"height:6.5mm;border:1px solid black\">\r\n"
                        + T10 + "<td class=\"Cambria12I_R\" style=\"border:1px solid black\">\r\n"
                        + T11 + "<span>2ème degré - doit progresser</span>\r\n"
                        + T10 + "</td>\r\n"
                        + T10 + "<td class=\"Cambria12I_C\" style=\"border:1px solid black\">\r\n"
                        + ((periodComment?.DisciplineReport ?? 0) == 2 ? T11 + "X\r\n" : "")
                        + T10 + "</td>\r\n"
                        + T9 + "</tr>\r\n"
                        + T9 + "<tr style=\"height:6.5mm;border:1px solid black\">\r\n"
                        + T10 + "<td class=\"Cambria12I_R\" style=\"border:1px solid black\">\r\n"
                        + T11 + "<span>3ème degré - insuffisant</span>\r\n"
                        + T10 + "</td>\r\n"
                        + T10 + "<td class=\"Cambria12I_C\" style=\"border:1px solid black\">\r\n"
                        + ((periodComment?.DisciplineReport ?? 0) == 3 ? T11 + "X\r\n" : "")
                        + T10 + "</td>\r\n"
                        + T9 + "</tr>\r\n"
                        + T9 + "<tr style=\"height:6.5mm;border:1px solid black\">\r\n"
                        + T10 + "<td class=\"Cambria12I_R\" style=\"border:1px solid black\">\r\n"
                        + T11 + "<span>Avertissement</span>\r\n"
                        + T10 + "</td>\r\n"
                        + T10 + "<td class=\"Cambria12I_C\" style=\"border:1px solid black\">\r\n"
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
                        + T7 + "<td>\r\n"
                        + T8 + "<table style=\"border-collapse:collapse\">\r\n"
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

                    WriteAdressFooter(writer);
                    WriteFooter(writer);

                    MainViewModel.Instance.Reports.PeriodReports.Add(Path.GetFileName(filename));
                }
            }
        }

        static public void CreatePeriodReportSummary(string directory, PeriodViewModel period, MainWindow.UpdatePeriodReportsDelegate _updatePeriodReportsDispatch)
        {
            if (!string.IsNullOrEmpty(directory))
            {
                try
                {
                    int classCount = 0;
                    foreach (ClassViewModel _class in MainViewModel.Instance.Parameters.Classes.OrderBy(c => c.Order))
                    {
                        classCount++;
                        ExportUtils.ExportPeriodSummary(directory, _class, period);
                        _updatePeriodReportsDispatch(800 + classCount * 200 / MainViewModel.Instance.Parameters.Classes.Count);
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
            string directory = FileUtils.SelectDirectory(Settings.Settings.Instance.LastSelectedDirectorySemiTrimesterReports, "LastSelectedDirectorySemiTrimesterReports");

            if (!string.IsNullOrEmpty(directory))
            {
                try
                {
                    CopyIcone(directory);

                    MainViewModel.Instance.Reports.SemiTrimesterReports.Clear();

                    int classCount = 0;
                    foreach (ClassViewModel _class in MainViewModel.Instance.Parameters.Classes)
                    {
                        HTMLUtils_SemiTrimester HTMLUtils_SemiTrimester = new HTMLUtils_SemiTrimester();
                        foreach (SubjectViewModel subject in _class.Level.Subjects)
                        {
                            if (subject.ChildrenSubjects.Any())
                            {
                                double average = MarkModel.ReadSemiTrimesterClassMainSubjectAverage(semiTrimester, _class, subject);
                                HTMLUtils_SemiTrimester.ClassSubjectAverages.Add(subject, average);
                                MarkModel.ReadSemiTrimesterMinMaxMainSubjectAverage(semiTrimester, _class, subject, out double minAverage, out double maxAverage);
                                HTMLUtils_SemiTrimester.ClassSubjectMinAverages.Add(subject, minAverage);
                                HTMLUtils_SemiTrimester.ClassSubjectMaxAverages.Add(subject, maxAverage);
                                foreach (SubjectViewModel subject2 in subject.ChildrenSubjects.OrderBy(s => s.Order))
                                {
                                    average = MarkModel.ReadSemiTrimesterClassSubjectAverage(semiTrimester, _class, subject2);
                                    HTMLUtils_SemiTrimester.ClassSubjectAverages.Add(subject2, average);
                                    MarkModel.ReadSemiTrimesterMinMaxSubjectAverage(semiTrimester, _class, subject2, out minAverage, out maxAverage);
                                    HTMLUtils_SemiTrimester.ClassSubjectMinAverages.Add(subject2, minAverage);
                                    HTMLUtils_SemiTrimester.ClassSubjectMaxAverages.Add(subject2, maxAverage);
                                }
                            }
                            else
                            {
                                double average = MarkModel.ReadSemiTrimesterClassSubjectAverage(semiTrimester, _class, subject);
                                HTMLUtils_SemiTrimester.ClassSubjectAverages.Add(subject, average);
                                MarkModel.ReadSemiTrimesterMinMaxSubjectAverage(semiTrimester, _class, subject, out double minAverage, out double maxAverage);
                                HTMLUtils_SemiTrimester.ClassSubjectMinAverages.Add(subject, minAverage);
                                HTMLUtils_SemiTrimester.ClassSubjectMaxAverages.Add(subject, maxAverage);
                            }
                        }
                        HTMLUtils_SemiTrimester.ClassAverage = MarkModel.ReadSemiTrimesterClassAverage(semiTrimester, _class);
                        MarkModel.ReadSemiTrimesterMinMaxAverage(semiTrimester, _class, out double classMinAverage, out double classMaxAverage);
                        HTMLUtils_SemiTrimester.ClassMinAverage = classMinAverage;
                        HTMLUtils_SemiTrimester.ClassMaxAverage = classMaxAverage;
                        foreach (StudentViewModel student in _class.Students)
                        {
                            double average = MarkModel.ReadSemiTrimesterAverage(semiTrimester, student);
                            if (average != double.MinValue)
                            {
                                HTMLUtils_SemiTrimester.StudentAverages[student] = average;
                            }
                        }
                        int studentCount = 0;
                        foreach (StudentViewModel student in HTMLUtils_SemiTrimester.StudentAverages.Keys)
                        {
                            try
                            {
                                GenerateSemiTrimesterReport(directory, semiTrimester, student, _class, HTMLUtils_SemiTrimester);
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show(e.Message);
                            }

                            studentCount++;
                            _updateSemiTrimesterReportsDispatch(studentCount * 800 / HTMLUtils_SemiTrimester.StudentAverages.Count / MainViewModel.Instance.Parameters.Classes.Count
                                + classCount * 800  / MainViewModel.Instance.Parameters.Classes.Count);
                        }
                        classCount++;
                    }

                    string filename = Path.Combine(directory, $"Bulletin de demi-trimestre {(semiTrimester.Name.ToUpper().StartsWith("A") || semiTrimester.Name.ToUpper().StartsWith("O") ? "D'" : "DE ")}{semiTrimester.Name} (regroupement).html");
                    File.Delete(filename);

                    MergeHTML(directory, MainViewModel.Instance.Reports.SemiTrimesterReports, filename);

                    MainViewModel.Instance.Reports.SemiTrimesterReports.Insert(0, Path.GetFileName(filename));
                    _updateSemiTrimesterReportsDispatch(1000);

                    MainViewModel.Instance.Reports.SemiTrimesterReportsPath = directory;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        private static void GenerateSemiTrimesterReport(string directory, SemiTrimesterViewModel semiTrimester, StudentViewModel student, ClassViewModel _class, HTMLUtils_SemiTrimester HTMLUtils_SemiTrimester)
        {
            string filename = Path.Combine(directory, $"Bulletin de demi-trimestre {(semiTrimester.Name.ToUpper().StartsWith("A") || semiTrimester.Name.ToUpper().StartsWith("O") ? "D'" : "DE ")}{semiTrimester.Name} de {_class.Name} de {student.LastName} {student.FirstName}.html");

            File.Delete(filename);
            using (FileStream file = new FileStream(filename, FileMode.OpenOrCreate))
            {
                using (StreamWriter writer = new StreamWriter(file))
                {
                    WriteHeader(writer);

                    writer.Write(T2 + "<table style=\"height:7mm;border-collapse:collapse\">\r\n"
                        + T3 + "<tr>\r\n"
                        + T4 + "<td>\r\n"
                        + T4 + "</td>\r\n"
                        + T3 + "<tr>\r\n"
                        + T2 + "</table>\r\n"
                        + T2 + "<table style=\"width:194mm;border-collapse:collapse\">\r\n"
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
                        + T8 + "BULLETIN DEMI-TRIMESTRIEL\r\n"
                        + T7 + "</td>\r\n"
                        + T6 + "</tr>\r\n"
                        + T6 + "<tr style=\"height:8.5mm\">\r\n"
                        + T7 + "<td class=\"Cambria27_C\">\r\n"
                        + T8 + $"{(semiTrimester.Name.ToUpper().StartsWith("A") || semiTrimester.Name.ToUpper().StartsWith("O") ? "D'" : "DE ")}{semiTrimester.Name.ToUpper()}\r\n"
                        + T7 + "</td>\r\n"
                        + T6 + "</tr>\r\n"
                        + T6 + "<tr style=\"height:7mm\">\r\n"
                        + T7 + "<td class=\"Cambria19_C\">\r\n"
                        + T8 + $"Année {student.Year} - {student.Year + 1}\r\n"
                        + T7 + "</td>\r\n"
                        + T6 + "</tr>\r\n"
                        + T6 + "<tr style=\"height:6mm\">\r\n"
                        + T7 + "<td class=\"Cambria15_C\">\r\n"
                        + T8 + $"Classe de {_class.Name} - Effectif {_class.Students.Count}\r\n"
                        + T7 + "</td>\r\n"
                        + T6 + "</tr>\r\n"
                        + T6 + "<tr style=\"height:5mm\">\r\n"
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

                    writer.Write(T2 + "<table style=\"margin-top:2mm;border-collapse:collapse;width:194mm\">\r\n"
                        + T3 + "<tr>\r\n"
                        + T4 + "<td>\r\n"
                        + T5 + "<table style=\"border:1px solid black;\">\r\n"
                        + T6 + "<tr>\r\n"
                        + T7 + "<td>\r\n"
                        + T8 + "<table style=\"border-collapse:collapse\">\r\n"
                        + T9 + "<tr style=\"height:8mm;border:1px solid black\">\r\n"
                        + T10 + "<td class=\"Cambria15_C\" style=\"width:28mm;border:1px solid black\">\r\n"
                        + T11 + "<span>Disciplines</span>\r\n"
                        + T10 + "</td>\r\n"
                        + T10 + "<td class=\"Cambria11_C\" style=\"width:13mm;border:1px solid black\">\r\n"
                        + T11 + "<span>Moyenne élève</span>\r\n"
                        + T10 + "</td>\r\n"
                        + T9 + "</tr>\r\n");

                    int subjectsHeight = 8;

                    foreach (SubjectViewModel subject in _class.Level.Subjects.Where(s => s.ParentSubject == null))
                    {
                        subjectsHeight += 11;
                        TeacherViewModel teacher = ModelUtils.GetTeacherFromClassAndSubject(student.Class, subject, semiTrimester.Period1);

                        double average = double.MinValue;
                        if (subject.ChildrenSubjects.Any())
                        {
                            average = MarkModel.ReadSemiTrimesterMainSubjectAverage(semiTrimester, student, subject);
                        }
                        else
                        {
                            average = MarkModel.ReadSemiTrimesterSubjectAverage(semiTrimester, student, subject);
                        }

                        writer.Write(T9 + "<tr style=\"height:11mm;border:1px solid black\">\r\n"
                            + T10 + "<td style=\"border:1px solid black\">\r\n"
                            + T11 + "<span class=\"Cambria10B_L\">\r\n"
                            + T12 + $"{subject.Name.ToUpper()}\r\n"
                            + T11 + "</span><br/>\r\n"
                            + T11 + "<span class=\"Cambria9I_L\">\r\n"
                            + T12 + $"{(subject.Option ? $"(option " : $"(")}coeff. {subject.Coefficient})\r\n"
                            + T11 + "</span><br/>\r\n"
                            + T11 + "<span class=\"Cambria9I_L\">\r\n"
                            + T12 + $"{(teacher != null ? $"{teacher.Title} {(!string.IsNullOrEmpty(teacher.FirstName) ? teacher.FirstName.Substring(0, 1) : "")}. {teacher.LastName}" : "")}\r\n"
                            + T11 + "</span><br/>\r\n"
                            + T10 + "</td>\r\n"
                            + T10 + "<td class=\"Cambria17B_C\" style=\"border:1px solid black\">\r\n"
                            + T11 + $"{(average != double.MinValue ? average.ToString("0.0") : "")}\r\n"
                            + T10 + "</td>\r\n"
                            + T9 + "</tr>\r\n");

                        foreach (SubjectViewModel subject2 in subject.ChildrenSubjects.OrderBy(s => s.Order))
                        {
                            subjectsHeight += 8;
                            average = MarkModel.ReadSemiTrimesterSubjectAverage(semiTrimester, student, subject2);
                            writer.Write(T9 + "<tr style=\"height:8mm;border:1px solid black\">\r\n"
                                + T10 + "<td style=\"border:1px solid black\">\r\n"
                                + T11 + "<span class=\"Cambria12I_R\">\r\n"
                                + T12 + $"{subject2.Name.ToUpper()}\r\n"
                                + T11 + "</span><br/>\r\n"
                                + T11 + "<span class=\"Cambria9I_R\">\r\n"
                                + T12 + $"{(subject2.Option ? $"(option " : $"(")}coeff. {subject2.Coefficient})\r\n"
                                + T11 + "</span><br/>\r\n"
                                + T10 + "</td>\r\n"
                                + T10 + "<td class=\"Cambria17B_C\" style=\"border:1px solid black\">\r\n"
                                + T11 + $"{(average != double.MinValue ? average.ToString("0.0") : "")}\r\n"
                                + T10 + "</td>\r\n"
                                + T9 + "</tr>\r\n");
                        }
                    }

                    writer.Write(T8 + "</table>\r\n"
                        + T7 + "</td>\r\n"
                        + T6 + "</tr>\r\n"
                        + T5 + "</table>\r\n"
                        + T4 + "</td>\r\n"
                        + T4 + "<td style=\"width:2mm\">\r\n"
                        + T4 + "</td>\r\n"
                        + T4 + "<td>\r\n"
                        + T5 + "<table style=\"height:31mm;border:1px solid black\">\r\n"
                        + T6 + "<tr>\r\n"
                        + T7 + "<td style=\"height:29mm\">\r\n"
                        + T8 + "<table style=\"height:29mm;border-collapse:collapse\">\r\n"
                        + T9 + "<tr style=\"height:8mm;border:1px solid black\">\r\n"
                        + T10 + "<td class=\"Cambria11_C\" style=\"width:10mm;border:1px solid black\">\r\n"
                        + T11 + "<span>Moy. classe</span>\r\n"
                        + T10 + "</td>\r\n"
                        + T10 + "<td class=\"Cambria10_C\" style=\"width:7.5mm;border:1px solid black\">\r\n"
                        + T11 + "<span>Moy. min</span>\r\n"
                        + T10 + "</td>\r\n"
                        + T10 + "<td class=\"Cambria10_C\" style=\"width:7.5mm;border:1px solid black\">\r\n"
                        + T11 + "<span>Moy. max</span>\r\n"
                        + T10 + "</td>\r\n"
                        + T9 + "</tr>\r\n");

                    foreach (SubjectViewModel subject in _class.Level.Subjects.Where(s => s.ParentSubject == null))
                    {
                        writer.Write(T9 + "<tr style=\"height:11mm;border:1px solid black\">\r\n"
                            + T10 + "<td class=\"Cambria15I_C\" style=\"border:1px solid black\">\r\n"
                            + T11 + $"{(HTMLUtils_SemiTrimester.ClassSubjectAverages[subject] != double.MinValue ? HTMLUtils_SemiTrimester.ClassSubjectAverages[subject].ToString("0.0") : "")}\r\n"
                            + T10 + "</td>\r\n"
                            + T10 + "<td class=\"Cambria12I_C\" style=\"border:1px solid black\">\r\n"
                            + T11 + $"{(HTMLUtils_SemiTrimester.ClassSubjectMinAverages[subject] != double.MaxValue ? HTMLUtils_SemiTrimester.ClassSubjectMinAverages[subject].ToString("0.0") : "")}\r\n"
                            + T10 + "</td>\r\n"
                            + T10 + "<td class=\"Cambria12I_C\" style=\"border:1px solid black\">\r\n"
                            + T11 + $"{(HTMLUtils_SemiTrimester.ClassSubjectMaxAverages[subject] != double.MinValue ? HTMLUtils_SemiTrimester.ClassSubjectMaxAverages[subject].ToString("0.0") : "")}\r\n"
                            + T10 + "</td>\r\n"
                            + T9 + "</tr>\r\n");

                        foreach (SubjectViewModel subject2 in subject.ChildrenSubjects.OrderBy(s => s.Order))
                        {
                            writer.Write(T9 + "<tr style=\"height:8mm;border:1px solid black\">\r\n"
                                + T10 + "<td class=\"Cambria15I_C\" style=\"border:1px solid black\">\r\n"
                                + T11 + $"{(HTMLUtils_SemiTrimester.ClassSubjectAverages[subject2] != double.MinValue ? HTMLUtils_SemiTrimester.ClassSubjectAverages[subject2].ToString("0.0") : "")}\r\n"
                                + T10 + "</td>\r\n"
                                + T10 + "<td class=\"Cambria12I_C\" style=\"border:1px solid black\">\r\n"
                                + T11 + $"{(HTMLUtils_SemiTrimester.ClassSubjectMinAverages[subject2] != double.MaxValue ? HTMLUtils_SemiTrimester.ClassSubjectMinAverages[subject2].ToString("0.0") : "")}\r\n"
                                + T10 + "</td>\r\n"
                                + T10 + "<td class=\"Cambria12I_C\" style=\"border:1px solid black\">\r\n"
                                + T11 + $"{(HTMLUtils_SemiTrimester.ClassSubjectMaxAverages[subject2] != double.MinValue ? HTMLUtils_SemiTrimester.ClassSubjectMaxAverages[subject2].ToString("0.0") : "")}\r\n"
                                + T10 + "</td>\r\n"
                                + T9 + "</tr>\r\n");
                        }
                    }

                    SemiTrimesterCommentModel semiTrimesterComment = SemiTrimesterCommentModel.Read(semiTrimester, student);

                    writer.Write(T8 + "</table>\r\n"
                        + T7 + "</td>\r\n"
                        + T6 + "</tr>\r\n"
                        + T5 + "</table>\r\n"
                        + T4 + "</td>\r\n"
                        + T4 + "<td style=\"width:2mm\">\r\n"
                        + T4 + "</td>\r\n"
                        + T4 + "<td>\r\n"
                        + T5 + "<table style=\"border:1px solid black\">\r\n"
                        + T6 + "<tr>\r\n"
                        + T7 + "<td>\r\n"
                        + T8 + "<table style=\"height:29mm;border-collapse:collapse\">\r\n"
                        + T9 + "<tr style=\"height:8mm;border:1px solid black\">\r\n"
                        + T10 + "<td class=\"Cambria15_C\" style=\"width:115mm;border:1px solid black\">\r\n"
                        + T11 + "<span>Compte rendu des études</span>\r\n"
                        + T10 + "</td>\r\n"
                        + T9 + "</tr>\r\n"
                        + T9 + "<tr style=\"height:55mm;border:1px solid black\">\r\n"
                        + T10 + "<td class=\"Cambria15_L\" style=\"border:1px solid black\">\r\n"
                        + T11 + $"<span>{(semiTrimesterComment?.MainTeacherComment ?? "")}</span>\r\n"
                        + T10 + "</td>\r\n"
                        + T9 + "</tr>\r\n"
                        + T8 + "</table>\r\n"
                        + T7 + "</td>\r\n"
                        + T6 + "</tr>\r\n"
                        + T5 + "</table>\r\n"
                        + T5 + "<table style=\"height:2mm;border-collapse:collapse\">\r\n"
                        + T6 + "<tr>\r\n"
                        + T7 + "<td>\r\n"
                        + T7 + "</td>\r\n"
                        + T6 + "</tr>\r\n"
                        + T5 + "</table>\r\n"
                        + T5 + "<table style=\"border:1px solid black\">\r\n"
                        + T6 + "<tr>\r\n"
                        + T7 + "<td>\r\n"
                        + T8 + "<table style=\"height:29mm;border-collapse:collapse\">\r\n"
                        + T9 + "<tr style=\"height:8mm;border:1px solid black\">\r\n"
                        + T10 + "<td class=\"Cambria15_C\" style=\"width:115mm;border:1px solid black\">\r\n"
                        + T11 + $"<span>Appréciation du préfet de division{(!string.IsNullOrEmpty(MainViewModel.Instance.Parameters.YearParameters.DivisionPrefect) ? $" {MainViewModel.Instance.Parameters.YearParameters.DivisionPrefect}" : "")}</span>\r\n"
                        + T10 + "</td>\r\n"
                        + T9 + "</tr>\r\n"
                        + T9 + "<tr style=\"height:55mm;border:1px solid black\">\r\n"
                        + T10 + "<td class=\"Cambria15_L\" style=\"border:1px solid black\">\r\n"
                        + T11 + $"<span>{(semiTrimesterComment?.DivisionPrefectComment ?? "")}</span>\r\n"
                        + T10 + "</td>\r\n"
                        + T9 + "</tr>\r\n"
                        + T8 + "</table>\r\n"
                        + T7 + "</td>\r\n"
                        + T6 + "</tr>\r\n"
                        + T5 + "</table>\r\n"
                        + T5 + $"<table style=\"height:{((subjectsHeight - 132) + 1.75).ToString().Replace(",", ".")}mm;border-collapse:collapse\">\r\n"
                        + T6 + "<tr>\r\n"
                        + T7 + "<td>\r\n"
                        + T7 + "</td>\r\n"
                        + T6 + "</tr>\r\n"
                        + T5 + "</table>\r\n"
                        + T4 + "</td>\r\n");

                    double average2 = MarkModel.ReadSemiTrimesterAverage(semiTrimester, student);

                    writer.Write(T3 + "</tr>\r\n"
                        + T2 + "</table>\r\n"
                        + T2 + "<table style=\"margin-top:2mm;border-collapse:collapse;width:194mm\">\r\n"
                        + T3 + "<tr>\r\n"
                        + T4 + "<td>\r\n"
                        + T5 + "<table style=\"border:1px solid black;\">\r\n"
                        + T6 + "<tr>\r\n"
                        + T7 + "<td>\r\n"
                        + T8 + "<table style=\"border-collapse:collapse\">\r\n"
                        + T9 + "<tr style=\"height:11mm;border:1px solid black\">\r\n"
                        + T10 + "<td class=\"Cambria15B_R\" style=\"width:28mm;border:1px solid black\">\r\n"
                        + T11 + "<span>Moyenne</span>\r\n"
                        + T10 + "</td>\r\n"
                        + T10 + "<td class=\"Cambria15B_C\" style=\"width:13mm;border:1px solid black\">\r\n"
                        + T11 + $"<span>{(average2 != double.MinValue ? average2.ToString("0.0") : "")}</span>\r\n"
                        + T10 + "</td>\r\n"
                        + T9 + "</tr>\r\n"
                        + T8 + "</table>\r\n"
                        + T7 + "</td>\r\n"
                        + T6 + "</tr>\r\n"
                        + T5 + "</table>\r\n"
                        + T5 + "<table style = \"height:2mm;border-collapse:collapse\">\r\n"
                        + T6 + "<tr>\r\n"
                        + T7 + "<td>\r\n"
                        + T7 + "</td>\r\n"
                        + T6 + "</tr>\r\n"
                        + T5 + "</table>\r\n"
                        + T5 + "<table style=\"border:1px solid black;\">\r\n"
                        + T6 + "<tr>\r\n"
                        + T7 + "<td>\r\n"
                        + T8 + "<table style=\"border-collapse:collapse\">\r\n"
                        + T9 + "<tr style=\"height:11mm;border:1px solid black\">\r\n"
                        + T10 + "<td class=\"Cambria15B_R\" style=\"width:28mm;border:1px solid black\">\r\n"
                        + T11 + "<span>Classement</span>\r\n"
                        + T10 + "</td>\r\n"
                        + T10 + "<td class=\"Cambria15B_C\" style=\"width:13mm;border:1px solid black\">\r\n"
                        + T11 + $"<span>{MarkModel.ReadSemiTrimesterRanking(student, HTMLUtils_SemiTrimester.StudentAverages)}/{HTMLUtils_SemiTrimester.StudentAverages.Count}</span>\r\n"
                        + T10 + "</td>\r\n"
                        + T9 + "</tr>\r\n"
                        + T8 + "</table>\r\n"
                        + T7 + "</td>\r\n"
                        + T6 + "</tr>\r\n"
                        + T5 + "</table>\r\n"
                        + T4 + "</td>\r\n"
                        + T4 + "<td style=\"width:2mm\">\r\n"
                        + T4 + "</td>\r\n"
                        + T4 + "<td>\r\n"
                        + T5 + "<table style=\"height:13mm;border:1px solid black\">\r\n"
                        + T6 + "<tr>\r\n"
                        + T7 + "<td>\r\n"
                        + T8 + "<table style=\"border-collapse:collapse\">\r\n"
                        + T9 + "<tr style=\"height:11mm;border:1px solid black\">\r\n"
                        + T10 + "<td class=\"Cambria15I_C\" style=\"width:10mm;border:1px solid black\">\r\n"
                        + T11 + $"{(HTMLUtils_SemiTrimester.ClassAverage != double.MinValue ? HTMLUtils_SemiTrimester.ClassAverage.ToString("0.0") : "")}\r\n"
                        + T10 + " </td>\r\n"
                        + T10 + "<td class=\"Cambria12I_C\" style=\"width:7.5mm;border:1px solid black\">\r\n"
                        + T11 + $"{(HTMLUtils_SemiTrimester.ClassMinAverage != double.MaxValue ? HTMLUtils_SemiTrimester.ClassMinAverage.ToString("0.0") : "")}\r\n"
                        + T10 + " </td>\r\n"
                        + T10 + "<td class=\"Cambria12I_C\" style=\"width:7.5mm;border:1px solid black\">\r\n"
                        + T11 + $"{(HTMLUtils_SemiTrimester.ClassMaxAverage != double.MinValue ? HTMLUtils_SemiTrimester.ClassMaxAverage.ToString("0.0") : "")}\r\n"
                        + T10 + "</td>\r\n"
                        + T9 + "</tr>\r\n"
                        + T8 + "</table>\r\n"
                        + T7 + "</td>\r\n"
                        + T6 + "</tr>\r\n"
                        + T5 + "</table>\r\n"
                        + T5 + "<table style=\"height:15.4mm;border-collapse:collapse\">\r\n"
                        + T6 + "<tr>\r\n"
                        + T7 + "<td>\r\n"
                        + T7 + "</td>\r\n"
                        + T6 + "</tr>\r\n"
                        + T5 + "</table>\r\n"
                        + T4 + "</td>\r\n"
                        + T4 + "<td style=\"width:74mm\">\r\n"
                        + T4 + "</td>\r\n"
                        + T4 + "<td>\r\n"
                        + T5 + "<table style=\"height:21mm;border:1px solid black\">\r\n"
                        + T6 + "<tr>\r\n"
                        + T7 + "<td>\r\n"
                        + T8 + "<table style=\"height:21mm;border-collapse:collapse\">\r\n"
                        + T9 + "<tr style=\"height:6.5mm;border:1px solid black\">\r\n"
                        + T10 + "<td class=\"Cambria12I_L\" style=\"width:39mm;border:1px solid black\">\r\n"
                        + T11 + "<span>Signature des parents :</span>\r\n"
                        + T10 + "</td>\r\n"
                        + T9 + "<tr>\r\n"
                        + T9 + "<tr style=\"height:19mm;border:1px solid black\">\r\n"
                        + T10 + "<td style=\"width:39mm;border:1px solid black\">\r\n"
                        + T10 + "</td>\r\n"
                        + T9 + "<tr>\r\n"
                        + T8 + "</table>\r\n"
                        + T7 + "</td>\r\n"
                        + T6 + "</tr>\r\n"
                        + T5 + "</table>\r\n"
                        + T4 + "</td>\r\n"
                        + T3 + "</tr>\r\n"
                        + T2 + "</table>\r\n");

                    WriteAdressFooter(writer);
                    WriteFooter(writer);
                }
            }

            MainViewModel.Instance.Reports.SemiTrimesterReports.Add(Path.GetFileName(filename));
        }

        static public void CreateTrimesterReport(int trimester, MainWindow.UpdateTrimesterReportsDelegate _updateTrimesterReportsDispatch)
        {
            string directory = FileUtils.SelectDirectory(Settings.Settings.Instance.LastSelectedDirectoryTrimesterReports, "LastSelectedDirectoryTrimesterReports");

            if (!string.IsNullOrEmpty(directory))
            {
                try
                {
                    CopyIcone(directory);

                    MainViewModel.Instance.Reports.TrimesterReports.Clear();

                    int classCount = 0;
                    foreach (ClassViewModel _class in MainViewModel.Instance.Parameters.Classes)
                    {
                        HTMLUtils_Trimester HTMLUtils_Trimester = new HTMLUtils_Trimester();
                        foreach (SubjectViewModel subject in _class.Level.Subjects)
                        {
                            if (subject.ChildrenSubjects.Any())
                            {
                                double average = MarkModel.ReadTrimesterClassMainSubjectAverage(trimester, _class, subject);
                                HTMLUtils_Trimester.ClassSubjectAverages.Add(subject, average);
                                MarkModel.ReadTrimesterMinMaxMainSubjectAverage(trimester, _class, subject, out double minAverage, out double maxAverage);
                                HTMLUtils_Trimester.ClassSubjectMinAverages.Add(subject, minAverage);
                                HTMLUtils_Trimester.ClassSubjectMaxAverages.Add(subject, maxAverage);
                                foreach (SubjectViewModel subject2 in subject.ChildrenSubjects.OrderBy(s => s.Order))
                                {
                                    average = MarkModel.ReadTrimesterClassSubjectAverage(trimester, _class, subject2);
                                    HTMLUtils_Trimester.ClassSubjectAverages.Add(subject2, average);
                                    MarkModel.ReadTrimesterMinMaxSubjectAverage(trimester, _class, subject2, out minAverage, out maxAverage);
                                    HTMLUtils_Trimester.ClassSubjectMinAverages.Add(subject2, minAverage);
                                    HTMLUtils_Trimester.ClassSubjectMaxAverages.Add(subject2, maxAverage);
                                }
                            }
                            else
                            {
                                double average = MarkModel.ReadTrimesterClassSubjectAverage(trimester, _class, subject);
                                HTMLUtils_Trimester.ClassSubjectAverages.Add(subject, average);
                                MarkModel.ReadTrimesterMinMaxSubjectAverage(trimester, _class, subject, out double minAverage, out double maxAverage);
                                HTMLUtils_Trimester.ClassSubjectMinAverages.Add(subject, minAverage);
                                HTMLUtils_Trimester.ClassSubjectMaxAverages.Add(subject, maxAverage);
                            }
                        }
                        HTMLUtils_Trimester.ClassAverage = MarkModel.ReadTrimesterClassAverage(trimester, _class);
                        MarkModel.ReadTrimesterMinMaxAverage(trimester, _class, out double classMinAverage, out double classMaxAverage);
                        HTMLUtils_Trimester.ClassMinAverage = classMinAverage;
                        HTMLUtils_Trimester.ClassMaxAverage = classMaxAverage;
                        foreach (StudentViewModel student in _class.Students)
                        {
                            HTMLUtils_Trimester.StudentAverages[student] = MarkModel.ReadTrimesterAverage(trimester, student);
                        }
                        int studentCount = 0;
                        foreach (StudentViewModel student in _class.Students.Where(s => HTMLUtils_Trimester.StudentAverages[s] != double.MinValue))
                        {
                            try
                            {
                                GenerateTrimesterReport(directory, trimester, student, _class, HTMLUtils_Trimester);
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show(e.Message);
                            }

                            studentCount++;
                            _updateTrimesterReportsDispatch(studentCount * 600 / HTMLUtils_Trimester.StudentAverages.Count / MainViewModel.Instance.Parameters.Classes.Count
                                + classCount * 800 / MainViewModel.Instance.Parameters.Classes.Count);
                        }
                        ExportUtils.ExportTrimesterSummary(directory, _class, trimester);
                        classCount++;
                        _updateTrimesterReportsDispatch(classCount * 800 / MainViewModel.Instance.Parameters.Classes.Count);
                    }

                    string filename = Path.Combine(directory, $"Bulletin de trimestre {trimester} (regroupement).html");
                    File.Delete(filename);

                    MergeHTML(directory, MainViewModel.Instance.Reports.TrimesterReports, filename);

                    MainViewModel.Instance.Reports.TrimesterReports.Insert(0, Path.GetFileName(filename));
                    _updateTrimesterReportsDispatch(1000);

                    MainViewModel.Instance.Reports.TrimesterReportsPath = directory;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        private static void GenerateTrimesterReport(string directory, int trimester, StudentViewModel student, ClassViewModel _class, HTMLUtils_Trimester HTMLUtils_Trimester)
        {
            PeriodViewModel period = MainViewModel.Instance.Reports.Periods.OrderBy(p => p.Number).FirstOrDefault(p => p.Trimester == trimester);

            string filename = Path.Combine(directory, $"Bulletin de trimestre {trimester} de {_class.Name} de {student.LastName} {student.FirstName}.html");

            File.Delete(filename);
            using (FileStream file = new FileStream(filename, FileMode.OpenOrCreate))
            {
                using (StreamWriter writer = new StreamWriter(file))
                {
                    WriteHeader(writer);

                    writer.Write(T2 + "<table style=\"height:7mm;border-collapse:collapse\">\r\n"
                        + T3 + "<tr>\r\n"
                        + T4 + "<td>\r\n"
                        + T4 + "</td>\r\n"
                        + T3 + "<tr>\r\n"
                        + T2 + "</table>\r\n"
                        + T2 + "<table style=\"width:194mm;border-collapse:collapse\">\r\n"
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
                        + T8 + "BULLETIN\r\n"
                        + T7 + "</td>\r\n"
                        + T6 + "</tr>\r\n"
                        + T6 + "<tr style=\"height:7mm\">\r\n"
                        + T7 + "<td class=\"Cambria19_C\">\r\n"
                        + T8 + $"DU {NumberUtils.GetRankString(trimester)} TRIMESTRE\r\n"
                        + T7 + "</td>\r\n"
                        + T6 + "</tr>\r\n"
                        + T6 + "<tr style=\"height:7mm\">\r\n"
                        + T7 + "<td class=\"Cambria19_C\">\r\n"
                        + T8 + $"Année {student.Year} - {student.Year + 1}\r\n"
                        + T7 + "</td>\r\n"
                        + T6 + "</tr>\r\n"
                        + T6 + "<tr style=\"height:6mm\">\r\n"
                        + T7 + "<td class=\"Cambria15_C\">\r\n"
                        + T8 + $"Classe de {_class.Name} - Effectif {_class.Students.Count}\r\n"
                        + T7 + "</td>\r\n"
                        + T6 + "</tr>\r\n"
                        + T6 + "<tr style=\"height:5mm\">\r\n"
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

                    writer.Write(T2 + "<table style=\"margin-top:2mm;border-collapse:collapse;width:194mm\">\r\n"
                        + T3 + "<tr>\r\n"
                        + T4 + "<td>\r\n"
                        + T5 + "<table style=\"border:1px solid black;\">\r\n"
                        + T6 + "<tr>\r\n"
                        + T7 + "<td>\r\n"
                        + T8 + "<table style=\"border-collapse:collapse\">\r\n"
                        + T9 + "<tr style=\"height:8mm;border:1px solid black\">\r\n"
                        + T10 + "<td class=\"Cambria15_C\" style=\"width:28mm;border:1px solid black\">\r\n"
                        + T11 + "<span>Disciplines</span>\r\n"
                        + T10 + "</td>\r\n"
                        + T10 + "<td class=\"Cambria11_C\" style=\"width:13mm;border:1px solid black\">\r\n"
                        + T11 + "<span>Moyenne élève</span>\r\n"
                        + T10 + "</td>\r\n"
                        + T9 + "</tr>\r\n");

                    double average = double.MinValue;
                    foreach (SubjectViewModel subject in _class.Level.Subjects.Where(s => s.ParentSubject == null))
                    {
                        TeacherViewModel teacher = ModelUtils.GetTeacherFromClassAndSubject(student.Class, subject, period);

                        average = double.MinValue;
                        if (subject.ChildrenSubjects.Any())
                        {
                            average = MarkModel.ReadTrimesterMainSubjectAverage(trimester, student, subject);
                        }
                        else
                        {
                            average = MarkModel.ReadTrimesterSubjectAverage(trimester, student, subject);
                        }

                        writer.Write(T9 + "<tr style=\"height:11mm;border:1px solid black\">\r\n"
                            + T10 + "<td style=\"border:1px solid black\">\r\n"
                            + T11 + "<span class=\"Cambria10B_L\">\r\n"
                            + T12 + $"{subject.Name.ToUpper()}\r\n"
                            + T11 + "</span><br/>\r\n"
                            + T11 + "<span class=\"Cambria9I_L\">\r\n"
                            + T12 + $"{(subject.Option ? $"(option " : $"(")}coeff. {subject.Coefficient})\r\n"
                            + T11 + "</span><br/>\r\n"
                            + T11 + "<span class=\"Cambria9I_L\">\r\n"
                            + T12 + $"{(teacher != null ? $"{teacher.Title} {(!string.IsNullOrEmpty(teacher.FirstName) ? teacher.FirstName.Substring(0, 1) : "")}. {teacher.LastName}" : "")}\r\n"
                            + T11 + "</span><br/>\r\n"
                            + T10 + "</td>\r\n"
                            + T10 + "<td class=\"Cambria17B_C\" style=\"border:1px solid black\">\r\n"
                            + T11 + $"{(average != double.MinValue ? average.ToString("0.0") : "")}\r\n"
                            + T10 + "</td>\r\n"
                            + T9 + "</tr>\r\n");

                        foreach (SubjectViewModel subject2 in subject.ChildrenSubjects.OrderBy(s => s.Order))
                        {
                            average = MarkModel.ReadTrimesterSubjectAverage(trimester, student, subject2);
                            writer.Write(T9 + "<tr style=\"height:8mm;border:1px solid black\">\r\n"
                                + T10 + "<td style=\"border:1px solid black\">\r\n"
                                + T11 + "<span class=\"Cambria12I_R\">\r\n"
                                + T12 + $"{subject2.Name.ToUpper()}\r\n"
                                + T11 + "</span><br/>\r\n"
                                + T11 + "<span class=\"Cambria9I_R\">\r\n"
                                + T12 + $"{(subject2.Option ? $"(option " : $"(")}coeff. {subject2.Coefficient})\r\n"
                                + T11 + "</span><br/>\r\n"
                                + T10 + "</td>\r\n"
                                + T10 + "<td class=\"Cambria17B_C\" style=\"border:1px solid black\">\r\n"
                                + T11 + $"{(average != double.MinValue ? average.ToString("0.0") : "")}\r\n"
                                + T10 + "</td>\r\n"
                                + T9 + "</tr>\r\n");
                        }
                    }

                    writer.Write(T8 + "</table>\r\n"
                        + T7 + "</td>\r\n"
                        + T6 + "</tr>\r\n"
                        + T5 + "</table>\r\n"
                        + T4 + "</td>\r\n"
                        + T4 + "<td style=\"width:2mm\">\r\n"
                        + T4 + "</td>\r\n"
                        + T4 + "<td>\r\n"
                        + T5 + "<table style=\"border:1px solid black\">\r\n"
                        + T6 + "<tr>\r\n"
                        + T7 + "<td>\r\n"
                        + T8 + "<table style=\"border-collapse:collapse\">\r\n"
                        + T9 + "<tr style=\"height:8mm;border:1px solid black\">\r\n"
                        + T10 + "<td class=\"Cambria11_C\" style=\"width:10mm;border:1px solid black\">\r\n"
                        + T11 + "<span>Moy. classe</span>\r\n"
                        + T10 + "</td>\r\n"
                        + T10 + "<td class=\"Cambria10_C\" style=\"width:7.5mm;border:1px solid black\">\r\n"
                        + T11 + "<span>Moy. min</span>\r\n"
                        + T10 + "</td>\r\n"
                        + T10 + "<td class=\"Cambria10_C\" style=\"width:7.5mm;border:1px solid black\">\r\n"
                        + T11 + "<span>Moy. max</span>\r\n"
                        + T10 + "</td>\r\n"
                        + T9 + "</tr>\r\n");

                    foreach (SubjectViewModel subject in _class.Level.Subjects.Where(s => s.ParentSubject == null))
                    {
                        writer.Write(T9 + "<tr style=\"height:11mm;border:1px solid black\">\r\n"
                            + T10 + "<td class=\"Cambria15I_C\" style=\"border:1px solid black\">\r\n"
                            + T11 + $"{(HTMLUtils_Trimester.ClassSubjectAverages[subject] != double.MinValue ? HTMLUtils_Trimester.ClassSubjectAverages[subject].ToString("0.0") : "")}\r\n"
                            + T10 + "</td>\r\n"
                            + T10 + "<td class=\"Cambria12I_C\" style=\"border:1px solid black\">\r\n"
                            + T11 + $"{(HTMLUtils_Trimester.ClassSubjectMinAverages[subject] != double.MaxValue ? HTMLUtils_Trimester.ClassSubjectMinAverages[subject].ToString("0.0") : "")}\r\n"
                            + T10 + "</td>\r\n"
                            + T10 + "<td class=\"Cambria12I_C\" style=\"border:1px solid black\">\r\n"
                            + T11 + $"{(HTMLUtils_Trimester.ClassSubjectMaxAverages[subject] != double.MinValue ? HTMLUtils_Trimester.ClassSubjectMaxAverages[subject].ToString("0.0") : "")}\r\n"
                            + T10 + "</td>\r\n"
                            + T9 + "</tr>\r\n");

                        foreach (SubjectViewModel subject2 in subject.ChildrenSubjects.OrderBy(s => s.Order))
                        {
                            writer.Write(T9 + "<tr style=\"height:8mm;border:1px solid black\">\r\n"
                                + T10 + "<td class=\"Cambria15I_C\" style=\"border:1px solid black\">\r\n"
                                + T11 + $"{(HTMLUtils_Trimester.ClassSubjectAverages[subject2] != double.MinValue ? HTMLUtils_Trimester.ClassSubjectAverages[subject2].ToString("0.0") : "")}\r\n"
                                + T10 + "</td>\r\n"
                                + T10 + "<td class=\"Cambria12I_C\" style=\"border:1px solid black\">\r\n"
                                + T11 + $"{(HTMLUtils_Trimester.ClassSubjectMinAverages[subject2] != double.MaxValue ? HTMLUtils_Trimester.ClassSubjectMinAverages[subject2].ToString("0.0") : "")}\r\n"
                                + T10 + "</td>\r\n"
                                + T10 + "<td class=\"Cambria12I_C\" style=\"border:1px solid black\">\r\n"
                                + T11 + $"{(HTMLUtils_Trimester.ClassSubjectMaxAverages[subject2] != double.MinValue ? HTMLUtils_Trimester.ClassSubjectMaxAverages[subject2].ToString("0.0") : "")}\r\n"
                                + T10 + "</td>\r\n"
                                + T9 + "</tr>\r\n");
                        }
                    }

                    writer.Write(T8 + "</table>\r\n"
                        + T7 + "</td>\r\n"
                        + T6 + "</tr>\r\n"
                        + T5 + "</table>\r\n"
                        + T4 + "</td>\r\n"
                        + T4 + "<td style=\"width:2mm\">\r\n"
                        + T4 + "</td>\r\n"
                        + T4 + "<td>\r\n"
                        + T5 + "<table style=\"border:1px solid black\">\r\n"
                        + T6 + "<tr>\r\n"
                        + T7 + "<td>\r\n"
                        + T8 + "<table style=\"border-collapse:collapse\">\r\n"
                        + T9 + "<tr style=\"height:8mm;border:1px solid black\">\r\n"
                        + T10 + "<td class=\"Cambria15_C\" style=\"width:115mm;border:1px solid black\">\r\n"
                        + T11 + "<span>Appréciation des professeurs</span>\r\n"
                        + T10 + "</td>\r\n"
                        + T9 + "</tr>\r\n");

                    foreach (SubjectViewModel subject in _class.Level.Subjects.Where(s => s.ParentSubject == null))
                    {
                        TrimesterSubjectCommentModel trimesterSubjectComment = TrimesterSubjectCommentModel.Read(trimester, student, subject);

                        writer.Write(T9 + $"<tr style=\"height:{11 + 8 * subject.ChildrenSubjects.Count}mm;border:1px solid black\">\r\n"
                            + T10 + "<td class=\"Cambria15_L\" style=\"border:1px solid black\">\r\n"
                            + T11 + $"<span>{(trimesterSubjectComment?.Comment)}</span>\r\n"
                            + T10 + "</td>\r\n"
                            + T9 + "</tr>\r\n");
                    }

                    writer.Write(T8 + "</table>\r\n"
                        + T7 + "</td>\r\n"
                        + T6 + "</tr>\r\n"
                        + T5 + "</table>\r\n"
                        + T4 + "</td>\r\n"
                        + T3 + "</tr>\r\n"
                        + T2 + "</table>\r\n");

                    average = MarkModel.ReadTrimesterAverage(trimester, student);
                    int ranking = MarkModel.ReadTrimesterRanking(student, HTMLUtils_Trimester.StudentAverages);
                    TrimesterCommentModel trimesterComment = TrimesterCommentModel.Read(trimester, student);

                    writer.Write(T2 + "<table style=\"margin-top:2mm;border-collapse:collapse;width:194mm\">\r\n"
                        + T3 + "<tr>\r\n"
                        + T4 + "<td>\r\n"
                        + T5 + "<table style=\"border:1px solid black;\">\r\n"
                        + T6 + "<tr>\r\n"
                        + T7 + "<td>\r\n"
                        + T8 + "<table style=\"border-collapse:collapse\">\r\n"
                        + T9 + "<tr style=\"height:11mm;border:1px solid black\">\r\n"
                        + T10 + "<td class=\"Cambria15B_R\" style=\"width:28mm;border:1px solid black\">\r\n"
                        + T11 + "<span>Moyenne</span>\r\n"
                        + T10 + "</td>\r\n"
                        + T10 + "<td class=\"Cambria15B_C\" style=\"width:13mm;border:1px solid black\">\r\n"
                        + T11 + $"<span>{(average != double.MinValue ? average.ToString("0.0") : "")}</span>\r\n"
                        + T10 + "</td>\r\n"
                        + T9 + "</tr>\r\n"
                        + T8 + "</table>\r\n"
                        + T7 + "</td>\r\n"
                        + T6 + "</tr>\r\n"
                        + T5 + "</table>\r\n"
                        + T5 + "<table style=\"height:2mm;border-collapse:collapse\">\r\n"
                        + T6 + "<tr>\r\n"
                        + T6 + "</tr>\r\n"
                        + T5 + "</table>\r\n"
                        + T5 + "<table style=\"border:1px solid black;\">\r\n"
                        + T6 + "<tr>\r\n"
                        + T7 + "<td>\r\n"
                        + T8 + "<table style=\"border-collapse:collapse\">\r\n"
                        + T9 + "<tr style=\"height:11mm;border:1px solid black\">\r\n"
                        + T10 + "<td class=\"Cambria15B_R\" style=\"width:28mm;border:1px solid black\">\r\n"
                        + T11 + "<span>Classement</span>\r\n"
                        + T10 + "</td>\r\n"
                        + T10 + "<td class=\"Cambria15B_C\" style=\"width:13mm;border:1px solid black\">\r\n"
                        + T11 + $"<span>{ranking}/{HTMLUtils_Trimester.StudentAverages.Count}</span>\r\n"
                        + T10 + "</td>\r\n"
                        + T9 + "</tr>\r\n"
                        + T8 + "</table>\r\n"
                        + T7 + "</td>\r\n"
                        + T6 + "</tr>\r\n"
                        + T5 + "</table>\r\n"
                        + T4 + "</td>\r\n"
                        + T4 + "<td style=\"width:2mm\">\r\n"
                        + T4 + "</td>\r\n"
                        + T4 + "<td>\r\n"
                        + T5 + "<table style=\"height:13mm;border:1px solid black\">\r\n"
                        + T6 + "<tr>\r\n"
                        + T7 + "<td>\r\n"
                        + T8 + "<table style=\"border-collapse:collapse\">\r\n"
                        + T9 + "<tr style=\"height:11mm;border:1px solid black\">\r\n"
                        + T10 + "<td class=\"Cambria15I_C\" style=\"width:10mm;border:1px solid black\">\r\n"
                        + T11 + $"{(HTMLUtils_Trimester.ClassAverage != double.MinValue ? HTMLUtils_Trimester.ClassAverage.ToString("0.0") : "")}\r\n"
                        + T10 + "</td>\r\n"
                        + T10 + "<td class=\"Cambria12I_C\" style=\"width:7.5mm;border:1px solid black\">\r\n"
                        + T11 + $"{(HTMLUtils_Trimester.ClassMinAverage != double.MinValue ? HTMLUtils_Trimester.ClassMinAverage.ToString("0.0") : "")}\r\n"
                        + T10 + "</td>\r\n"
                        + T10 + "<td class=\"Cambria12I_C\" style=\"width:7.5mm;border:1px solid black\">\r\n"
                        + T11 + $"{(HTMLUtils_Trimester.ClassMaxAverage != double.MinValue ? HTMLUtils_Trimester.ClassMaxAverage.ToString("0.0") : "")}\r\n"
                        + T10 + "</td>\r\n"
                        + T9 + "</tr>\r\n"
                        + T8 + "</table>\r\n"
                        + T7 + "</td>\r\n"
                        + T6 + "</tr>\r\n"
                        + T5 + "</table>\r\n"
                        + T5 + "<table style=\"height:15.4mm;border-collapse:collapse\">\r\n"
                        + T6 + "<tr>\r\n"
                        + T6 + "</tr>\r\n"
                        + T5 + "</table>\r\n"
                        + T4 + "</td>\r\n"
                        + T4 + "<td style=\"width:2mm\">\r\n"
                        + T4 + "</td>\r\n"
                        + T4 + "<td>\r\n"
                        + T5 + "<table style=\"border:1px solid black\">\r\n"
                        + T6 + "<tr>\r\n"
                        + T7 + "<td>\r\n"
                        + T8 + "<table style=\"border-collapse:collapse\">\r\n"
                        + T9 + "<tr style=\"height:6.5mm;border:1px solid black\">\r\n"
                        + T10 + "<td class=\"Cambria15_C\" style=\"width:115mm;border:1px solid black\">\r\n"
                        + T11 + "Compte rendu des études\r\n"
                        + T10 + "</td>\r\n"
                        + T9 + "<tr>\r\n"
                        + T9 + "<tr style=\"height:19.5mm;border:1px solid black\">\r\n"
                        + T10 + "<td style=\"border:1px solid black\">\r\n"
                        + T11 + $"<span>{(trimesterComment?.MainTeacherComment ?? "")}</span>\r\n"
                        + T10 + "</td>\r\n"
                        + T9 + "</tr>\r\n"
                        + T8 + "</table>\r\n"
                        + T7 + "</td>\r\n"
                        + T6 + "</tr>\r\n"
                        + T5 + "</table>\r\n"
                        + T4 + "</td>\r\n"
                        + T3 + "</tr>\r\n"
                        + T2 + "</table>\r\n");

                    writer.Write(T2 + "<table style=\"margin-top:2mm;border-collapse:collapse;width:194mm\">\r\n"
                        + T3 + "<tr>\r\n"
                        + T4 + "<td>\r\n"
                        + T5 + "<table style=\"height:21mm;border:1px solid black\">\r\n"
                        + T6 + "<tr>\r\n"
                        + T7 + "<td>\r\n"
                        + T8 + "<table style=\"height:21mm;border-collapse:collapse\">\r\n"
                        + T9 + "<tr style=\"height:6.5mm;border:1px solid black\">\r\n"
                        + T10 + "<td class=\"Cambria15_L\" style=\"width:155mm;border:1px solid black\">\r\n"
                        + T11 + $"<span>Appréciation du préfet de division{(!string.IsNullOrEmpty(MainViewModel.Instance.Parameters.YearParameters.DivisionPrefect) ? " " + MainViewModel.Instance.Parameters.YearParameters.DivisionPrefect : "")}</span>\r\n"
                        + T10 + "</td>\r\n"
                        + T9 + "</tr>\r\n"
                        + T9 + "<tr style=\"height:19mm;border:1px solid black\">\r\n"
                        + T10 + "<td style=\"border:1px solid black\">\r\n"
                        + T11 + $"<span>{(trimesterComment?.DivisionPrefectComment ?? "")}</span>\r\n"
                        + T10 + "</td>\r\n"
                        + T9 + "<tr>\r\n"
                        + T8 + "</table>\r\n"
                        + T7 + "</td>\r\n"
                        + T6 + "</tr>\r\n"
                        + T5 + "</table>\r\n"
                        + T4 + "</td>\r\n"
                        + T4 + "<td style=\"width:2mm\">\r\n"
                        + T4 + "</td>\r\n"
                        + T4 + "<td>\r\n"
                        + T5 + "<table style=\"height:21mm;border:1px solid black\">\r\n"
                        + T6 + "<tr>\r\n"
                        + T7 + "<td>\r\n"
                        + T8 + "<table style=\"height:21mm;border-collapse:collapse\">\r\n"
                        + T9 + "<tr style=\"height:6.5mm;border:1px solid black\">\r\n"
                        + T10 + "<td class=\"Cambria12I_L\" style=\"width:39mm;border:1px solid black\">\r\n"
                        + T11 + "<span>Signature des parents :</span>\r\n"
                        + T10 + "</td>\r\n"
                        + T9 + "<tr>\r\n"
                        + T9 + "<tr style=\"height:19mm;border:1px solid black\">\r\n"
                        + T10 + "<td style=\"border:1px solid black\">\r\n"
                        + T10 + "</td>\r\n"
                        + T9 + "<tr>\r\n"
                        + T8 + "</table>\r\n"
                        + T7 + "</td>\r\n"
                        + T6 + "</tr>\r\n"
                        + T5 + "</table>\r\n"
                        + T4 + "</td>\r\n"
                        + T3 + "</tr>\r\n"
                        + T2 + "</table>\r\n");

                    WriteAdressFooter(writer);
                    WriteFooter(writer);
                }
            }

            MainViewModel.Instance.Reports.TrimesterReports.Add(Path.GetFileName(filename));
        }

        static public void CreateYearReport(int year, MainWindow.UpdateYearReportsDelegate _updateYearReportsDispatch)
        {
            string directory = FileUtils.SelectDirectory(Settings.Settings.Instance.LastSelectedDirectoryYearReports, "LastSelectedDirectoryYearReports");

            if (!string.IsNullOrEmpty(directory))
            {
                try
                {
                    CopyIcone(directory);

                    MainViewModel.Instance.Reports.TrimesterReports.Clear();

                    int classCount = 0;
                    foreach (ClassViewModel _class in MainViewModel.Instance.Parameters.Classes)
                    {
                        HTMLUtils_Year HTMLUtils_Year = new HTMLUtils_Year();
                        foreach (SubjectViewModel subject in _class.Level.Subjects)
                        {
                            if (subject.ChildrenSubjects.Any())
                            {
                                double average = MarkModel.ReadYearClassMainSubjectAverage(year, _class, subject);
                                HTMLUtils_Year.ClassSubjectAverages.Add(subject, average);
                                MarkModel.ReadYearMinMaxMainSubjectAverage(year, _class, subject, out double minAverage, out double maxAverage);
                                HTMLUtils_Year.ClassSubjectMinAverages.Add(subject, minAverage);
                                HTMLUtils_Year.ClassSubjectMaxAverages.Add(subject, maxAverage);
                                foreach (SubjectViewModel subject2 in subject.ChildrenSubjects.OrderBy(s => s.Order))
                                {
                                    average = MarkModel.ReadYearClassSubjectAverage(year, _class, subject2);
                                    HTMLUtils_Year.ClassSubjectAverages.Add(subject2, average);
                                    MarkModel.ReadYearMinMaxSubjectAverage(year, _class, subject2, out minAverage, out maxAverage);
                                    HTMLUtils_Year.ClassSubjectMinAverages.Add(subject2, minAverage);
                                    HTMLUtils_Year.ClassSubjectMaxAverages.Add(subject2, maxAverage);
                                }
                            }
                            else
                            {
                                double average = MarkModel.ReadYearClassSubjectAverage(year, _class, subject);
                                HTMLUtils_Year.ClassSubjectAverages.Add(subject, average);
                                MarkModel.ReadYearMinMaxSubjectAverage(year, _class, subject, out double minAverage, out double maxAverage);
                                HTMLUtils_Year.ClassSubjectMinAverages.Add(subject, minAverage);
                                HTMLUtils_Year.ClassSubjectMaxAverages.Add(subject, maxAverage);
                            }
                        }
                        HTMLUtils_Year.ClassAverage = MarkModel.ReadYearClassAverage(year, _class);
                        MarkModel.ReadYearMinMaxAverage(year, _class, out double classMinAverage, out double classMaxAverage);
                        HTMLUtils_Year.ClassMinAverage = classMinAverage;
                        HTMLUtils_Year.ClassMaxAverage = classMaxAverage;
                        foreach (StudentViewModel student in _class.Students)
                        {
                            HTMLUtils_Year.StudentAverages[student] = MarkModel.ReadYearAverage(year, student);
                        }
                        int studentCount = 0;
                        foreach (StudentViewModel student in _class.Students.Where(s => HTMLUtils_Year.StudentAverages[s] != double.MinValue))
                        {
                            try
                            {
                                GenerateYearReport(directory, year, student, _class, HTMLUtils_Year);
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show(e.Message);
                            }

                            studentCount++;
                            _updateYearReportsDispatch(studentCount * 800 / HTMLUtils_Year.StudentAverages.Count / MainViewModel.Instance.Parameters.Classes.Count
                                + classCount * 800 / MainViewModel.Instance.Parameters.Classes.Count);
                        }
                        classCount++;
                    }

                    string filename = Path.Combine(directory, $"Bulletin de l'année {year}-{year + 1} (regroupement).html");
                    File.Delete(filename);

                    MergeHTML(directory, MainViewModel.Instance.Reports.YearReports, filename);

                    MainViewModel.Instance.Reports.YearReports.Insert(0, Path.GetFileName(filename));
                    _updateYearReportsDispatch(1000);

                    MainViewModel.Instance.Reports.YearReportsPath = directory;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        private static void GenerateYearReport(string directory, int year, StudentViewModel student, ClassViewModel _class, HTMLUtils_Year HTMLUtils_Year)
        {
            PeriodViewModel period = MainViewModel.Instance.Models.Periods.OrderBy(p => p.Number).FirstOrDefault();

            string filename = Path.Combine(directory, $"Bulletin de l'année {year}-{year + 1} de {_class.Name} de {student.LastName} {student.FirstName}.html");

            File.Delete(filename);
            using (FileStream file = new FileStream(filename, FileMode.OpenOrCreate))
            {
                using (StreamWriter writer = new StreamWriter(file))
                {
                    WriteHeader(writer);

                    writer.Write(T2 + "<table style=\"height:7mm;border-collapse:collapse\">\r\n"
                        + T3 + "<tr>\r\n"
                        + T4 + "<td>\r\n"
                        + T4 + "</td>\r\n"
                        + T3 + "<tr>\r\n"
                        + T2 + "</table>\r\n"
                        + T2 + "<table style=\"width:194mm;border-collapse:collapse\">\r\n"
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
                        + T8 + "BULLETIN ANNUEL\r\n"
                        + T7 + "</td>\r\n"
                        + T6 + "</tr>\r\n"
                        + T6 + "<tr style=\"height:7mm\">\r\n"
                        + T7 + "<td class=\"Cambria19_C\">\r\n"
                        + T8 + $"Année {student.Year} - {student.Year + 1}\r\n"
                        + T7 + "</td>\r\n"
                        + T6 + "</tr>\r\n"
                        + T6 + "<tr style=\"height:6mm\">\r\n"
                        + T7 + "<td class=\"Cambria15_C\">\r\n"
                        + T8 + $"Classe de {_class.Name} - Effectif {_class.Students.Count}\r\n"
                        + T7 + "</td>\r\n"
                        + T6 + "</tr>\r\n"
                        + T6 + "<tr style=\"height:12mm\">\r\n"
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

                    writer.Write(T2 + "<table style=\"margin-top:2mm;border-collapse:collapse;width:194mm\">\r\n"
                        + T3 + "<tr>\r\n"
                        + T4 + "<td>\r\n"
                        + T5 + "<table style=\"border:1px solid black;\">\r\n"
                        + T6 + "<tr>\r\n"
                        + T7 + "<td>\r\n"
                        + T8 + "<table style=\"border-collapse:collapse\">\r\n"
                        + T9 + "<tr style=\"height:8mm;border:1px solid black\">\r\n"
                        + T10 + "<td class=\"Cambria15_C\" style=\"width:28mm;border:1px solid black\">\r\n"
                        + T11 + "<span>Disciplines</span>\r\n"
                        + T10 + "</td>\r\n"
                        + T10 + "<td class=\"Cambria11_C\" style=\"width:13mm;border:1px solid black\">\r\n"
                        + T11 + "<span>Moyenne élève</span>\r\n"
                        + T10 + "</td>\r\n"
                        + T9 + "</tr>\r\n");

                    double average = double.MinValue;
                    foreach (SubjectViewModel subject in _class.Level.Subjects.Where(s => s.ParentSubject == null))
                    {
                        TeacherViewModel teacher = ModelUtils.GetTeacherFromClassAndSubject(student.Class, subject, period);

                        average = double.MinValue;
                        if (subject.ChildrenSubjects.Any())
                        {
                            average = MarkModel.ReadYearMainSubjectAverage(year, student, subject);
                        }
                        else
                        {
                            average = MarkModel.ReadYearSubjectAverage(year, student, subject);
                        }

                        writer.Write(T9 + "<tr style=\"height:11mm;border:1px solid black\">\r\n"
                            + T10 + "<td style=\"border:1px solid black\">\r\n"
                            + T11 + "<span class=\"Cambria10B_L\">\r\n"
                            + T12 + $"{subject.Name.ToUpper()}\r\n"
                            + T11 + "</span><br/>\r\n"
                            + T11 + "<span class=\"Cambria9I_L\">\r\n"
                            + T12 + $"{(subject.Option ? $"(option " : $"(")}coeff. {subject.Coefficient})\r\n"
                            + T11 + "</span><br/>\r\n"
                            + T11 + "<span class=\"Cambria9I_L\">\r\n"
                            + T12 + $"{(teacher != null ? $"{teacher.Title} {(!string.IsNullOrEmpty(teacher.FirstName) ? teacher.FirstName.Substring(0, 1) : "")}. {teacher.LastName}" : "")}\r\n"
                            + T11 + "</span><br/>\r\n"
                            + T10 + "</td>\r\n"
                            + T10 + "<td class=\"Cambria17B_C\" style=\"border:1px solid black\">\r\n"
                            + T11 + $"{(average != double.MinValue ? average.ToString("0.0") : "")}\r\n"
                            + T10 + "</td>\r\n"
                            + T9 + "</tr>\r\n");

                        foreach (SubjectViewModel subject2 in subject.ChildrenSubjects.OrderBy(s => s.Order))
                        {
                            average = MarkModel.ReadYearSubjectAverage(year, student, subject2);
                            writer.Write(T9 + "<tr style=\"height:8mm;border:1px solid black\">\r\n"
                                + T10 + "<td style=\"border:1px solid black\">\r\n"
                                + T11 + "<span class=\"Cambria12I_R\">\r\n"
                                + T12 + $"{subject2.Name.ToUpper()}\r\n"
                                + T11 + "</span><br/>\r\n"
                                + T11 + "<span class=\"Cambria9I_R\">\r\n"
                                + T12 + $"{(subject2.Option ? $"(option " : $"(")}coeff. {subject2.Coefficient})\r\n"
                                + T11 + "</span><br/>\r\n"
                                + T10 + "</td>\r\n"
                                + T10 + "<td class=\"Cambria17B_C\" style=\"border:1px solid black\">\r\n"
                                + T11 + $"{(average != double.MinValue ? average.ToString("0.0") : "")}\r\n"
                                + T10 + "</td>\r\n"
                                + T9 + "</tr>\r\n");
                        }
                    }

                    writer.Write(T8 + "</table>\r\n"
                        + T7 + "</td>\r\n"
                        + T6 + "</tr>\r\n"
                        + T5 + "</table>\r\n"
                        + T4 + "</td>\r\n"
                        + T4 + "<td style=\"width:2mm\">\r\n"
                        + T4 + "</td>\r\n"
                        + T4 + "<td>\r\n"
                        + T5 + "<table style=\"border:1px solid black\">\r\n"
                        + T6 + "<tr>\r\n"
                        + T7 + "<td>\r\n"
                        + T8 + "<table style=\"border-collapse:collapse\">\r\n"
                        + T9 + "<tr style=\"height:8mm;border:1px solid black\">\r\n"
                        + T10 + "<td class=\"Cambria11_C\" style=\"width:10mm;border:1px solid black\">\r\n"
                        + T11 + "<span>Moy. classe</span>\r\n"
                        + T10 + "</td>\r\n"
                        + T10 + "<td class=\"Cambria10_C\" style=\"width:7.5mm;border:1px solid black\">\r\n"
                        + T11 + "<span>Moy. min</span>\r\n"
                        + T10 + "</td>\r\n"
                        + T10 + "<td class=\"Cambria10_C\" style=\"width:7.5mm;border:1px solid black\">\r\n"
                        + T11 + "<span>Moy. max</span>\r\n"
                        + T10 + "</td>\r\n"
                        + T9 + "</tr>\r\n");

                    foreach (SubjectViewModel subject in _class.Level.Subjects.Where(s => s.ParentSubject == null))
                    {
                        writer.Write(T9 + "<tr style=\"height:11mm;border:1px solid black\">\r\n"
                            + T10 + "<td class=\"Cambria15I_C\" style=\"border:1px solid black\">\r\n"
                            + T11 + $"{(HTMLUtils_Year.ClassSubjectAverages[subject] != double.MinValue ? HTMLUtils_Year.ClassSubjectAverages[subject].ToString("0.0") : "")}\r\n"
                            + T10 + "</td>\r\n"
                            + T10 + "<td class=\"Cambria12I_C\" style=\"border:1px solid black\">\r\n"
                            + T11 + $"{(HTMLUtils_Year.ClassSubjectMinAverages[subject] != double.MaxValue ? HTMLUtils_Year.ClassSubjectMinAverages[subject].ToString("0.0") : "")}\r\n"
                            + T10 + "</td>\r\n"
                            + T10 + "<td class=\"Cambria12I_C\" style=\"border:1px solid black\">\r\n"
                            + T11 + $"{(HTMLUtils_Year.ClassSubjectMaxAverages[subject] != double.MinValue ? HTMLUtils_Year.ClassSubjectMaxAverages[subject].ToString("0.0") : "")}\r\n"
                            + T10 + "</td>\r\n"
                            + T9 + "</tr>\r\n");

                        foreach (SubjectViewModel subject2 in subject.ChildrenSubjects.OrderBy(s => s.Order))
                        {
                            writer.Write(T9 + "<tr style=\"height:8mm;border:1px solid black\">\r\n"
                                + T10 + "<td class=\"Cambria15I_C\" style=\"border:1px solid black\">\r\n"
                                + T11 + $"{(HTMLUtils_Year.ClassSubjectAverages[subject2] != double.MinValue ? HTMLUtils_Year.ClassSubjectAverages[subject2].ToString("0.0") : "")}\r\n"
                                + T10 + "</td>\r\n"
                                + T10 + "<td class=\"Cambria12I_C\" style=\"border:1px solid black\">\r\n"
                                + T11 + $"{(HTMLUtils_Year.ClassSubjectMinAverages[subject2] != double.MaxValue ? HTMLUtils_Year.ClassSubjectMinAverages[subject2].ToString("0.0") : "")}\r\n"
                                + T10 + "</td>\r\n"
                                + T10 + "<td class=\"Cambria12I_C\" style=\"border:1px solid black\">\r\n"
                                + T11 + $"{(HTMLUtils_Year.ClassSubjectMaxAverages[subject2] != double.MinValue ? HTMLUtils_Year.ClassSubjectMaxAverages[subject2].ToString("0.0") : "")}\r\n"
                                + T10 + "</td>\r\n"
                                + T9 + "</tr>\r\n");
                        }
                    }

                    writer.Write(T8 + "</table>\r\n"
                        + T7 + "</td>\r\n"
                        + T6 + "</tr>\r\n"
                        + T5 + "</table>\r\n"
                        + T4 + "</td>\r\n"
                        + T4 + "<td style=\"width:2mm\">\r\n"
                        + T4 + "</td>\r\n"
                        + T4 + "<td>\r\n"
                        + T5 + "<table style=\"border:1px solid black\">\r\n"
                        + T6 + "<tr>\r\n"
                        + T7 + "<td>\r\n"
                        + T8 + "<table style=\"border-collapse:collapse\">\r\n"
                        + T9 + "<tr style=\"height:8mm;border:1px solid black\">\r\n");
                    for (int trimester = 1; trimester <= 3; trimester++)
                    {
                        writer.Write(T10 + "<td class=\"Cambria12_C\" style=\"width:38.3mm;border:1px solid black\">\r\n"
                            + T11 + $"<span>Trimestre {trimester}</span>\r\n"
                            + T10 + "</td>\r\n");
                    }
                    writer.Write(T9 + "</tr>\r\n");

                    foreach (SubjectViewModel subject in _class.Level.Subjects.Where(s => s.ParentSubject == null))
                    {
                        writer.Write(T9 + "<tr style=\"height:11mm;border:1px solid black\">\r\n");
                        for (int trimester = 1; trimester <= 3; trimester++)
                        {
                            if (subject.ChildrenSubjects.Any())
                            {
                                average = MarkModel.ReadTrimesterMainSubjectAverage(trimester, student, subject);
                            }
                            else
                            {
                                average = MarkModel.ReadTrimesterSubjectAverage(trimester, student, subject);
                            }
                            writer.Write(T10 + "<td class=\"Cambria15I_C\" style=\"border:1px solid black\">\r\n"
                                + T11 + $"<span>{(average != double.MinValue ? average.ToString("0.0") : "")}</span>\r\n"
                                + T10 + "</td>\r\n");
                        }
                        writer.Write(T9 + "</tr>\r\n");
                        foreach (SubjectViewModel subject2 in subject.ChildrenSubjects.OrderBy(s => s.Order))
                        {
                            writer.Write(T9 + "<tr style=\"height:8mm;border:1px solid black\">\r\n");
                            for (int trimester = 1; trimester <= 3; trimester++)
                            {
                                average = MarkModel.ReadTrimesterSubjectAverage(trimester, student, subject2);
                                writer.Write(T10 + "<td class=\"Cambria15I_C\" style=\"border:1px solid black\">\r\n"
                                    + T11 + $"<span>{(average != double.MinValue ? average.ToString("0.0") : "")}</span>\r\n"
                                    + T10 + "</td>\r\n");
                            }
                            writer.Write(T9 + "</tr>\r\n");
                        }
                    }

                    writer.Write(T8 + "</table>\r\n"
                        + T7 + "</td>\r\n"
                        + T6 + "</tr>\r\n"
                        + T5 + "</table>\r\n"
                        + T4 + "</td>\r\n"
                        + T3 + "</tr>\r\n"
                        + T2 + "</table>\r\n");

                    average = MarkModel.ReadYearAverage(year, student);
                    int ranking = MarkModel.ReadYearRanking(student, HTMLUtils_Year.StudentAverages);

                    writer.Write(T2 + "<table style=\"margin-top:2mm;border-collapse:collapse;width:194mm\">\r\n"
                        + T3 + "<tr>\r\n"
                        + T4 + "<td>\r\n"
                        + T5 + "<table style=\"border:1px solid black;\">\r\n"
                        + T6 + "<tr>\r\n"
                        + T7 + "<td>\r\n"
                        + T8 + "<table style=\"border-collapse:collapse\">\r\n"
                        + T9 + "<tr style=\"height:11mm;border:1px solid black\">\r\n"
                        + T10 + "<td class=\"Cambria15B_R\" style=\"width:28mm;border:1px solid black\">\r\n"
                        + T11 + "<span>Moyenne</span>\r\n"
                        + T10 + "</td>\r\n"
                        + T10 + "<td class=\"Cambria15B_C\" style=\"width:13mm;border:1px solid black\">\r\n"
                        + T11 + $"<span>{(average != double.MinValue ? average.ToString("0.0") : "")}</span>\r\n"
                        + T10 + "</td>\r\n"
                        + T9 + "</tr>\r\n"
                        + T8 + "</table>\r\n"
                        + T7 + "</td>\r\n"
                        + T6 + "</tr>\r\n"
                        + T5 + "</table>\r\n"
                        + T4 + "</td>\r\n"
                        + T4 + "<td style=\"width:2mm\">\r\n"
                        + T4 + "</td>\r\n"
                        + T4 + "<td>\r\n"
                        + T5 + "<table style=\"height:13mm;border:1px solid black\">\r\n"
                        + T6 + "<tr>\r\n"
                        + T7 + "<td>\r\n"
                        + T8 + "<table style=\"border-collapse:collapse\">\r\n"
                        + T9 + "<tr style=\"height:11mm;border:1px solid black\">\r\n"
                        + T10 + "<td class=\"Cambria15I_C\" style=\"width:10mm;border:1px solid black\">\r\n"
                        + T11 + $"<span>{(HTMLUtils_Year.ClassAverage != double.MinValue ? HTMLUtils_Year.ClassAverage.ToString("0.0") : "")}</span>\r\n"
                        + T10 + "</td>\r\n"
                        + T10 + "<td class=\"Cambria12I_C\" style=\"width:7.5mm;border:1px solid black\">\r\n"
                        + T11 + $"<span>{(HTMLUtils_Year.ClassMinAverage != double.MinValue ? HTMLUtils_Year.ClassMinAverage.ToString("0.0") : "")}</span>\r\n"
                        + T10 + "</td>\r\n"
                        + T10 + "<td class=\"Cambria12I_C\" style=\"width:7.5mm;border:1px solid black\">\r\n"
                        + T11 + $"<span>{(HTMLUtils_Year.ClassMaxAverage != double.MinValue ? HTMLUtils_Year.ClassMaxAverage.ToString("0.0") : "")}</span>\r\n"
                        + T10 + "</td>\r\n"
                        + T9 + "</tr>\r\n"
                        + T8 + "</table>\r\n"
                        + T7 + "</td>\r\n"
                        + T6 + "</tr>\r\n"
                        + T5 + "</table>\r\n"
                        + T4 + "</td>\r\n"
                        + T4 + "<td style=\"width:2mm\">\r\n"
                        + T4 + "</td>\r\n"
                        + T4 + "<td>\r\n"
                        + T5 + "<table style=\"border:1px solid black\">\r\n"
                        + T6 + "<tr>\r\n"
                        + T7 + "<td>\r\n"
                        + T8 + "<table style=\"border-collapse:collapse\">\r\n"
                        + T9 + "<tr style=\"height:11mm;border:1px solid black\">\r\n");
                    for (int trimester = 1; trimester <= 3; trimester++)
                    {
                        average = MarkModel.ReadTrimesterAverage(trimester, student);
                        writer.Write(T10 + "<td class=\"Cambria15I_C\" style=\"width:38.3mm;border:1px solid black\">\r\n"
                            + T11 + $"<span>{(average != double.MinValue ? average.ToString("0.0") : "")}</span>\r\n"
                            + T10 + "</td>\r\n");
                    }
                    writer.Write(T9 + "<tr>\r\n"
                        + T8 + "</table>\r\n"
                        + T7 + "</td>\r\n"
                        + T6 + "</tr>\r\n"
                        + T5 + "</table>\r\n"
                        + T4 + "</td>\r\n"
                        + T3 + "</tr>\r\n"
                        + T2 + "</table>\r\n");

                    writer.Write(T2 + "<table style=\"margin-top:2mm;border-collapse:collapse;width:194mm\">\r\n"
                        + T3 + "<tr>\r\n"
                        + T4 + "<td>\r\n"
                        + T5 + "<table style=\"border:1px solid black;\">\r\n"
                        + T6 + "<tr>\r\n"
                        + T7 + "<td>\r\n"
                        + T8 + "<table style=\"border-collapse:collapse\">\r\n"
                        + T9 + "<tr style=\"height:11mm;border:1px solid black\">\r\n"
                        + T10 + "<td class=\"Cambria15B_R\" style=\"width:28mm;border:1px solid black\">\r\n"
                        + T11 + "<span>Classement</span>\r\n"
                        + T10 + "</td>\r\n"
                        + T10 + "<td class=\"Cambria15B_C\" style=\"width:13mm;border:1px solid black\">\r\n"
                        + T11 + $"<span>{ranking}/{HTMLUtils_Year.StudentAverages.Count}</span>\r\n"
                        + T10 + "</td>\r\n"
                        + T9 + "</tr>\r\n"
                        + T8 + "</table>\r\n"
                        + T7 + "</td>\r\n"
                        + T6 + "</tr>\r\n"
                        + T5 + "</table>\r\n"
                        + T4 + "</td>\r\n"
                        + T4 + "<td style=\"width:149mm\">\r\n"
                        + T4 + "</td>\r\n"
                        + T3 + "</tr>\r\n"
                        + T2 + "</table>\r\n");

                    WriteAdressFooter(writer);
                    WriteFooter(writer);
                }
            }

            MainViewModel.Instance.Reports.YearReports.Add(Path.GetFileName(filename));
        }
    }
}

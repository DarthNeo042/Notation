using Notation.ViewModels;
using Notation.Views;
using System.Collections.Generic;

namespace Notation.Utils
{
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
        static public void CreateYearReport(int year, MainWindow.UpdateYearReportsDelegate _updateYearReportsDispatch)
        {
            //string directory = FileUtils.SelectDirectory(Settings.Settings.Instance.LastSelectedDirectoryYearReports, "LastSelectedDirectoryYearReports");

            //if (!string.IsNullOrEmpty(directory))
            //{
            //    try
            //    {
            //        MainViewModel.Instance.Reports.YearReports.Clear();

            //        int classCount = 0;
            //        foreach (ClassViewModel _class in MainViewModel.Instance.Parameters.Classes)
            //        {
            //            SSRSUtils_Year SSRSUtils_Year = new SSRSUtils_Year();
            //            foreach (SubjectViewModel subject in _class.Level.Subjects)
            //            {
            //                if (subject.ChildrenSubjects.Any())
            //                {
            //                    double average = MarkModel.ReadYearClassMainSubjectAverage(year, _class, subject);
            //                    SSRSUtils_Year.ClassSubjectAverages.Add(subject, average);
            //                    MarkModel.ReadYearMinMaxMainSubjectAverage(year, _class, subject, out double minAverage, out double maxAverage);
            //                    SSRSUtils_Year.ClassSubjectMinAverages.Add(subject, minAverage);
            //                    SSRSUtils_Year.ClassSubjectMaxAverages.Add(subject, maxAverage);
            //                    foreach (SubjectViewModel subject2 in subject.ChildrenSubjects.OrderBy(s => s.Order))
            //                    {
            //                        average = MarkModel.ReadYearClassSubjectAverage(year, _class, subject2);
            //                        SSRSUtils_Year.ClassSubjectAverages.Add(subject2, average);
            //                        MarkModel.ReadYearMinMaxSubjectAverage(year, _class, subject2, out minAverage, out maxAverage);
            //                        SSRSUtils_Year.ClassSubjectMinAverages.Add(subject2, minAverage);
            //                        SSRSUtils_Year.ClassSubjectMaxAverages.Add(subject2, maxAverage);
            //                    }
            //                }
            //                else
            //                {
            //                    double average = MarkModel.ReadYearClassSubjectAverage(year, _class, subject);
            //                    SSRSUtils_Year.ClassSubjectAverages.Add(subject, average);
            //                    MarkModel.ReadYearMinMaxSubjectAverage(year, _class, subject, out double minAverage, out double maxAverage);
            //                    SSRSUtils_Year.ClassSubjectMinAverages.Add(subject, minAverage);
            //                    SSRSUtils_Year.ClassSubjectMaxAverages.Add(subject, maxAverage);
            //                }
            //            }
            //            SSRSUtils_Year.ClassAverage = MarkModel.ReadYearClassAverage(year, _class);
            //            MarkModel.ReadYearMinMaxAverage(year, _class, out double classMinAverage, out double classMaxAverage);
            //            SSRSUtils_Year.ClassMinAverage = classMinAverage;
            //            SSRSUtils_Year.ClassMaxAverage = classMaxAverage;
            //            foreach (StudentViewModel student in _class.Students)
            //            {
            //                SSRSUtils_Year.StudentAverages[student] = MarkModel.ReadYearAverage(year, student);
            //            }
            //            int studentCount = 0;
            //            foreach (StudentViewModel student in _class.Students.Where(s => SSRSUtils_Year.StudentAverages[s] != double.MinValue))
            //            {
            //                try
            //                {
            //                    GenerateYearReport(directory, year, student, _class, SSRSUtils_Year);
            //                }
            //                catch (Exception e)
            //                {
            //                    MessageBox.Show(e.Message);
            //                }

            //                studentCount++;
            //                _updateYearReportsDispatch(studentCount * 1000 / SSRSUtils_Year.StudentAverages.Count
            //                    + classCount * 1000 / SSRSUtils_Year.StudentAverages.Count / MainViewModel.Instance.Parameters.Classes.Count);
            //            }
            //        }

            //        MainViewModel.Instance.Reports.YearReportsPath = directory;
            //    }
            //    catch (Exception e)
            //    {
            //        MessageBox.Show(e.Message);
            //    }
            //}
        }

        private static void GenerateYearReport(string directory, int year, StudentViewModel student, ClassViewModel _class, SSRSUtils_Year SSRSUtils_Year)
        {
            //PeriodViewModel period = MainViewModel.Instance.Models.Periods.OrderBy(p => p.Number).FirstOrDefault();

            //string filename = Path.Combine(directory, $"Bulletin de l'année {year}-{year + 1} de {_class.Name} de {student.LastName} {student.FirstName}.pdf");

            //ReportViewer report = new ReportViewer();
            //report.LocalReport.ReportPath = @".\Reports\BulletinAnnuel.rdlc";

            //BulletinAnnuelHeaderDataSource bulletinAnnuelHeader = new BulletinAnnuelHeaderDataSource()
            //{
            //    BirthDate = $"né le {student.BirthDate.ToShortDateString()}",
            //    FirstName = student.FirstName,
            //    LastName = student.LastName,
            //    Header1 = $"Année {student.Year} - {student.Year + 1}",
            //    Header2 = $"Classe de {_class.Name} - Effectif {_class.Students.Count}",
            //};

            //double average = MarkModel.ReadYearAverage(year, student);
            //if (average != double.MinValue)
            //{
            //    bulletinAnnuelHeader.Average = average.ToString("0.0");
            //}
            //if (SSRSUtils_Year.ClassAverage != double.MinValue)
            //{
            //    bulletinAnnuelHeader.ClassAverage = SSRSUtils_Year.ClassAverage.ToString("0.0");
            //}
            //if (SSRSUtils_Year.ClassMinAverage != double.MaxValue)
            //{
            //    bulletinAnnuelHeader.ClassMinAverage = SSRSUtils_Year.ClassMinAverage.ToString("0.0");
            //}
            //if (SSRSUtils_Year.ClassMaxAverage != double.MinValue)
            //{
            //    bulletinAnnuelHeader.ClassMaxAverage = SSRSUtils_Year.ClassMaxAverage.ToString("0.0");
            //}
            //int ranking = MarkModel.ReadYearRanking(student, SSRSUtils_Year.StudentAverages);
            //bulletinAnnuelHeader.Ranking = $"{ranking}/{SSRSUtils_Year.StudentAverages.Count}";

            //List<BulletinAnnuelLineDataSource> bulletinAnnuelLines = new List<BulletinAnnuelLineDataSource>();

            //foreach (SubjectViewModel subject in _class.Level.Subjects.Where(s => s.ParentSubject == null))
            //{
            //    BulletinAnnuelLineDataSource bulletinAnnuelLine = new BulletinAnnuelLineDataSource()
            //    {
            //        IsChildSubject = false,
            //        Subject = subject.Name.ToUpper(),
            //        ClassAverage = SSRSUtils_Year.ClassSubjectAverages[subject] != double.MinValue ? SSRSUtils_Year.ClassSubjectAverages[subject].ToString("0.0") : "",
            //        ClassMinAverage = SSRSUtils_Year.ClassSubjectMinAverages[subject] != double.MaxValue ? SSRSUtils_Year.ClassSubjectMinAverages[subject].ToString("0.0") : "",
            //        ClassMaxAverage = SSRSUtils_Year.ClassSubjectMaxAverages[subject] != double.MinValue ? SSRSUtils_Year.ClassSubjectMaxAverages[subject].ToString("0.0") : "",
            //    };
            //    if (subject.Option)
            //    {
            //        bulletinAnnuelLine.Coefficient = $"(option coeff. {subject.Coefficient})";
            //    }
            //    else
            //    {
            //        bulletinAnnuelLine.Coefficient = $"(coeff. {subject.Coefficient})";
            //    }
            //    TeacherViewModel teacher = ModelUtils.GetTeacherFromClassAndSubject(student.Class, subject, period);
            //    if (teacher != null)
            //    {
            //        bulletinAnnuelLine.Teacher = $"{teacher.Title} {(!string.IsNullOrEmpty(teacher.FirstName) ? teacher.FirstName.Substring(0, 1) : "")}. {teacher.LastName}";
            //    }
            //    average = double.MinValue;
            //    if (subject.ChildrenSubjects.Any())
            //    {
            //        average = MarkModel.ReadYearMainSubjectAverage(year, student, subject);
            //    }
            //    else
            //    {
            //        average = MarkModel.ReadYearSubjectAverage(year, student, subject);
            //    }
            //    if (average != double.MinValue)
            //    {
            //        bulletinAnnuelLine.Average = average.ToString("0.0");
            //    }
            //    if (subject.ChildrenSubjects.Any())
            //    {
            //        average = MarkModel.ReadTrimesterMainSubjectAverage(1, student, subject);
            //    }
            //    else
            //    {
            //        average = MarkModel.ReadTrimesterSubjectAverage(1, student, subject);
            //    }
            //    if (average != double.MinValue)
            //    {
            //        bulletinAnnuelLine.Marks1 = average.ToString("0.0");
            //    }
            //    if (subject.ChildrenSubjects.Any())
            //    {
            //        average = MarkModel.ReadTrimesterMainSubjectAverage(2, student, subject);
            //    }
            //    else
            //    {
            //        average = MarkModel.ReadTrimesterSubjectAverage(2, student, subject);
            //    }
            //    if (average != double.MinValue)
            //    {
            //        bulletinAnnuelLine.Marks2 = average.ToString("0.0");
            //    }
            //    if (subject.ChildrenSubjects.Any())
            //    {
            //        average = MarkModel.ReadTrimesterMainSubjectAverage(3, student, subject);
            //    }
            //    else
            //    {
            //        average = MarkModel.ReadTrimesterSubjectAverage(3, student, subject);
            //    }
            //    if (average != double.MinValue)
            //    {
            //        bulletinAnnuelLine.Marks3 = average.ToString("0.0");
            //    }
            //    bulletinAnnuelLines.Add(bulletinAnnuelLine);

            //    foreach (SubjectViewModel subject2 in subject.ChildrenSubjects.OrderBy(s => s.Order))
            //    {
            //        bulletinAnnuelLine = new BulletinAnnuelLineDataSource()
            //        {
            //            IsChildSubject = true,
            //            Subject = subject2.Name.ToUpper(),
            //            ClassAverage = SSRSUtils_Year.ClassSubjectAverages[subject2] != double.MinValue ? SSRSUtils_Year.ClassSubjectAverages[subject2].ToString("0.0") : "",
            //            ClassMinAverage = SSRSUtils_Year.ClassSubjectMinAverages[subject2] != double.MaxValue ? SSRSUtils_Year.ClassSubjectMinAverages[subject2].ToString("0.0") : "",
            //            ClassMaxAverage = SSRSUtils_Year.ClassSubjectMaxAverages[subject2] != double.MinValue ? SSRSUtils_Year.ClassSubjectMaxAverages[subject2].ToString("0.0") : "",
            //        };
            //        if (subject.Option)
            //        {
            //            bulletinAnnuelLine.Coefficient = $"(option coeff. {subject2.Coefficient})";
            //        }
            //        else
            //        {
            //            bulletinAnnuelLine.Coefficient = $"(coeff. {subject2.Coefficient})";
            //        }
            //        average = MarkModel.ReadYearSubjectAverage(year, student, subject2);
            //        if (average != double.MinValue)
            //        {
            //            bulletinAnnuelLine.Average = average.ToString("0.0");
            //        }
            //        average = MarkModel.ReadTrimesterSubjectAverage(1, student, subject2);
            //        if (average != double.MinValue)
            //        {
            //            bulletinAnnuelLine.Marks1 = average.ToString("0.0");
            //        }
            //        average = MarkModel.ReadTrimesterSubjectAverage(2, student, subject2);
            //        if (average != double.MinValue)
            //        {
            //            bulletinAnnuelLine.Marks2 = average.ToString("0.0");
            //        }
            //        average = MarkModel.ReadTrimesterSubjectAverage(3, student, subject2);
            //        if (average != double.MinValue)
            //        {
            //            bulletinAnnuelLine.Marks3 = average.ToString("0.0");
            //        }

            //        bulletinAnnuelLines.Add(bulletinAnnuelLine);
            //    }
            //}

            //average = MarkModel.ReadTrimesterAverage(1, student);
            //if (average != double.MinValue)
            //{
            //    bulletinAnnuelHeader.Average1 = average.ToString("0.0");
            //}
            //average = MarkModel.ReadTrimesterAverage(2, student);
            //if (average != double.MinValue)
            //{
            //    bulletinAnnuelHeader.Average2 = average.ToString("0.0");
            //}
            //average = MarkModel.ReadTrimesterAverage(3, student);
            //if (average != double.MinValue)
            //{
            //    bulletinAnnuelHeader.Average3 = average.ToString("0.0");
            //}

            //ReportDataSource dataSource = new ReportDataSource("HeaderDataSet", new List<BulletinAnnuelHeaderDataSource>() { bulletinAnnuelHeader });
            //report.LocalReport.DataSources.Add(dataSource);

            //dataSource = new ReportDataSource("LineDataSet", bulletinAnnuelLines);
            //report.LocalReport.DataSources.Add(dataSource);

            //byte[] bytes = report.LocalReport.Render("PDF", null);

            //using (FileStream fileStream = new FileStream(filename, FileMode.Create))
            //{
            //    fileStream.Write(bytes, 0, bytes.Length);
            //}

            //MainViewModel.Instance.Reports.YearReports.Add(Path.GetFileName(filename));
        }
    }
}

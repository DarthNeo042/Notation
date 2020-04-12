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
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Notation.Utils
{
    public static class SSRSUtils
    {
        static public void CreatePeriodReport(PeriodViewModel period)
        {
            string directory = FileUtils.SelectDirectory();

            if (!string.IsNullOrEmpty(directory))
            {
                ProgressView progress = new ProgressView();
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

            PeriodCommentViewModel periodComment = PeriodCommentModel.Read(student.Year, period.Id, student.Id);
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

            foreach (SubjectViewModel subject in _class.Level.Subjects.Where(s => s.ParentSubject == null))
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

                foreach (SubjectViewModel subject2 in subject.ChildrenSubjects)
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
                }
            }

            //Cell mainCell = new Cell();
            //mainCell.Add(table);
            //mainCell.SetMargin(2);

            //Table mainTable = new Table(1);
            //mainTable.UseAllAvailableWidth();
            //mainTable.SetBorder(new SolidBorder(0.25f));
            //mainTable.SetMarginBottom(15);
            //mainTable.AddCell(mainCell);

            //document.Add(mainTable);

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

    }
}

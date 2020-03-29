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

            ReportDataSource dataSource = new ReportDataSource("HeaderDataSet", new List<BulletinPeriodeHeaderDataSource>() { new BulletinPeriodeHeaderDataSource()
            {
                BirthDate = string.Format("né le {0}", student.BirthDate.ToShortDateString()),
                FirstName = student.FirstName,
                LastName = student.LastName,
                Header1 = string.Format("Année {0} - {1}", student.Year, student.Year + 1),
                Header2 = string.Format("Période du {0} au {1}", period.FromDate.ToShortDateString(), period.ToDate.ToShortDateString()),
                Header3 = string.Format("Classe de {0}\tEffectif {1}", _class.Name, _class.Students.Count),
            }
            });

            ReportViewer report = new ReportViewer();
            report.LocalReport.ReportPath = @".\Reports\BulletinPeriode.rdlc";
            report.LocalReport.DataSources.Add(dataSource);

            byte[] bytes = report.LocalReport.Render("PDF", null, out string mimeType, out string encoding, out string extension, out string[] streamIds, out Warning[] warnings);

            using (FileStream fileStream = new FileStream(filename, FileMode.Create))
            {
                fileStream.Write(bytes, 0, bytes.Length);
            }
        }

        //static private Cell NewStandardCell()
        //{
        //    Cell cell = new Cell();
        //    cell.SetHeight(18);
        //    cell.SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE);
        //    return cell;
        //}

        //static private Cell NewStandardCell2()
        //{
        //    Cell cell = new Cell();
        //    cell.SetHeight(15);
        //    cell.SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE);
        //    return cell;
        //}

        //static private void GeneratePeriodAverage(Document document, StudentViewModel student, PeriodViewModel period)
        //{
        //    Table table = new Table(UnitValue.CreatePercentArray(new float[] { 0.85f, 0.15f }));
        //    table.UseAllAvailableWidth();

        //    PdfFont cambriaBoldItalic = PdfFontFactory.CreateFont(FontProgramFactory.CreateFont(@"Resources\cambriaz.ttf"));
        //    PdfFont cambriaBold = PdfFontFactory.CreateFont(FontProgramFactory.CreateFont(@"Resources\cambriab.ttf"));
        //    PdfFont cambria = PdfFontFactory.CreateFont(FontProgramFactory.CreateFont(@"Resources\cambria.ttc,0"));

        //    Cell averageHeaderCell = NewStandardCell2();
        //    Paragraph paragraph = new Paragraph();
        //    paragraph.SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT);
        //    Text text = new Text("Moyenne des notes cumulées depuis le début du trimestre");
        //    text.SetFont(cambriaBoldItalic);
        //    text.SetFontSize(9);
        //    paragraph.Add(text);
        //    averageHeaderCell.Add(paragraph);
        //    table.AddCell(averageHeaderCell);

        //    double currentAverage = MarkModel.ReadPeriodTrimesterAverage(period, student);
        //    Cell averageHeader = NewStandardCell2();
        //    paragraph = new Paragraph();
        //    paragraph.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
        //    text = new Text(currentAverage != -1 ? currentAverage.ToString("0.0") : "");
        //    text.SetFont(cambriaBold);
        //    text.SetFontSize(11);
        //    paragraph.Add(text);
        //    averageHeader.Add(paragraph);
        //    table.AddCell(averageHeader);

        //    Cell lastAverageHeaderCell = NewStandardCell2();
        //    paragraph = new Paragraph();
        //    paragraph.SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT);
        //    text = new Text("Rappel de la dernière moyenne obtenue");
        //    text.SetFont(cambriaBoldItalic);
        //    text.SetFontSize(9);
        //    paragraph.Add(text);
        //    lastAverageHeaderCell.Add(paragraph);
        //    table.AddCell(lastAverageHeaderCell);

        //    PeriodViewModel previousPeriod = ModelUtils.GetPreviousPeriod(period);
        //    double lastAverage = previousPeriod != null ? MarkModel.ReadPeriodTrimesterAverage(previousPeriod, student) : 0;
        //    Cell lastAverageHeader = NewStandardCell2();
        //    paragraph = new Paragraph();
        //    paragraph.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
        //    text = new Text(previousPeriod != null ? lastAverage.ToString() : "");
        //    text.SetFont(cambriaBold);
        //    text.SetFontSize(11);
        //    paragraph.Add(text);
        //    lastAverageHeader.Add(paragraph);
        //    table.AddCell(lastAverageHeader);

        //    Cell tendancyHeaderCell = NewStandardCell2();
        //    paragraph = new Paragraph();
        //    paragraph.SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT);
        //    text = new Text("Tendance");
        //    text.SetFont(cambriaBoldItalic);
        //    text.SetFontSize(9);
        //    paragraph.Add(text);
        //    tendancyHeaderCell.Add(paragraph);
        //    table.AddCell(tendancyHeaderCell);

        //    string tendancy = "";
        //    if (previousPeriod != null)
        //    {
        //        tendancy = currentAverage > lastAverage ? "↗" : currentAverage < lastAverage ? "↘" : "→";
        //    }
        //    Cell tendancyCell = NewStandardCell2();
        //    paragraph = new Paragraph();
        //    paragraph.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
        //    text = new Text(tendancy);
        //    text.SetFont(cambriaBold);
        //    text.SetFontSize(11);
        //    paragraph.Add(text);
        //    tendancyCell.Add(paragraph);
        //    table.AddCell(tendancyCell);

        //    Cell mainCell = new Cell();
        //    mainCell.Add(table);
        //    mainCell.SetMargin(2);

        //    Table mainTable = new Table(1);
        //    mainTable.SetMarginBottom(15);
        //    mainTable.UseAllAvailableWidth();
        //    //mainTable.SetWidth(UnitValue.CreatePercentValue(0.8f));
        //    mainTable.SetBorder(new SolidBorder(0.25f));
        //    mainTable.SetMarginBottom(15);
        //    mainTable.AddCell(mainCell);

        //    document.Add(mainTable);
        //}

        //private static void GenerateReportHeader(Document document, StudentViewModel student, ClassViewModel _class, object time)
        //{
        //    Table table = new Table(UnitValue.CreatePercentArray(new float[] { 0.275f, 0.45f, 0.275f }));
        //    table.UseAllAvailableWidth();
        //    table.SetMarginBottom(5);

        //    Cell iconCell = new Cell();
        //    iconCell.SetBorder(Border.NO_BORDER);
        //    Image icon = new Image(ImageDataFactory.Create(@"Resources\Icon.bmp"));
        //    icon.SetWidth(101);
        //    icon.SetHeight(95);
        //    iconCell.Add(icon);
        //    table.AddCell(iconCell);

        //    PdfFont cambria = PdfFontFactory.CreateFont(FontProgramFactory.CreateFont(@"Resources\cambria.ttc,0"));

        //    Cell titleCell = new Cell();
        //    titleCell.SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.TOP);
        //    titleCell.SetBorder(Border.NO_BORDER);
        //    Paragraph paragraph = null;
        //    Text text = null;
        //    switch (time.GetType().ToString())
        //    {
        //        case "Notation.ViewModels.PeriodViewModel":
        //            {
        //                PeriodViewModel period = (PeriodViewModel)time;
        //                paragraph = new Paragraph();
        //                paragraph.SetMultipliedLeading(1.25f);
        //                paragraph.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
        //                text = new Text("BULLETIN DE PÉRIODE\r\n");
        //                text.SetFont(cambria);
        //                text.SetFontSize(20);
        //                paragraph.Add(text);
        //                paragraph.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
        //                text = new Text(string.Format("Année {0} - {1}\r\n", student.Year, student.Year + 1));
        //                text.SetFont(cambria);
        //                text.SetFontSize(14);
        //                paragraph.Add(text);
        //                paragraph.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
        //                text = new Text(string.Format("Période du {0} au {1}", period.FromDate.ToShortDateString(), period.ToDate.ToShortDateString()));
        //                text.SetFont(cambria);
        //                text.SetFontSize(14);
        //                paragraph.Add(text);
        //                titleCell.Add(paragraph);
        //            }
        //            break;

        //        case "Notation.ViewModels.SemiTrimesterViewModel":
        //            {
        //                SemiTrimesterViewModel semiTrimester = (SemiTrimesterViewModel)time;
        //                paragraph = new Paragraph();
        //                paragraph.SetMultipliedLeading(1.25f);
        //                paragraph.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
        //                text = new Text("BULLETIN\r\n");
        //                text.SetFont(cambria);
        //                text.SetFontSize(20);
        //                paragraph.Add(text);
        //                paragraph.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
        //                text = new Text("DEMI-TRIMESTRIEL\r\n");
        //                text.SetFont(cambria);
        //                text.SetFontSize(20);
        //                paragraph.Add(text);
        //                paragraph.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
        //                text = new Text(string.Format("DE {0}\r\n", semiTrimester.Month.ToUpper()));
        //                text.SetFont(cambria);
        //                text.SetFontSize(20);
        //                paragraph.Add(text);
        //                paragraph.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
        //                text = new Text(string.Format("Année {0} - {1}", student.Year, student.Year + 1));
        //                text.SetFont(cambria);
        //                text.SetFontSize(14);
        //                paragraph.Add(text);
        //                titleCell.Add(paragraph);
        //            }
        //            break;

        //        case "System.Int32":
        //            {
        //                int value = (int)time;
        //                if (value <= 4)
        //                {
        //                    int trimester = (int)time;
        //                    paragraph = new Paragraph();
        //                    paragraph.SetMultipliedLeading(1.25f);
        //                    paragraph.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
        //                    text = new Text("BULLETIN\r\n");
        //                    text.SetFont(cambria);
        //                    text.SetFontSize(20);
        //                    paragraph.Add(text);
        //                    paragraph.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
        //                    text = new Text(string.Format("DU {0} TRIMESTRE\r\n", NumberUtils.GetRankString(trimester)));
        //                    text.SetFont(cambria);
        //                    text.SetFontSize(20);
        //                    paragraph.Add(text);
        //                    paragraph.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
        //                    text = new Text(string.Format("Année {0} - {1}", student.Year, student.Year + 1));
        //                    text.SetFont(cambria);
        //                    text.SetFontSize(14);
        //                    paragraph.Add(text);
        //                    titleCell.Add(paragraph);
        //                }
        //                else
        //                {
        //                    int year = (int)time;
        //                    paragraph = new Paragraph();
        //                    paragraph.SetMultipliedLeading(1.25f);
        //                    paragraph.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
        //                    text = new Text("BULLETIN ANNUEL\r\n");
        //                    text.SetFont(cambria);
        //                    text.SetFontSize(20);
        //                    paragraph.Add(text);
        //                    paragraph.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
        //                    text = new Text(string.Format("Année {0} - {1}", student.Year, student.Year + 1));
        //                    text.SetFont(cambria);
        //                    text.SetFontSize(14);
        //                    paragraph.Add(text);
        //                    titleCell.Add(paragraph);
        //                }
        //            }
        //            break;
        //    }
        //    paragraph = new Paragraph();
        //    paragraph.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
        //    text = new Text(string.Format("Classe de {0}\tEffectif {1}", _class.Name, _class.Students.Count));
        //    text.SetFont(cambria);
        //    text.SetFontSize(10);
        //    paragraph.Add(text);
        //    titleCell.Add(paragraph);
        //    table.AddCell(titleCell);

        //    Cell studentCell = new Cell();
        //    studentCell.SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.TOP);
        //    studentCell.SetBorder(Border.NO_BORDER);

        //    paragraph = new Paragraph();
        //    paragraph.SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT);
        //    text = new Text(student.LastName);
        //    text.SetFont(cambria);
        //    text.SetFontSize(16);
        //    paragraph.Add(text);
        //    studentCell.Add(paragraph);
        //    paragraph = new Paragraph();
        //    paragraph.SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT);
        //    text = new Text(student.FirstName);
        //    text.SetFont(cambria);
        //    text.SetFontSize(16);
        //    paragraph.Add(text);
        //    studentCell.Add(paragraph);
        //    paragraph = new Paragraph();
        //    paragraph.SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT);
        //    text = new Text(string.Format("né le {0}", student.BirthDate.ToShortDateString()));
        //    text.SetFont(cambria);
        //    text.SetFontSize(10);
        //    paragraph.Add(text);
        //    studentCell.Add(paragraph);
        //    table.AddCell(studentCell);

        //    document.Add(table);
        //}

        //static private Cell NewHeader2Cell()
        //{
        //    Cell cell = new Cell();
        //    cell.SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE);
        //    cell.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
        //    cell.SetHeight(12);
        //    cell.SetBorder(new SolidBorder(0.25f));
        //    return cell;
        //}

        //static private Cell CreateSubjectCell(SubjectViewModel subject, TeacherViewModel teacher)
        //{
        //    PdfFont cambriaBold = PdfFontFactory.CreateFont(FontProgramFactory.CreateFont(@"Resources\cambriab.ttf"));
        //    PdfFont cambriaItalic = PdfFontFactory.CreateFont(FontProgramFactory.CreateFont(@"Resources\cambriai.ttf"));

        //    Cell cell = new Cell();
        //    cell.SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE);
        //    cell.SetBorder(new SolidBorder(0.25f));
        //    cell.SetHeight(26);

        //    Paragraph paragraph = new Paragraph();
        //    paragraph.SetMultipliedLeading(0.9f);
        //    Text text = new Text(subject.Name.ToUpper() + "\r\n");
        //    text.SetFont(cambriaBold);
        //    text.SetFontSize(8);
        //    paragraph.Add(text);
        //    text = new Text(string.Format("({0}coeff. {1})", subject.Option ? "option " : "", subject.Coefficient));
        //    text.SetFont(cambriaItalic);
        //    text.SetFontSize(7);
        //    paragraph.Add(text);
        //    cell.Add(paragraph);

        //    if (teacher != null)
        //    {
        //        paragraph = new Paragraph();
        //        text = new Text(string.Format("{0} {1}{2}", teacher.Title, !string.IsNullOrEmpty(teacher.FirstName) ? teacher.FirstName.Substring(0, 1) + ". " : "", teacher.LastName));
        //        text.SetFont(cambriaItalic);
        //        text.SetFontSize(7);
        //        paragraph.Add(text);
        //        cell.Add(paragraph);
        //    }

        //    return cell;
        //}

        //static private Cell CreateChildSubjectCell(SubjectViewModel subject)
        //{
        //    PdfFont cambriaItalic = PdfFontFactory.CreateFont(FontProgramFactory.CreateFont(@"Resources\cambriai.ttf"));

        //    Cell cell = new Cell();
        //    cell.SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE);
        //    cell.SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT);
        //    cell.SetBorder(new SolidBorder(0.25f));
        //    cell.SetHeight(18);

        //    Paragraph paragraph = new Paragraph();
        //    paragraph.SetMultipliedLeading(0.85f);
        //    Text text = new Text(subject.Name + "\r\n");
        //    text.SetFont(cambriaItalic);
        //    text.SetFontSize(9);
        //    paragraph.Add(text);
        //    text = new Text(string.Format("({0}coeff. {1})", subject.Option ? "option " : "", subject.Coefficient));
        //    text.SetFont(cambriaItalic);
        //    text.SetFontSize(7);
        //    paragraph.Add(text);
        //    cell.Add(paragraph);

        //    return cell;
        //}

        //static private void GeneratePeriodMarks(Document document, IEnumerable<MarkViewModel> marks, StudentViewModel student, PeriodViewModel period, ClassViewModel _class)
        //{
        //    PdfFont cambriaBold = PdfFontFactory.CreateFont(FontProgramFactory.CreateFont(@"Resources\cambriab.ttf"));
        //    PdfFont calibri = PdfFontFactory.CreateFont(FontProgramFactory.CreateFont(@"Resources\calibri.ttf"));

        //    IEnumerable<int> coefficients = marks.Select(m => m.Coefficient).Distinct().OrderBy(c => c);
        //    Dictionary<int, string> coefficientLibelles = new Dictionary<int, string>() { { 1, "Leçons" }, { 2, "Devoirs" }, { 4, "Examens" } };

        //    float[] tabColums = new float[coefficients.Count() + 1];
        //    tabColums[0] = 0.15f;
        //    int i = 0;
        //    foreach (int coefficient in coefficients.OrderBy(c => c))
        //    {
        //        tabColums[++i] = (coefficient == 4 ? 0.425f : 0.85f) / (coefficients.Count() + (coefficients.Contains(4) ? -0.5f : 0));
        //    }

        //    Table table = new Table(UnitValue.CreatePercentArray(tabColums));
        //    table.UseAllAvailableWidth();

        //    Cell header = NewHeader2Cell();
        //    header.SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT);
        //    Paragraph paragraph = new Paragraph();
        //    Text text = new Text("Disciplines");
        //    text.SetFont(cambriaBold);
        //    text.SetFontSize(8);
        //    paragraph.Add(text);
        //    header.Add(paragraph);
        //    table.AddCell(header);

        //    foreach (int coefficient in coefficients)
        //    {
        //        header = NewHeader2Cell();
        //        paragraph = new Paragraph();
        //        text = new Text(string.Format("{0}oefficient {1}",
        //            coefficientLibelles.ContainsKey(coefficient) ? coefficientLibelles[coefficient] + " - c" : "C", coefficient));
        //        text.SetFont(cambriaBold);
        //        text.SetFontSize(8);
        //        paragraph.Add(text);
        //        header.Add(paragraph);
        //        table.AddCell(header);
        //    }

        //    foreach (SubjectViewModel subject in _class.Level.Subjects.Where(s => s.ParentSubject == null))
        //    {
        //        table.AddCell(CreateSubjectCell(subject, ModelUtils.GetTeacherFromClassAndSubject(student.Class, subject)));

        //        foreach (int coefficient in coefficients)
        //        {
        //            string marksStr = "";
        //            foreach (MarkViewModel mark in marks.Where(m => m.IdSubject == subject.Id && m.Coefficient == coefficient))
        //            {
        //                if (!string.IsNullOrEmpty(marksStr))
        //                {
        //                    marksStr += "\t";
        //                }
        //                marksStr += mark.Mark.ToString();
        //            }
        //            Cell marksCell = new Cell();
        //            marksCell.SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE);
        //            marksCell.SetBorder(new SolidBorder(0.25f));
        //            marksCell.SetHeight(26);
        //            paragraph = new Paragraph();
        //            text = new Text(marksStr);
        //            text.SetFont(calibri);
        //            text.SetFontSize(11);
        //            paragraph.Add(text);
        //            marksCell.Add(paragraph);
        //            table.AddCell(marksCell);
        //        }

        //        foreach (SubjectViewModel subject2 in subject.ChildrenSubjects)
        //        {
        //            table.AddCell(CreateChildSubjectCell(subject2));

        //            foreach (int coefficient in coefficients)
        //            {
        //                string marksStr = "";
        //                foreach (MarkViewModel mark in marks.Where(m => m.IdSubject == subject2.Id && m.Coefficient == coefficient))
        //                {
        //                    if (!string.IsNullOrEmpty(marksStr))
        //                    {
        //                        marksStr += "\t";
        //                    }
        //                    marksStr += mark.Mark.ToString();
        //                }
        //                Cell marksCell = new Cell();
        //                marksCell.SetBorder(new SolidBorder(0.25f));
        //                marksCell.SetHeight(18);
        //                paragraph = new Paragraph();
        //                paragraph.SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE);
        //                text = new Text(marksStr);
        //                text.SetFont(calibri);
        //                text.SetFontSize(11);
        //                paragraph.Add(text);
        //                marksCell.Add(paragraph);
        //                table.AddCell(marksCell);
        //            }
        //        }
        //    }

        //    Cell mainCell = new Cell();
        //    mainCell.Add(table);
        //    mainCell.SetMargin(2);

        //    Table mainTable = new Table(1);
        //    mainTable.UseAllAvailableWidth();
        //    mainTable.SetBorder(new SolidBorder(0.25f));
        //    mainTable.SetMarginBottom(15);
        //    mainTable.AddCell(mainCell);

        //    document.Add(mainTable);
        //}
    }
}

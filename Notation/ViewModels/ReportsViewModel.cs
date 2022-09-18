using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Notation.ViewModels
{
    public class ReportsViewModel : DependencyObject
    {
        public ObservableCollection<PeriodViewModel> Periods { get; set; }
        public ObservableCollection<SemiTrimesterViewModel> SemiTrimesters { get; set; }
        public ObservableCollection<int> Trimesters { get; set; }

        public int Year
        {
            get { return (int)GetValue(YearProperty); }
            set { SetValue(YearProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Year.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty YearProperty =
            DependencyProperty.Register("Year", typeof(int), typeof(ReportsViewModel), new PropertyMetadata(0));

        public ObservableCollection<string> PeriodReports { get; set; }

        public string SelectedPeriodReport
        {
            get { return (string)GetValue(SelectedPeriodReportProperty); }
            set { SetValue(SelectedPeriodReportProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedPeriodReport.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedPeriodReportProperty =
            DependencyProperty.Register("SelectedPeriodReport", typeof(string), typeof(ReportsViewModel), new PropertyMetadata(""));

        public string PeriodReportsPath
        {
            get { return (string)GetValue(PeriodReportsPathProperty); }
            set { SetValue(PeriodReportsPathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PeriodReportsPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PeriodReportsPathProperty =
            DependencyProperty.Register("PeriodReportsPath", typeof(string), typeof(ReportsViewModel), new PropertyMetadata(""));

        public ICommand OpenPeriodReportsPathCommand { get; set; }

        private void OpenPeriodReportsPathCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !PeriodReports.Any();
        }

        private void OpenPeriodReportsPathExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Process.Start("explorer", $"/root,{PeriodReportsPath}");
        }

        public ICommand PrintPDFPeriodReportsCommand { get; set; }

        private void PrintPDFPeriodReportsCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !string.IsNullOrEmpty(PeriodReportsPath);
        }

        private void PrintPDFPeriodReportsExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            List<string> filenames = PeriodReports.Where(r => r.EndsWith(".html")).ToList();
            PeriodReports.Clear();
            foreach (string filename in filenames.Select(f => Path.Combine(MainViewModel.Instance.Reports.PeriodReportsPath, f)))
            {
                string PDFFilename = $"{Path.GetFileNameWithoutExtension(filename)}.pdf";

                PdfSharpCore.Pdf.PdfDocument pdf = TheArtOfDev.HtmlRenderer.PdfSharp.PdfGenerator.GeneratePdf(File.ReadAllText(filename), PdfSharpCore.PageSize.A4, 0);
                pdf.Save(Path.Combine(MainViewModel.Instance.Reports.PeriodReportsPath, PDFFilename));

                PeriodReports.Add(PDFFilename);
            }
        }

        public ObservableCollection<string> SemiTrimesterReports { get; set; }

        public string SelectedSemiTrimesterReport
        {
            get { return (string)GetValue(SelectedSemiTrimesterReportProperty); }
            set { SetValue(SelectedSemiTrimesterReportProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedSemiTrimesterReport.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedSemiTrimesterReportProperty =
            DependencyProperty.Register("SelectedSemiTrimesterReport", typeof(string), typeof(ReportsViewModel), new PropertyMetadata(""));

        public string SemiTrimesterReportsPath
        {
            get { return (string)GetValue(SemiTrimesterReportsPathProperty); }
            set { SetValue(SemiTrimesterReportsPathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SemiTrimesterReportsPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SemiTrimesterReportsPathProperty =
            DependencyProperty.Register("SemiTrimesterReportsPath", typeof(string), typeof(ReportsViewModel), new PropertyMetadata(""));

        public ICommand OpenSemiTrimesterReportsPathCommand { get; set; }

        private void OpenSemiTrimesterReportsPathCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !string.IsNullOrEmpty(SemiTrimesterReportsPath);
        }

        private void OpenSemiTrimesterReportsPathExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Process.Start("explorer", $"/root,{SemiTrimesterReportsPath}");
        }

        public ObservableCollection<string> TrimesterReports { get; set; }

        public string SelectedTrimesterReport
        {
            get { return (string)GetValue(SelectedTrimesterReportProperty); }
            set { SetValue(SelectedTrimesterReportProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedTrimesterReport.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedTrimesterReportProperty =
            DependencyProperty.Register("SelectedTrimesterReport", typeof(string), typeof(ReportsViewModel), new PropertyMetadata(""));

        public string TrimesterReportsPath
        {
            get { return (string)GetValue(TrimesterReportsPathProperty); }
            set { SetValue(TrimesterReportsPathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TrimesterReportsPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TrimesterReportsPathProperty =
            DependencyProperty.Register("TrimesterReportsPath", typeof(string), typeof(ReportsViewModel), new PropertyMetadata(""));

        public ICommand OpenTrimesterReportsPathCommand { get; set; }

        private void OpenTrimesterReportsPathCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !string.IsNullOrEmpty(TrimesterReportsPath);
        }

        private void OpenTrimesterReportsPathExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Process.Start("explorer", $"/root,{TrimesterReportsPath}");
        }

        public ObservableCollection<string> YearReports { get; set; }

        public string SelectedYearReport
        {
            get { return (string)GetValue(SelectedYearReportProperty); }
            set { SetValue(SelectedYearReportProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedYearReport.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedYearReportProperty =
            DependencyProperty.Register("SelectedYearReport", typeof(string), typeof(ReportsViewModel), new PropertyMetadata(""));

        public string YearReportsPath
        {
            get { return (string)GetValue(YearReportsPathProperty); }
            set { SetValue(YearReportsPathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for YearReportsPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty YearReportsPathProperty =
            DependencyProperty.Register("YearReportsPath", typeof(string), typeof(ReportsViewModel), new PropertyMetadata(""));

        public ICommand OpenYearReportsPathCommand { get; set; }

        private void OpenYearReportsPathCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !string.IsNullOrEmpty(YearReportsPath);
        }

        private void OpenYearReportsPathExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Process.Start("explorer", $"/root,{YearReportsPath}");
        }

        public CommandBindingCollection Bindings { get; set; }

        public ReportsViewModel()
        {
            Periods = new ObservableCollection<PeriodViewModel>();
            SemiTrimesters = new ObservableCollection<SemiTrimesterViewModel>();
            Trimesters = new ObservableCollection<int>();

            PeriodReports = new ObservableCollection<string>();
            SemiTrimesterReports = new ObservableCollection<string>();
            TrimesterReports = new ObservableCollection<string>();
            YearReports = new ObservableCollection<string>();

            OpenPeriodReportsPathCommand = new RoutedUICommand("OpenPeriodReportsPath", "OpenPeriodReportsPath", typeof(ReportsViewModel));
            PrintPDFPeriodReportsCommand = new RoutedUICommand("PrintPDFPeriodReports", "PrintPDFPeriodReports", typeof(ReportsViewModel));
            OpenSemiTrimesterReportsPathCommand = new RoutedUICommand("OpenSemiTrimesterReportsPath", "OpenSemiTrimesterReportsPath", typeof(ReportsViewModel));
            OpenTrimesterReportsPathCommand = new RoutedUICommand("OpenTrimesterReportsPath", "OpenTrimesterReportsPath", typeof(ReportsViewModel));
            OpenYearReportsPathCommand = new RoutedUICommand("OpenYearReportsPath", "OpenYearReportsPath", typeof(ReportsViewModel));
            Bindings = new CommandBindingCollection()
            {
                new CommandBinding(OpenPeriodReportsPathCommand, OpenPeriodReportsPathExecuted, OpenPeriodReportsPathCanExecute),
                new CommandBinding(PrintPDFPeriodReportsCommand, PrintPDFPeriodReportsExecuted, PrintPDFPeriodReportsCanExecute),
                new CommandBinding(OpenSemiTrimesterReportsPathCommand, OpenSemiTrimesterReportsPathExecuted, OpenSemiTrimesterReportsPathCanExecute),
                new CommandBinding(OpenTrimesterReportsPathCommand, OpenTrimesterReportsPathExecuted, OpenTrimesterReportsPathCanExecute),
                new CommandBinding(OpenYearReportsPathCommand, OpenYearReportsPathExecuted, OpenYearReportsPathCanExecute),
            };
        }

        public void LoadData()
        {
            Periods.Clear();
            SemiTrimesters.Clear();
            Trimesters.Clear();

            foreach (PeriodViewModel period in MainViewModel.Instance.Parameters.Periods.OrderBy(p => p.Trimester).ThenBy(p => p.Number))
            {
                Periods.Add(period);
            }
            foreach (SemiTrimesterViewModel semiTrimester in MainViewModel.Instance.Parameters.SemiTrimesters)
            {
                SemiTrimesters.Add(semiTrimester);
            }
            foreach (int trimester in Periods.Select(p => p.Trimester).Distinct())
            {
                Trimesters.Add(trimester);
            }

            Year = MainViewModel.Instance.Parameters.Year;
        }
    }
}

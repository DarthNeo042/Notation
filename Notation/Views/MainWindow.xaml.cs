using Notation.Utils;
using Notation.ViewModels;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Notation.Views
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public delegate void UpdatePeriodModelsDelegate(int value);
        private UpdatePeriodModelsDelegate _updatePeriodModels;
        private UpdatePeriodModelsDelegate _updatePeriodModelsDispatch;

        private void UpdatePeriodModels(int value)
        {
            PeriodModelsProgressBar.Value = value;
        }

        private void UpdatePeriodModelsDispatch(int value)
        {
            Dispatcher.Invoke(_updatePeriodModels, System.Windows.Threading.DispatcherPriority.Background, value);
        }

        public delegate void UpdateSemiTrimesterModelsDelegate(int value);
        private UpdateSemiTrimesterModelsDelegate _updateSemiTrimesterModels;
        private UpdateSemiTrimesterModelsDelegate _updateSemiTrimesterModelsDispatch;

        private void UpdateSemiTrimesterModels(int value)
        {
            SemiTrimesterModelsProgressBar.Value = value;
        }

        private void UpdateSemiTrimesterModelsDispatch(int value)
        {
            Dispatcher.Invoke(_updateSemiTrimesterModels, System.Windows.Threading.DispatcherPriority.Background, value);
        }

        public delegate void UpdateTrimesterModelsDelegate(int value);
        private UpdateTrimesterModelsDelegate _updateTrimesterModels;
        private UpdateTrimesterModelsDelegate _updateTrimesterModelsDispatch;

        private void UpdateTrimesterModels(int value)
        {
            TrimesterModelsProgressBar.Value = value;
        }

        private void UpdateTrimesterModelsDispatch(int value)
        {
            Dispatcher.Invoke(_updateTrimesterModels, System.Windows.Threading.DispatcherPriority.Background, value);
        }

        public delegate void UpdateImportDelegate(int value);
        private UpdateImportDelegate _updateImport;

        private void UpdateImport(int value)
        {
            ImportProgressBar.Value = value;
        }

        public delegate void UpdatePeriodReportsDelegate(int value);
        private UpdatePeriodReportsDelegate _updatePeriodReports;
        private UpdatePeriodReportsDelegate _updatePeriodReportsDispatch;

        private void UpdatePeriodReports(int value)
        {
            PeriodReportsProgressBar.Value = value;
        }

        private void UpdatePeriodReportsDispatch(int value)
        {
            Dispatcher.Invoke(_updatePeriodReports, System.Windows.Threading.DispatcherPriority.Background, value);
        }

        public delegate void UpdateSemiTrimesterReportsDelegate(int value);
        private UpdateSemiTrimesterReportsDelegate _updateSemiTrimesterReports;
        private UpdateSemiTrimesterReportsDelegate _updateSemiTrimesterReportsDispatch;

        private void UpdateSemiTrimesterReports(int value)
        {
            SemiTrimesterReportsProgressBar.Value = value;
        }

        private void UpdateSemiTrimesterReportsDispatch(int value)
        {
            Dispatcher.Invoke(_updateSemiTrimesterReports, System.Windows.Threading.DispatcherPriority.Background, value);
        }

        public delegate void UpdateTrimesterReportsDelegate(int value);
        private UpdateTrimesterReportsDelegate _updateTrimesterReports;
        private UpdateTrimesterReportsDelegate _updateTrimesterReportsDispatch;

        private void UpdateTrimesterReports(int value)
        {
            TrimesterReportsProgressBar.Value = value;
        }

        private void UpdateTrimesterReportsDispatch(int value)
        {
            Dispatcher.Invoke(_updateTrimesterReports, System.Windows.Threading.DispatcherPriority.Background, value);
        }

        public delegate void UpdateYearReportsDelegate(int value);
        private UpdateYearReportsDelegate _updateYearReports;
        private UpdateYearReportsDelegate _updateYearReportsDispatch;

        private void UpdateYearReports(int value)
        {
            YearReportsProgressBar.Value = value;
        }

        private void UpdateYearReportsDispatch(int value)
        {
            Dispatcher.Invoke(_updateYearReports, System.Windows.Threading.DispatcherPriority.Background, value);
        }

        public MainWindow()
        {
            DataContext = MainViewModel.Instance;
            CommandBindings.AddRange(MainViewModel.Instance.Bindings);
            CommandBindings.AddRange(MainViewModel.Instance.Parameters.Bindings);
            CommandBindings.AddRange(MainViewModel.Instance.Models.Bindings);
            CommandBindings.AddRange(MainViewModel.Instance.Reports.Bindings);
            CommandBindings.AddRange(MainViewModel.Instance.Entry.Bindings);

            _updatePeriodModels = new UpdatePeriodModelsDelegate(UpdatePeriodModels);
            _updatePeriodModelsDispatch = new UpdatePeriodModelsDelegate(UpdatePeriodModelsDispatch);
            _updateSemiTrimesterModels = new UpdateSemiTrimesterModelsDelegate(UpdateSemiTrimesterModels);
            _updateSemiTrimesterModelsDispatch = new UpdateSemiTrimesterModelsDelegate(UpdateSemiTrimesterModelsDispatch);
            _updateTrimesterModels = new UpdateTrimesterModelsDelegate(UpdateTrimesterModels);
            _updateTrimesterModelsDispatch = new UpdateTrimesterModelsDelegate(UpdateTrimesterModelsDispatch);
            _updateImport = new UpdateImportDelegate(UpdateImport);

            _updatePeriodReports = new UpdatePeriodReportsDelegate(UpdatePeriodReports);
            _updatePeriodReportsDispatch = new UpdatePeriodReportsDelegate(UpdatePeriodReportsDispatch);
            _updateSemiTrimesterReports = new UpdateSemiTrimesterReportsDelegate(UpdateSemiTrimesterReports);
            _updateSemiTrimesterReportsDispatch = new UpdateSemiTrimesterReportsDelegate(UpdateSemiTrimesterReportsDispatch);
            _updateTrimesterReports = new UpdateTrimesterReportsDelegate(UpdateTrimesterReports);
            _updateTrimesterReportsDispatch = new UpdateTrimesterReportsDelegate(UpdateTrimesterReportsDispatch);
            _updateYearReports = new UpdateYearReportsDelegate(UpdateYearReports);
            _updateYearReportsDispatch = new UpdateYearReportsDelegate(UpdateYearReportsDispatch);

            InitializeComponent();

            for (int i = 1; i <= 12; i++)
            {
                ComboPeriodNumber.Items.Add(i);
            }
            for (int i = 1; i <= 4; i++)
            {
                ComboPeriodTrimester.Items.Add(i);
            }
            ComboTeacherTitle.ItemsSource = new List<string>() { "M.", "Mme", "Mlle", "Ab.", "Fr." };

            MainViewModel.Instance.Parameters.BaseParametersChangedEvent += Parameters_BaseParametersChangedEvent;
            Parameters_BaseParametersChangedEvent();
        }

        private void Parameters_BaseParametersChangedEvent()
        {
            AdminPassword.Password = MainViewModel.Instance.Parameters.BaseParameters?.AdminPassword;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MainViewModel.Instance.LoginCommand.Execute(null, this);
        }

        private void Period_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ModifyPeriod.Command.Execute(null);
        }

        private void Level_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ModifyLevel.Command.Execute(null);
        }

        private void Subject_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ModifySubject.Command.Execute(null);
        }

        private void Teacher_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ModifyTeacher.Command.Execute(null);
        }

        private void Class_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ModifyClass.Command.Execute(null);
        }

        private void Student_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ModifyStudent.Command.Execute(null);
        }

        private void SemiTrimester_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ModifySemiTrimester.Command.Execute(null);
        }

        private void ExportPeriod_Click(object sender, RoutedEventArgs e)
        {
            if (((Control)sender).DataContext is PeriodViewModel period)
            {
                PeriodModelsProgressBar.Visibility = Visibility.Visible;
                PeriodModelsProgressBar.Value = 0;
                ExportUtils.ExportPeriodModels(period, _updatePeriodModelsDispatch);
                PeriodModelsProgressBar.Visibility = Visibility.Collapsed;
            }
        }

        private void ExportSemiTrimester_Click(object sender, RoutedEventArgs e)
        {
            if (((Control)sender).DataContext is SemiTrimesterViewModel semiTrimester)
            {
                SemiTrimesterModelsProgressBar.Visibility = Visibility.Visible;
                SemiTrimesterModelsProgressBar.Value = 0;
                ExportUtils.ExportSemiTrimesterModels(semiTrimester, _updateSemiTrimesterModelsDispatch);
                SemiTrimesterModelsProgressBar.Visibility = Visibility.Collapsed;
            }
        }

        private void ExportTrimester_Click(object sender, RoutedEventArgs e)
        {
            if (((Control)sender).DataContext is int trimester)
            {
                TrimesterModelsProgressBar.Visibility = Visibility.Visible;
                TrimesterModelsProgressBar.Value = 0;
                ExportUtils.ExportTrimesterModels(trimester, _updateTrimesterModelsDispatch);
                TrimesterModelsProgressBar.Visibility = Visibility.Collapsed;
            }
        }

        private void ImportModel_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                MainViewModel.Instance.Models.SuccessfulImport = false;

                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                ImportProgressBar.Visibility = Visibility.Visible;
                ImportProgressBar.Value = 0;
                int fileCount = 0;
                foreach (string file in files)
                {
                    ExportUtils.Import(file);
                    fileCount++;
                    Dispatcher.Invoke(_updateImport, System.Windows.Threading.DispatcherPriority.Background, fileCount * 1000 / files.Length);
                }
                ImportProgressBar.Visibility = Visibility.Collapsed;

                MainViewModel.Instance.Models.SuccessfulImport = true;
            }
        }

        private void PeriodModel_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!string.IsNullOrEmpty(MainViewModel.Instance.Models.PeriodModelsPath) && !string.IsNullOrEmpty(MainViewModel.Instance.Models.SelectedPeriodModel))
            {
                Process.Start(Path.Combine(MainViewModel.Instance.Models.PeriodModelsPath, MainViewModel.Instance.Models.SelectedPeriodModel));
            }
        }

        private void SemiTrimesterModel_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!string.IsNullOrEmpty(MainViewModel.Instance.Models.SemiTrimesterModelsPath) && !string.IsNullOrEmpty(MainViewModel.Instance.Models.SelectedSemiTrimesterModel))
            {
                Process.Start(Path.Combine(MainViewModel.Instance.Models.SemiTrimesterModelsPath, MainViewModel.Instance.Models.SelectedSemiTrimesterModel));
            }
        }

        private void TrimesterModel_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!string.IsNullOrEmpty(MainViewModel.Instance.Models.TrimesterModelsPath) && !string.IsNullOrEmpty(MainViewModel.Instance.Models.SelectedTrimesterModel))
            {
                Process.Start(Path.Combine(MainViewModel.Instance.Models.TrimesterModelsPath, MainViewModel.Instance.Models.SelectedTrimesterModel));
            }
        }

        private void ReportPeriod_Click(object sender, RoutedEventArgs e)
        {
            if (((Control)sender).DataContext is PeriodViewModel period)
            {
                PeriodReportsProgressBar.Visibility = Visibility.Visible;
                PeriodReportsProgressBar.Value = 0;
                SSRSUtils.CreatePeriodReport(period, _updatePeriodReportsDispatch);
                PeriodReportsProgressBar.Visibility = Visibility.Collapsed;
            }
        }

        private void ReportSemiTrimester_Click(object sender, RoutedEventArgs e)
        {
            if (((Control)sender).DataContext is SemiTrimesterViewModel semiTrimester)
            {
                SemiTrimesterReportsProgressBar.Visibility = Visibility.Visible;
                SemiTrimesterReportsProgressBar.Value = 0;
                SSRSUtils.CreateSemiTrimesterReport(semiTrimester, _updateSemiTrimesterReportsDispatch);
                SemiTrimesterReportsProgressBar.Visibility = Visibility.Collapsed;
            }
        }

        private void ReportTrimester_Click(object sender, RoutedEventArgs e)
        {
            if (((Control)sender).DataContext is int trimester)
            {
                TrimesterReportsProgressBar.Visibility = Visibility.Visible;
                TrimesterReportsProgressBar.Value = 0;
                SSRSUtils.CreateTrimesterReport(trimester, _updateTrimesterReportsDispatch);
                TrimesterReportsProgressBar.Visibility = Visibility.Collapsed;
            }
        }

        private void ReportYear_Click(object sender, RoutedEventArgs e)
        {
            YearReportsProgressBar.Visibility = Visibility.Visible;
            YearReportsProgressBar.Value = 0;
            SSRSUtils.CreateYearReport(MainViewModel.Instance.Reports.Year, _updateYearReportsDispatch);
            YearReportsProgressBar.Visibility = Visibility.Collapsed;
        }

        private void PeriodReport_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!string.IsNullOrEmpty(MainViewModel.Instance.Reports.PeriodReportsPath) && !string.IsNullOrEmpty(MainViewModel.Instance.Reports.SelectedPeriodReport))
            {
                Process.Start(Path.Combine(MainViewModel.Instance.Reports.PeriodReportsPath, MainViewModel.Instance.Reports.SelectedPeriodReport));
            }
        }

        private void SemiTrimesterReport_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!string.IsNullOrEmpty(MainViewModel.Instance.Reports.SemiTrimesterReportsPath) && !string.IsNullOrEmpty(MainViewModel.Instance.Reports.SelectedSemiTrimesterReport))
            {
                Process.Start(Path.Combine(MainViewModel.Instance.Reports.SemiTrimesterReportsPath, MainViewModel.Instance.Reports.SelectedSemiTrimesterReport));
            }
        }

        private void TrimesterReport_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!string.IsNullOrEmpty(MainViewModel.Instance.Reports.TrimesterReportsPath) && !string.IsNullOrEmpty(MainViewModel.Instance.Reports.SelectedTrimesterReport))
            {
                Process.Start(Path.Combine(MainViewModel.Instance.Reports.TrimesterReportsPath, MainViewModel.Instance.Reports.SelectedTrimesterReport));
            }
        }

        private void YearReport_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!string.IsNullOrEmpty(MainViewModel.Instance.Reports.YearReportsPath) && !string.IsNullOrEmpty(MainViewModel.Instance.Reports.SelectedYearReport))
            {
                Process.Start(Path.Combine(MainViewModel.Instance.Reports.YearReportsPath, MainViewModel.Instance.Reports.SelectedYearReport));
            }
        }

        private void ParametersTab_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (MainViewModel.Instance.User != null && MainViewModel.Instance.User.IsAdmin)
            {
                ParametersTab.IsSelected = true;
            }
            else
            {
                EntryTab.IsSelected = true;
            }
        }

        private void AdminPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (MainViewModel.Instance.Parameters.BaseParameters != null)
            {
                MainViewModel.Instance.Parameters.BaseParameters.AdminPassword = AdminPassword.Password;
            }
        }
    }
}

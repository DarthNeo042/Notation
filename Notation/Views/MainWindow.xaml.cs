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

        public MainWindow()
        {
            DataContext = MainViewModel.Instance;
            CommandBindings.AddRange(MainViewModel.Instance.Bindings);
            CommandBindings.AddRange(MainViewModel.Instance.Parameters.Bindings);
            CommandBindings.AddRange(MainViewModel.Instance.Models.Bindings);
            CommandBindings.AddRange(MainViewModel.Instance.Entry.Bindings);

            _updatePeriodModels = new UpdatePeriodModelsDelegate(UpdatePeriodModels);
            _updatePeriodModelsDispatch = new UpdatePeriodModelsDelegate(UpdatePeriodModelsDispatch);
            _updateSemiTrimesterModels = new UpdateSemiTrimesterModelsDelegate(UpdateSemiTrimesterModels);
            _updateSemiTrimesterModelsDispatch = new UpdateSemiTrimesterModelsDelegate(UpdateSemiTrimesterModelsDispatch);
            _updateTrimesterModels = new UpdateTrimesterModelsDelegate(UpdateTrimesterModels);
            _updateTrimesterModelsDispatch = new UpdateTrimesterModelsDelegate(UpdateTrimesterModelsDispatch);
            _updateImport = new UpdateImportDelegate(UpdateImport);

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
                MessageBox.Show("Import terminé.", "Fin", MessageBoxButton.OK, MessageBoxImage.Information);
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
                SSRSUtils.CreatePeriodReport(period);
            }
        }

        private void ReportSemiTrimester_Click(object sender, RoutedEventArgs e)
        {
            if (((Control)sender).DataContext is SemiTrimesterViewModel semiTrimester)
            {
                SSRSUtils.CreateSemiTrimesterReport(semiTrimester);
            }
        }

        private void ReportTrimester_Click(object sender, RoutedEventArgs e)
        {
            if (((Control)sender).DataContext is int trimester)
            {
                SSRSUtils.CreateTrimesterReport(trimester);
            }
        }

        private void ParametersTab_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (MainViewModel.Instance.User.IsAdmin)
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

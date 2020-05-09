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
        public MainWindow()
        {
            DataContext = MainViewModel.Instance;
            CommandBindings.AddRange(MainViewModel.Instance.Bindings);
            CommandBindings.AddRange(MainViewModel.Instance.Parameters.Bindings);
            CommandBindings.AddRange(MainViewModel.Instance.Models.Bindings);
            InitializeComponent();

            for (int i = 1; i <= 12; i++)
            {
                ComboPeriodNumber.Items.Add(i);
            }
            for (int i = 1; i <= 9; i++)
            {
                ComboPeriodTrimester.Items.Add(i);
            }
            ComboTeacherTitle.ItemsSource = new List<string>() { "M.", "Mme", "Mlle", "Ab.", "Fr." };
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
                ExportUtils.ExportPeriodModels(period);
            }
        }

        private void ExportSemiTrimester_Click(object sender, RoutedEventArgs e)
        {
            if (((Control)sender).DataContext is SemiTrimesterViewModel semiTrimester)
            {
                ExportUtils.ExportSemiTrimesterModels(semiTrimester);
            }
        }

        private void ImportModel_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                foreach (string file in files)
                {
                    ExportUtils.Import(file);
                }
            }
        }

        private void PeriodModel_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!string.IsNullOrEmpty(MainViewModel.Instance.Models.PeriodModelsPath) && !string.IsNullOrEmpty(MainViewModel.Instance.Models.SelectedPeriodModel))
            {
                Process.Start(Path.Combine(MainViewModel.Instance.Models.PeriodModelsPath, MainViewModel.Instance.Models.SelectedPeriodModel));
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
    }
}

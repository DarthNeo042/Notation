﻿using Notation.Models;
using Notation.Utils;
using Notation.Views;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace Notation.ViewModels
{
    public class MainViewModel : DependencyObject
    {
        private static MainViewModel _instance;
        public static MainViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MainViewModel();
                }
                return _instance;
            }
        }

        public UserViewModel User
        {
            get { return (UserViewModel)GetValue(UserProperty); }
            set { SetValue(UserProperty, value); }
        }

        // Using a DependencyProperty as the backing store for User.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UserProperty =
            DependencyProperty.Register("User", typeof(UserViewModel), typeof(MainViewModel), new PropertyMetadata(null));

        public LoginViewModel Login
        {
            get { return (LoginViewModel)GetValue(LoginProperty); }
            set { SetValue(LoginProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Login.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LoginProperty =
            DependencyProperty.Register("Login", typeof(LoginViewModel), typeof(MainViewModel), new PropertyMetadata(null));

        public int SelectedYear
        {
            get { return (int)GetValue(SelectedYearProperty); }
            set { SetValue(SelectedYearProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedYear.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedYearProperty =
            DependencyProperty.Register("SelectedYear", typeof(int), typeof(MainViewModel), new PropertyMetadata(0, SelectedYearChanged));

        private static void SelectedYearChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MainViewModel mainViewModel = (MainViewModel)d;
            mainViewModel.Parameters.Year = mainViewModel.SelectedYear;
            if (mainViewModel.User != null)
            {
                mainViewModel.Parameters.LoadData();
                mainViewModel.Models.LoadData();
                mainViewModel.Reports.LoadData();
            }
        }

        public ObservableCollection<int> Years { get; set; }

        public ParametersViewModel Parameters
        {
            get { return (ParametersViewModel)GetValue(ParametersProperty); }
            set { SetValue(ParametersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Parameters.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ParametersProperty =
            DependencyProperty.Register("Parameters", typeof(ParametersViewModel), typeof(MainViewModel), new PropertyMetadata(null));

        public ReportsViewModel Reports
        {
            get { return (ReportsViewModel)GetValue(ReportsProperty); }
            set { SetValue(ReportsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Reports.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ReportsProperty =
            DependencyProperty.Register("Reports", typeof(ReportsViewModel), typeof(MainViewModel), new PropertyMetadata(null));

        public ModelsViewModel Models
        {
            get { return (ModelsViewModel)GetValue(ModelsProperty); }
            set { SetValue(ModelsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Models.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ModelsProperty =
            DependencyProperty.Register("Models", typeof(ModelsViewModel), typeof(MainViewModel), new PropertyMetadata(null));

        public EntryViewModel Entry
        {
            get { return (EntryViewModel)GetValue(EntryProperty); }
            set { SetValue(EntryProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Entry.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EntryProperty =
            DependencyProperty.Register("Entry", typeof(EntryViewModel), typeof(MainViewModel), new PropertyMetadata(null));

        public ICommand LoginCommand { get; set; }

        private void LoginExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            User = null;
            Login.Login = "";
            Login.Password = "";
        }

        public ICommand RefreshCommand { get; set; }

        private void RefreshExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Parameters.LoadData();
            Models.LoadData();
            Reports.LoadData();
        }

        public ICommand HelpCommand { get; set; }

        private void HelpExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Notation.Resources.Aide.pdf"))
                {
                    using (FileStream file = new FileStream("Aide.pdf", FileMode.Create))
                    {
                        stream.CopyTo(file);
                    }
                }
            }
            catch
            {
            }
            if (File.Exists("Aide.pdf"))
            {
                Process.Start(new ProcessStartInfo("Aide.pdf")
                    {
                        UseShellExecute = true,
                    }
                );
            }
        }

        public ICommand BackupCommand { get; set; }

        private void BackupCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = User?.IsAdmin ?? false;
        }

        private void BackupExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                string directoryName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Backups");
                Directory.CreateDirectory(directoryName);
                string filename = Path.Combine(directoryName, $"{Parameters.BaseParameters.Name}_{DateTime.Now.ToString("ddMMyy_HHmmss")}.bak");

                BaseModel.Save(filename);

                Process.Start("explorer", $"/root,{Path.GetDirectoryName(filename)}");
            }
            catch
            {
            }
        }

        public ICommand AddYearCommand { get; set; }

        private void AddYearCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = User?.IsAdmin ?? false;
        }

        private void AddYearExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            CreateYear createYear = new CreateYear();
            if (createYear.ShowDialog() ?? false)
            {
                SelectedYear = createYear.SelectedCopyYear;
                YearUtils.CreateYear(createYear.Year);
            }
        }

        public ICommand DeleteYearCommand { get; set; }

        private void DeleteYearCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (User?.IsAdmin ?? false) && SelectedYear != 0;
        }

        private void DeleteYearExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (MessageBox.Show($"Voulez-vous supprimer l'année {SelectedYear}/{SelectedYear + 1} ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes
                && MessageBox.Show($"Êtes vous vraiment sûr de vouloir supprimer l'année {SelectedYear}/{SelectedYear + 1} ?", "Confirmation (2)", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                YearUtils.DeleteYear(SelectedYear);
            }
            SelectedYear = Years.LastOrDefault();
        }

        public CommandBindingCollection Bindings { get; set; }

        public MainViewModel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            Login = new LoginViewModel();
            Models = new ModelsViewModel();
            Parameters = new ParametersViewModel();
            Reports = new ReportsViewModel();
            Entry = new EntryViewModel();
            Years = new ObservableCollection<int>();

            AddYearCommand = new RoutedUICommand("AddYear", "AddYear", typeof(MainViewModel));
            BackupCommand = new RoutedUICommand("Backup", "Backup", typeof(MainViewModel));
            DeleteYearCommand = new RoutedUICommand("DeleteYear", "DeleteYear", typeof(MainViewModel));
            LoginCommand = new RoutedUICommand("Login", "Login", typeof(MainViewModel));
            RefreshCommand = new RoutedUICommand("Refresh", "Refresh", typeof(MainViewModel));
            HelpCommand = new RoutedUICommand("Help", "Help", typeof(MainViewModel));

            Bindings = new CommandBindingCollection()
            {
                new CommandBinding(AddYearCommand, AddYearExecuted, AddYearCanExecute),
                new CommandBinding(BackupCommand, BackupExecuted, BackupCanExecute),
                new CommandBinding(DeleteYearCommand, DeleteYearExecuted, DeleteYearCanExecute),
                new CommandBinding(LoginCommand, LoginExecuted),
                new CommandBinding(RefreshCommand, RefreshExecuted),
                new CommandBinding(HelpCommand, HelpExecuted),
            };

            LoadYears();
        }

        public void LoadYears()
        {
            SelectedYear = 0;
            foreach (int year in Years.Where(y => y != 0).ToList())
            {
                Years.Remove(year);
            }
            if (!Years.Contains(0))
            {
                Years.Add(0);
            }
            foreach (int year in YearModel.List())
            {
                Years.Add(year);
            }
            SelectedYear = YearModel.GetCurrentYear();
        }

        public void LoadYears(IEnumerable<int> years)
        {
            SelectedYear = 0;
            foreach (int year in Years.Where(y => y != 0).ToList())
            {
                Years.Remove(year);
            }
            if (!Years.Contains(0))
            {
                Years.Add(0);
            }
            foreach (int year in years)
            {
                Years.Add(year);
            }
            SelectedYear = YearModel.GetCurrentYear();
            if (SelectedYear == 0 || !Years.Contains(SelectedYear))
            {
                SelectedYear = Years.LastOrDefault();
            }
        }
    }
}

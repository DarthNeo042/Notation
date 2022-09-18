using Notation.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Notation.ViewModels
{
    public class LoginViewModel : DependencyObject
    {
        public string Login
        {
            get { return (string)GetValue(LoginProperty); }
            set { SetValue(LoginProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Login.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LoginProperty =
            DependencyProperty.Register("Login", typeof(string), typeof(LoginViewModel), new PropertyMetadata(""));

        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Password.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(LoginViewModel), new PropertyMetadata(""));

        public delegate void CloseEventHandler();

        public event CloseEventHandler ValidateEvent;

        public ICommand ValidateCommand { get; set; }

        private void ValidateCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if ((MainViewModel.Instance.Parameters.BaseParameters.AdminLogin == Login && MainViewModel.Instance.Parameters.BaseParameters.AdminPassword == Password)
                || TeacherModel.Login(Login, Password).Any())
            {
                if (Login == MainViewModel.Instance.Parameters.BaseParameters.AdminLogin
                    && Password == MainViewModel.Instance.Parameters.BaseParameters.AdminPassword)
                {
                    MainViewModel.Instance.User = new UserViewModel()
                    {
                        Name = "Administrateur",
                        IsAdmin = true,
                    };
                }
                else
                {
                    IEnumerable<int> years = TeacherModel.Login(Login, Password);
                    MainViewModel.Instance.LoadYears(years);
                    TeacherViewModel teacher = TeacherModel.Login(Login, Password, MainViewModel.Instance.SelectedYear);
                    MainViewModel.Instance.User = new UserViewModel()
                    {
                        Name = $"{teacher.Title} {teacher.FirstName} {teacher.LastName}",
                        Teacher = teacher,
                    };
                }
                MainViewModel.Instance.Parameters.LoadData();
                MainViewModel.Instance.Models.LoadData();
                MainViewModel.Instance.Reports.LoadData();
            }
            else
            {
                MessageBox.Show("Erreur d'authentification", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public CommandBindingCollection Bindings { get; set; }

        public LoginViewModel()
        {
            ValidateCommand = new RoutedUICommand("Validate", "Validate", typeof(LoginViewModel));

            Bindings = new CommandBindingCollection()
            {
                new CommandBinding(ValidateCommand, ValidateCommandExecuted),
            };
        }
    }
}

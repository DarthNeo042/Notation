using Notation.ViewModels;
using System.Windows;

namespace Notation.Views
{
    /// <summary>
    /// Logique d'interaction pour Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login(LoginViewModel login)
        {
            DataContext = login;
            CommandBindings.AddRange(login.Bindings);
            InitializeComponent();

            login.ValidateEvent += Login_ValidateEvent;
            login.CancelEvent += () => Close();

            Password.Password = login.Password;
        }

        private void Login_ValidateEvent()
        {
            DialogResult = true;
            Close();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            LoginViewModel login = (LoginViewModel)DataContext;
            login.Password = Password.Password;
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                Validate.Command.Execute(null);
            }
            else if (e.Key == System.Windows.Input.Key.Escape)
            {
                Cancel.Command.Execute(null);
            }
        }
    }
}

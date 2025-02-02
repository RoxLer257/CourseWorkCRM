using CourseWork25.Classes;
using CourseWork25.BD;
using CourseWork25.Pages.Main;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace CourseWork25.Pages.Logon
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Page
    {
        public Authorization()
        {
            InitializeComponent();
        }

        private void BtnEnter_Click(object sender, RoutedEventArgs e)
        {
            string login = TxtLogin.Text;
            string password = TxtPass.Password;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Необходимо заполнить все поля", "Ошибка при авторизации",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!PasswordValidator.IsPasswordValid(password))
            {
                TxtLogin.Clear();
                TxtPass.ToolTip = "Пароль не соответствует требованиям";
                TxtPass.Background = Brushes.Red;
                MessageBox.Show("Пароль не соответствует требованиям", "Ошибка при авторизации",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                TxtLogin.Background = Brushes.Transparent;
                return;
            }

            try
            {
                using (var context = new CourseWork25Entities())
                {
                    var user = context.Users.FirstOrDefault(u => u.Username == login);

                    if (user == null || !string.Equals(user.Username, login, StringComparison.Ordinal))
                    {
                        TxtLogin.ToolTip = "Неверный логин";
                        TxtLogin.Background = Brushes.Red;
                        MessageBox.Show("Неверный логин", "Ошибка при авторизации",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        TxtLogin.Background = Brushes.Transparent;
                        return;
                    }

                    if (user.Password != password)
                    {
                        TxtPass.Clear();
                        TxtPass.ToolTip = "Неправильный пароль";
                        TxtPass.Background = Brushes.Red;
                        MessageBox.Show("Неправильный пароль", "Ошибка при авторизации",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        TxtPass.Background = Brushes.Transparent;
                        return;
                    }

                    ClassFrame.Role_Id = user.UserID;
                    CurrentUser.Login = user.Username;
                    CurrentUser.Password = user.Password;
                    CurrentUser.RoleName = context.Roles.FirstOrDefault(r => r.RoleID == user.Roles.RoleID)?.RoleName;
                    ClassFrame.frmObj.Navigate(new MainPage());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка при авторизации",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void BtnReg_Click(object sender, RoutedEventArgs e)
        {
            ForgotUPass forgotUPassWindow = new ForgotUPass();
            forgotUPassWindow.Show();
        }
    }
}

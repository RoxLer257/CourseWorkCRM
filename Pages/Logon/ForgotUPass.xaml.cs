using CourseWork25.BD;
using System.Linq;
using System.Windows;

namespace CourseWork25.Pages.Logon
{
    /// <summary>
    /// Логика взаимодействия для ForgotUPass.xaml
    /// </summary>
    public partial class ForgotUPass : Window
    {
        public ForgotUPass()
        {
            InitializeComponent();
        }

        private void ForgUPass_Click(object sender, RoutedEventArgs e)
        {
            string login = NumPol.Text;

            using (var context = new CourseWork25Entities())
            {
                var user = context.Users.FirstOrDefault(u => u.Username == login);

                if (user != null)
                {
                    Password.Text = user.Password;
                }
                else
                {
                    Password.Text = "Пользователь не найден";
                }
            }
        }
    }
}

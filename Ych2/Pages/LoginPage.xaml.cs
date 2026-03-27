using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Ych2.DBCon;

namespace Ych2.Pages
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }
        private void btnVhod_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txbLogin.Text) || string.IsNullOrEmpty(pswbPassword.Password))
            {
                MessageBox.Show("ГОЛОВУ ПЖ НАПРЯГИ... ПОЛЯ ВСЕ НУЖНО ЗАПОЛНИТЬ", "ОШИБКА АВТОРИЗАЦИИ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            try
            {
                var user = DBCon.Conn.comfortEntities.Logins.FirstOrDefault(a => a.Login == txbLogin.Text.Trim() && a.Password == pswbPassword.Password.Trim());
                if (user != null)
                {
                    CurrentUser.User = user;
                    NavigationService.Navigate(new PartnerListPage());
                }
                else
                {
                    MessageBox.Show("и такие дни выбают", "вспомни данные для входа", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ошибка при подключении бд {ex.Message}", "грустно конечно", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void btnReg_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegPage());
        }
    }
}

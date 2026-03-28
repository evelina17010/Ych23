using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Ych2.DBCon;

namespace Ych2.Pages
{
    public partial class RegPage : Page
    {
        public RegPage()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) ||string.IsNullOrWhiteSpace(txtSurname.Text) ||
                string.IsNullOrWhiteSpace(txtPassportSeria.Text) || string.IsNullOrWhiteSpace(txtPassportNumber.Text) ||
                string.IsNullOrWhiteSpace(txtLogin.Text) || string.IsNullOrWhiteSpace(pswPassword.Password))
            {
                MessageBox.Show("Заполните все поля!!!!");
            }
            else if (txtPassportSeria.Text.Length != 4)
            {
                MessageBox.Show("Серия паспорта - 4 цифры!");
            }
            else if (txtPassportNumber.Text.Length != 6)
            {
                MessageBox.Show("Номер паспорта - 6 цифр!");
            }
            else if (Conn.comfortEntities.Logins.Any(x => x.Login == txtLogin.Text))
            {
                MessageBox.Show("Логин занят");
            }
            else if (pswPassword.Password.Length < 3)
            {
                MessageBox.Show("Пароль должен быть минимум 3 символа!");
            }
            else if (pswPassword.Password != pswConfirmPassword.Password)
            {
                MessageBox.Show("Пароли не совпадают!");
            }
            else
            {
                try
                {
                    var employee = Conn.comfortEntities.Employee.Add(new Employee()
                    {
                        Name = txtName.Text,
                        Surname = txtSurname.Text,
                        Patronumic = string.IsNullOrWhiteSpace(txtPatronumic.Text) ? null : txtPatronumic.Text,
                        Birthday = dpBirthday.SelectedDate,
                        PassportSeria = txtPassportSeria.Text,
                        PassportNumber = txtPassportNumber.Text,
                        BankDetails = string.IsNullOrWhiteSpace(txtBankDetails.Text) ? null : txtBankDetails.Text,
                        Id_role = 2 
                    });

                    Conn.comfortEntities.SaveChanges();
                    Conn.comfortEntities.Logins.Add(new Logins()
                    {
                        Login = txtLogin.Text,
                        Password = pswPassword.Password,
                        Id_user = employee.Id_employee
                    });

                    Conn.comfortEntities.SaveChanges();

                    MessageBox.Show("Регистрация успешна!");
                    NavigationService.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
        }
    }
}
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
    /// Логика взаимодействия для RegPage.xaml
    /// </summary>
    public partial class RegPage : Page
    {
        public RegPage()
        {
            InitializeComponent();
            LoadComboBoxData();
        }

        private void LoadComboBoxData()
        {
            try
            {
                cmbFamilyStatus.ItemsSource = DBCon.Conn.comfortEntities.FamilyStatus.ToList();
                cmbHealth.ItemsSource = DBCon.Conn.comfortEntities.Health.ToList();
                cmbRole.ItemsSource = DBCon.Conn.comfortEntities.Role.ToList();
                if (cmbFamilyStatus.Items.Count > 0)
                    cmbFamilyStatus.SelectedIndex = 0;
                if (cmbHealth.Items.Count > 0)
                    cmbHealth.SelectedIndex = 0;
                if (cmbRole.Items.Count > 0)
                    cmbRole.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateData()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("ТЫ ИМЕНИ СВОЕГО НЕ ЗНАЕШЬ!?", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtName.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtSurname.Text))
            {
                MessageBox.Show("ФАМИЛИЮ ВВЕДИ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtSurname.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPassportSeria.Text) || txtPassportSeria.Text.Length != 4)
            {
                MessageBox.Show("Введите серию паспорта (4 цифры)!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPassportSeria.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPassportNumber.Text) || txtPassportNumber.Text.Length != 6)
            {
                MessageBox.Show("Введите номер паспорта (6 цифр)!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPassportNumber.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtLogin.Text))
            {
                MessageBox.Show("Введите логин!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtLogin.Focus();
                return false;
            }
            var existingLogin = DBCon.Conn.comfortEntities.Logins.FirstOrDefault(l => l.Login == txtLogin.Text.Trim());
            if (existingLogin != null)
            {
                MessageBox.Show("БУДЬ ОРИГИНАЛЬНЕЕ...Такой логин уже существует! ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtLogin.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(pswPassword.Password))
            {
                MessageBox.Show("Введите пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                pswPassword.Focus();
                return false;
            }

            if (pswPassword.Password != pswConfirmPassword.Password)
            {
                MessageBox.Show("ТЫ СВОЕЙ ЖЕ ПАРОЛЬ ЗАБЫЛ ЗА 5 СЕК.?!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                pswConfirmPassword.Focus();
                return false;
            }

            if (pswPassword.Password.Length < 3)
            {
                MessageBox.Show("Пароль должен содержать минимум 3 символа!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                pswPassword.Focus();
                return false;
            }

            return true;
        }
        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateData())
                return;

            try
            {
                var newEmployee = new Employee
                {
                    Name = txtName.Text.Trim(),
                    Surname = txtSurname.Text.Trim(),
                    Patronumic = string.IsNullOrWhiteSpace(txtPatronumic.Text) ? null : txtPatronumic.Text.Trim(),
                    Birthday = dpBirthday.SelectedDate,
                    PassportSeria = txtPassportSeria.Text.Trim(),
                    PassportNumber = txtPassportNumber.Text.Trim(),
                    BankDetails = string.IsNullOrWhiteSpace(txtBankDetails.Text) ? null : txtBankDetails.Text.Trim(),
                    Id_family = (cmbFamilyStatus.SelectedItem as FamilyStatus)?.Id_status,
                    Id_health = (cmbHealth.SelectedItem as Health)?.Id_health,
                    Id_role = (cmbRole.SelectedItem as Role)?.Id_role
                };
                DBCon.Conn.comfortEntities.Employee.Add(newEmployee);
                DBCon.Conn.comfortEntities.SaveChanges();
                var newLogin = new Logins
                {
                    Login = txtLogin.Text.Trim(),
                    Password = pswPassword.Password.Trim(),
                    Id_user = newEmployee.Id_employee
                };
                DBCon.Conn.comfortEntities.Logins.Add(newLogin);
                DBCon.Conn.comfortEntities.SaveChanges();

                MessageBox.Show("ПОЗДРАВЛЯЮ!ТЫ СМОГ ЗАПОЛНИТЬ ВСЕ ПОЛЯ...О ЧУДО", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.Navigate(new LoginPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при регистрации: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LoginPage());
        }
    }
}
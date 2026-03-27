using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Ych2.DBCon;

namespace Ych2.Pages
{
    public partial class PartnerListPage : Page
    {
        private List<Partners> allPartners;
        private Partners selectedPartner;

        public PartnerListPage()
        {
            InitializeComponent();
            LoadPartners();
        }

        private void LoadPartners()
        {
            try
            {
                allPartners = DBCon.Conn.comfortEntities.Partners.ToList();
                UpdateListPartner();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateListPartner()
        {
            if (allPartners == null) return;

            var filteredPartners = allPartners.ToList();

            if (!string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                string searchText = SearchTextBox.Text.ToLower();
                filteredPartners = filteredPartners.Where(p => (p.NamePartner != null && p.NamePartner.ToLower().Contains(searchText)) ||
                    (p.TypeOfBusiness != null && p.TypeOfBusiness.NameBusiness != null && p.TypeOfBusiness.NameBusiness.ToLower().Contains(searchText)) ||
                    (p.Phone != null && p.Phone.ToLower().Contains(searchText)) ||(p.SurnameDirector != null && p.SurnameDirector.ToLower().Contains(searchText))).ToList();
            }
            if (SortComboBox.SelectedItem != null)
            {
                string selectedSort = (SortComboBox.SelectedItem as ComboBoxItem).Content.ToString();
                if (selectedSort == "По названию (A-Z)")
                {
                    filteredPartners = filteredPartners.OrderBy(p => p.NamePartner).ToList();
                }
                else if (selectedSort == "По названию (Z-A)")
                {
                    filteredPartners = filteredPartners.OrderByDescending(p => p.NamePartner).ToList();
                }
                else
                {
                    filteredPartners = filteredPartners.OrderBy(p => p.Id_partner).ToList();
                }
            }

            PartnersLv.ItemsSource = filteredPartners;
        }
        private void PartnersLv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedPartner = PartnersLv.SelectedItem as Partners;
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateListPartner();
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateListPartner();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditPartners(new Partners()));
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (selectedPartner != null)
            {
                NavigationService.Navigate(new AddEditPartners(selectedPartner));
            }
            else
            {
                MessageBox.Show("ВЫБЕРИ ПАРНЕРА", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (selectedPartner == null)
            {
                MessageBox.Show("Сначала выберите партнера!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            MessageBoxResult result = MessageBox.Show( $"Вы точно хотите удалить партнера \"{selectedPartner.NamePartner}\"?","Подтверждение удаления",
                MessageBoxButton.YesNo,MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    DBCon.Conn.comfortEntities.Partners.Remove(selectedPartner);
                    DBCon.Conn.comfortEntities.SaveChanges();

                    MessageBox.Show("Партнер удален!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadPartners();
                    selectedPartner = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnHistory_Click(object sender, RoutedEventArgs e)
        {
            if (selectedPartner != null)
            {
                NavigationService.Navigate(new PartnerHistoryPage(selectedPartner));
            }
            else
            {
                MessageBox.Show("ВЫБЕРИ ПАРТНЕРА", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
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
    /// Логика взаимодействия для AddEditPartners.xaml
    /// </summary>
    public partial class AddEditPartners : Page
    {
        Partners partner;
        public AddEditPartners(Partners _partner)
        {
            InitializeComponent();
            partner = _partner;
            this.DataContext = partner;
            typecmb.ItemsSource=DBCon.Conn.comfortEntities.TypeOfBusiness.ToList();
            typecmb.DisplayMemberPath = "NameBusiness";
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                partner.Id_type = (typecmb.SelectedItem as TypeOfBusiness).Id_type;
                if (partner.Id_partner == 0)
                {
                    DBCon.Conn.comfortEntities.Partners.Add(partner);
                }
                DBCon.Conn.comfortEntities.SaveChanges();
                MessageBox.Show("ВСЁ НОРМ", "вау", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.Navigate(new PartnerListPage());
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnOtmena_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("НУ ОКЕЕЕЙ...", "ГАЛЯ,ОТМЕНА", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigationService.Navigate(new PartnerListPage());
        }
    }
}

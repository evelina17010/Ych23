using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Ych2.DBCon;

namespace Ych2.Pages
{
    public partial class PartnerHistoryPage : Page
    {
        public PartnerHistoryPage(Partners partner)
        {
            InitializeComponent();
            txtPartnerName.Text = partner.NamePartner;
            txtPartnerPhone.Text = partner.Phone;
            LoadSales(partner.Id_partner);
        }

        private void LoadSales(int partnerId)
        {
            try
            {              
                var points = DBCon.Conn.comfortEntities.SalePoint.Where(p => p.Id_partner == partnerId).ToList();
                if (points.Count == 0)
                {
                    HistoryLv.ItemsSource = null;
                    txtTotal.Text = "0 руб.";
                    return;
                }
                List<int> pointIds = new List<int>();
                foreach (var point in points)
                {
                    pointIds.Add(point.Id_point);
                }
                var sales = DBCon.Conn.comfortEntities.SaleHistory.Where(s => pointIds.Contains(s.Id_point ?? 0)).ToList();
                if (sales.Count == 0)
                {
                    HistoryLv.ItemsSource = null;
                    txtTotal.Text = "0 руб.";
                    return;
                }
                var list = new List<object>();
                decimal total = 0;
                foreach (var sale in sales)
                {
                    var product = DBCon.Conn.comfortEntities.Products.FirstOrDefault(pr => pr.Id_product == sale.Id_product);
                    string productName = "Неизвестно";
                    if (product != null)
                    {
                        productName = product.NameProduct;
                    }
                    var point = DBCon.Conn.comfortEntities.SalePoint.FirstOrDefault(p => p.Id_point == sale.Id_point);
                    string pointName = "Неизвестно";
                    if (point != null)
                    {
                        pointName = point.NamePoint;
                    }
                    total = total + (sale.Amount ?? 0);
                    list.Add(new
                    {
                        ProductName = productName,
                        Quantity = sale.Quantity ?? 0,
                        Amount = (sale.Amount ?? 0).ToString("N0") + " руб.",
                        Point = pointName
                    });
                }

                HistoryLv.ItemsSource = list;
                txtTotal.Text = total.ToString("N0") + " руб.";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
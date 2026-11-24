using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using _2Modul.Models;

namespace _2Modul
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadPartners();
        }

        public int CalculateDiscount(int totalSales)
        {
            if (totalSales < 10000)
            {
                return 0;
            }
            if (totalSales < 50000)
            {
                return 5;
            }
            if (totalSales < 300000)
            {
                return 10;
            }
            return 15;
        }

        private void LoadPartners()
        {
            try
            {
                using (var db = new ASKruchProductExcelDBEntities1())
                {
                    var partnersFromDb = db.Partners.ToList();
                    List<PartnerCardItem> list = new List<PartnerCardItem>();

                    foreach (var p in partnersFromDb)
                    {
                        int totalSales = 0;
                        foreach (var s in p.PartnerSales)
                        {
                            totalSales += s.Quantity;
                        }

                        int discount = CalculateDiscount(totalSales);

                        PartnerCardItem item = new PartnerCardItem();
                        item.IDPartner = p.IDPartner;
                        item.PartnerType = p.PartnerTypes.TypeName;
                        item.PartnerName = p.PartnerName;
                        item.DirectorName = p.DirectorName;
                        item.Phone = p.Phone;
                        item.Rating = p.Rating ?? 0;
                        item.Discount = discount;

                        list.Add(item);
                    }

                    PartnersListView.ItemsSource = list;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных:\n" + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddPartnerButton_Click(object sender, RoutedEventArgs e)
        {
            PartnerEditWindow window = new PartnerEditWindow(null);
            window.ShowDialog();
            LoadPartners();
        }

        private void PartnersListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            PartnerCardItem item = PartnersListView.SelectedItem as PartnerCardItem;
            if (item != null)
            {
                PartnerEditWindow window = new PartnerEditWindow(item.IDPartner);
                window.ShowDialog();
                LoadPartners();
            }
        }
    }
}

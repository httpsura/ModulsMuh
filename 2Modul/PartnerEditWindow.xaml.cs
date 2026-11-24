using System;
using System.Linq;
using System.Windows;
using _2Modul.Models;

namespace _2Modul
{
    public partial class PartnerEditWindow : Window
    {
        private int? _partnerId;

        public PartnerEditWindow(int? partnerId)
        {
            InitializeComponent();
            _partnerId = partnerId;

            using (var db = new ASKruchProductExcelDBEntities1())
            {
                TypeComboBox.ItemsSource = db.PartnerTypes.ToList();

                if (_partnerId != null)
                {
                    this.Title = "Редактирование партнера";
                    HeaderTextBlock.Text = "Редактирование партнера";
                    var p = db.Partners.Find(_partnerId);
                    NameTextBox.Text = p.PartnerName;
                    TypeComboBox.SelectedValue = p.IDType;
                    RatingTextBox.Text = p.Rating.ToString();
                    AddressTextBox.Text = p.LegalAddress;
                    DirectorTextBox.Text = p.DirectorName;
                    PhoneTextBox.Text = p.Phone;
                    EmailTextBox.Text = p.Email;
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text) || TypeComboBox.SelectedValue == null)
            {
                MessageBox.Show("Заполните обязательные поля (Наименование и Тип).", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int rating;
            if (!int.TryParse(RatingTextBox.Text, out rating) || rating < 0)
            {
                MessageBox.Show("Рейтинг должен быть целым неотрицательным числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                using (var db = new ASKruchProductExcelDBEntities1())
                {
                    var p = _partnerId == null ? new Partners() : db.Partners.Find(_partnerId);
                    if (_partnerId == null) db.Partners.Add(p);

                    p.PartnerName = NameTextBox.Text;
                    p.IDType = (int)TypeComboBox.SelectedValue;
                    p.Rating = rating;
                    p.LegalAddress = AddressTextBox.Text;
                    p.DirectorName = DirectorTextBox.Text;
                    p.Phone = PhoneTextBox.Text;
                    p.Email = EmailTextBox.Text;

                    db.SaveChanges();
                    MessageBox.Show("Данные сохранены.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

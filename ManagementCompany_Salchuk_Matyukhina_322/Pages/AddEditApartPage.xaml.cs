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

namespace ManagementCompany_Salchuk_Matyukhina_322.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditApartPage.xaml
    /// </summary>
    public partial class AddEditApartPage : Page
    {
        private Apartment _currentApart = new Apartment();
        private Entities _context = new Entities();
        public AddEditApartPage(Apartment selectedApart)
        {
            InitializeComponent();
            if (selectedApart != null)
            {
                _currentApart = selectedApart;

            }
            DataContext = _currentApart;

        }

        private void savebutton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(numbertextbox.Text) || !decimal.TryParse(numbertextbox.Text, out decimal apartValue) || apartValue <= 0)
                errors.AppendLine("Укажите номер квартиры!");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Проверьте введенные данные",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                if (_currentApart.Apartment_id == 0)
                {
                    _context.Apartments.Add(_currentApart);
                }
                else
                {
                    var existingAdt = _context.Apartments.Find(_currentApart.Apartment_id);
                    if (existingAdt != null)
                    {
                        _context.Entry(existingAdt).CurrentValues.SetValues(_currentApart);
                    }
                }

                _context.SaveChanges();
                MessageBox.Show("Данные успешно сохранены!", "Успех",
                              MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService?.Navigate(new Pages.ApartPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void ButtonClean_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите очистить все поля?", "Подтверждение очистки",
                                       MessageBoxButton.YesNo, MessageBoxImage.Question);

            _currentApart = new Apartment();
            DataContext = _currentApart;

            MessageBox.Show("Все поля очищены", "Информация",
                              MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}


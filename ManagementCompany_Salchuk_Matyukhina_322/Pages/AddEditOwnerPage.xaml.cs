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
    /// Логика взаимодействия для AddEditOwnerPage.xaml
    /// </summary>
    public partial class AddEditOwnerPage : Page
    {
        private Owner _currentOwner = new Owner();
        private Entities _context = new Entities();
        public AddEditOwnerPage(Owner selectedOwner)
        {
            InitializeComponent();
            if (selectedOwner != null)
            {
                _currentOwner = selectedOwner;

            }
            DataContext = _currentOwner;
        }

        private void savebutton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentOwner.Name))
                errors.AppendLine("Укажите ФИО!");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Проверьте введенные данные",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                if (_currentOwner.Owner_id == 0)
                {
                    _context.Owners.Add(_currentOwner);
                }
                else
                {
                    var existingAdt = _context.Owners.Find(_currentOwner.Owner_id);
                    if (existingAdt != null)
                    {
                        _context.Entry(existingAdt).CurrentValues.SetValues(_currentOwner);
                    }
                }

                _context.SaveChanges();
                MessageBox.Show("Данные успешно сохранены!", "Успех",
                              MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService?.Navigate(new Pages.OwnerPage());
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

            _currentOwner = new Owner();
            DataContext = _currentOwner;

            MessageBox.Show("Все поля очищены", "Информация",
                              MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}

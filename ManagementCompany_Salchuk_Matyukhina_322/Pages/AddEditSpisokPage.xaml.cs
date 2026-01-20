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
using System.Data.Entity;

namespace ManagementCompany_Salchuk_Matyukhina_322.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditSpisokPage.xaml
    /// </summary>
    public partial class AddEditSpisokPage : Page
    {
        private SpisokJilogoFonda _currentSpisok = new SpisokJilogoFonda();
        private Entities _context = new Entities();
        public AddEditSpisokPage(SpisokJilogoFonda selectedSpisok)
        {
            InitializeComponent();
            if (selectedSpisok != null)
            {
                _currentSpisok = selectedSpisok;
            }
            DataContext = _currentSpisok;
        }

        private void savebutton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentSpisok.Adress))
                errors.AppendLine("Укажите адрес!");
            if (_currentSpisok.StartDate == null || _currentSpisok.StartDate == DateTime.MinValue)
                errors.AppendLine("Укажите корректную дату!");
            if (string.IsNullOrWhiteSpace(floorstextbox.Text) || !decimal.TryParse(floorstextbox.Text, out decimal floorsValue) || floorsValue <= 0)
                errors.AppendLine("Укажите количество этажей!");
            if (string.IsNullOrWhiteSpace(aparttextbox.Text) || !decimal.TryParse(aparttextbox.Text, out decimal apartValue) || apartValue <= 0)
                errors.AppendLine("Укажите количество квартир!");
            if (string.IsNullOrWhiteSpace(yeartextbox.Text) || !decimal.TryParse(yeartextbox.Text, out decimal yearValue) || yearValue <= 0)
                errors.AppendLine("Укажите год!");
            if (string.IsNullOrWhiteSpace(squaretextbox.Text) || !decimal.TryParse(squaretextbox.Text, out decimal squareValue) || squareValue <= 0)
                errors.AppendLine("Укажите площадь дома!");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Проверьте введенные данные",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (_currentSpisok.Fond_id == 0)
                {
                    _context.SpisokJilogoFondas.Add(_currentSpisok);
                }
                else
                {
                    var existingAdt = _context.SpisokJilogoFondas.Find(_currentSpisok.Fond_id);
                    if (existingAdt != null)
                    {
                        _context.Entry(existingAdt).CurrentValues.SetValues(_currentSpisok);
                    }
                }

                _context.SaveChanges();
                MessageBox.Show("Данные успешно сохранены!", "Успех",
                              MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService?.Navigate(new Pages.SpisokPage());
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

            _currentSpisok = new SpisokJilogoFonda();
            DataContext = _currentSpisok;

            MessageBox.Show("Все поля очищены", "Информация",
                              MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}

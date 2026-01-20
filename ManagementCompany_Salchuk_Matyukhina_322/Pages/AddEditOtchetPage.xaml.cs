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
    /// Логика взаимодействия для AddEditOtchetPage.xaml
    /// </summary>
    public partial class AddEditOtchetPage : Page
    {
        private OtchetPoOplate _currentOtchet = new OtchetPoOplate();
        private Entities _context = new Entities();
        public AddEditOtchetPage(OtchetPoOplate selectedOtchet)
        {
            InitializeComponent();
            if (selectedOtchet != null)
            {
                if (selectedOtchet.PaymentId != 0)
                {
                    // Находим объект в базе данных
                    _currentOtchet = _context.OtchetPoOplates
                        .Include(d => d.Owner1)
                        .FirstOrDefault(d => d.PaymentId == selectedOtchet.PaymentId) ?? new OtchetPoOplate();
                }
                else
                {
                    // Новая запись - просто присваиваем
                    _currentOtchet = selectedOtchet;
                }
            }

            DataContext = _currentOtchet;

            try
            {
                comboboxowner.ItemsSource = _context.Owners.ToList();

                comboboxowner.DisplayMemberPath = "Name";
                comboboxowner.SelectedValuePath = "Owner_id";

                if (_currentOtchet.PaymentId != 0)
                {
                    _context.OtchetPoOplates.Attach(_currentOtchet);
                    _context.Entry(_currentOtchet).Reference(d => d.Owner1).Load();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonClean_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите очистить все поля?", "Подтверждение очистки",
                                       MessageBoxButton.YesNo, MessageBoxImage.Question);

            _currentOtchet = new OtchetPoOplate();
            DataContext = _currentOtchet;
            comboboxowner.SelectedIndex = -1;

            MessageBox.Show("Все поля очищены", "Информация",
                              MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void savebutton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentOtchet.Adress))
                errors.AppendLine("Укажите адрес!");
            if (string.IsNullOrWhiteSpace(_currentOtchet.Period))
                errors.AppendLine("Укажите период оплаты!");
            if (string.IsNullOrWhiteSpace(aparttextbox.Text) || !decimal.TryParse(aparttextbox.Text, out decimal apartValue) || apartValue <= 0)
                errors.AppendLine("Укажите номер квартиры!");
            if (string.IsNullOrWhiteSpace(accruedtextbox.Text) || !decimal.TryParse(accruedtextbox.Text, out decimal accruedValue) || accruedValue <= 0)
                errors.AppendLine("Укажите сумма начисления!");
            if (!decimal.TryParse(paidtextbox.Text, out decimal paidValue) || paidValue < 0)
                errors.AppendLine("Укажите сумму оплаты");
            else
                _currentOtchet.Paid = paidValue;

            // Проверка владельца
            if (comboboxowner.SelectedItem == null)
                errors.AppendLine("Выберите владельца!");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Проверьте введенные данные",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Устанавливаем владельца из ComboBox
                if (comboboxowner.SelectedItem is Owner selectedOwner)
                {
                    _currentOtchet.Owner1 = selectedOwner;
                    _currentOtchet.Owner = selectedOwner.Owner_id;
                }

                if (_currentOtchet.PaymentId == 0)
                {
                    _context.OtchetPoOplates.Add(_currentOtchet);
                }
                else
                {
                    var existingOtchet = _context.OtchetPoOplates.Find(_currentOtchet.PaymentId);
                    if (existingOtchet != null)
                    {
                        _context.Entry(existingOtchet).CurrentValues.SetValues(_currentOtchet);
                    }
                }

                _context.SaveChanges();
                MessageBox.Show("Данные успешно сохранены!", "Успех",
                              MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService?.Navigate(new Pages.DebtPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

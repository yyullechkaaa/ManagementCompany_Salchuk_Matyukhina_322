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
using System.Data.Entity.Validation;


namespace ManagementCompany_Salchuk_Matyukhina_322.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditDebtPage.xaml
    /// </summary>
    public partial class AddEditDebtPage : Page
    {
        private Debt _currentDebt = new Debt();
        private Entities _context = new Entities();
        public AddEditDebtPage(Debt selectedDebt)
        {
            InitializeComponent();
            if (selectedDebt != null)
            {
                if (selectedDebt.Debt_id != 0)
                {
                    // Находим объект в базе данных
                    _currentDebt = _context.Debts
                        .Include(d => d.Owner1)
                        .FirstOrDefault(d => d.Debt_id == selectedDebt.Debt_id) ?? new Debt();
                }
                else
                {
                    // Новая запись - просто присваиваем
                    _currentDebt = selectedDebt;
                }
            }

            DataContext = _currentDebt;

            try
            {
                comboboxowner.ItemsSource = _context.Owners.ToList();

                comboboxowner.DisplayMemberPath = "Name"; 
                comboboxowner.SelectedValuePath = "Owner_id";

                if (_currentDebt.Debt_id != 0)
                {
                    _context.Debts.Attach(_currentDebt);
                    _context.Entry(_currentDebt).Reference(d => d.Owner1).Load();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void savebutton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentDebt.Adress))
                errors.AppendLine("Укажите адрес!");
            if (string.IsNullOrWhiteSpace(_currentDebt.Phone))
                errors.AppendLine("Укажите номер телефона!");
            if (string.IsNullOrWhiteSpace(aparttextbox.Text) || !decimal.TryParse(aparttextbox.Text, out decimal apartValue) || apartValue <= 0)
                errors.AppendLine("Укажите количество квартир!");
            if (string.IsNullOrWhiteSpace(watertextbox.Text) || !decimal.TryParse(watertextbox.Text, out decimal waterValue) || waterValue <= 0)
                errors.AppendLine("Укажите счет на воду!");
            if (!decimal.TryParse(electrotextbox.Text, out decimal electroValue) || electroValue < 0)
                errors.AppendLine("Укажите корректный счет на электроэнергию (число, может быть 0)!");
            else
                _currentDebt.Electricity = electroValue;

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
                    _currentDebt.Owner1 = selectedOwner;
                    _currentDebt.Owner = selectedOwner.Owner_id;
                }

                if (_currentDebt.Debt_id == 0)
                {
                    _context.Debts.Add(_currentDebt);
                }
                else
                {
                    var existingDebt = _context.Debts.Find(_currentDebt.Debt_id);
                    if (existingDebt != null)
                    {
                        _context.Entry(existingDebt).CurrentValues.SetValues(_currentDebt);
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

        private void ButtonClean_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите очистить все поля?", "Подтверждение очистки",
                                       MessageBoxButton.YesNo, MessageBoxImage.Question);

            _currentDebt = new Debt();
            DataContext = _currentDebt;
            comboboxowner.SelectedIndex = -1;

            MessageBox.Show("Все поля очищены", "Информация",
                              MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}

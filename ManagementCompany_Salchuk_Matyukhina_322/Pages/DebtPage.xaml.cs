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
using System.ComponentModel;
using System.Runtime.Remoting.Contexts;

namespace ManagementCompany_Salchuk_Matyukhina_322.Pages
{
    /// <summary>
    /// Логика взаимодействия для DebtPage.xaml
    /// </summary>
    public partial class DebtPage : Page
    {
        private Entities _context = new Entities();
        public DebtPage()
        {
            InitializeComponent();
            // Загружаем данные с включением связанных данных
            var currentDebt = _context.Debts
                    .Include(d => d.Owner1)
                    .AsNoTracking()
                    .ToList();
            ListViewDebt.ItemsSource = currentDebt;
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).DataContext is Debt selectedDebt)
            {
                var detachedDebt = new Debt
                {
                    Debt_id = selectedDebt.Debt_id,
                    Adress = selectedDebt.Adress,
                    Apartment = selectedDebt.Apartment,
                    Owner = selectedDebt.Owner,
                    Phone = selectedDebt.Phone,
                    Water = selectedDebt.Water,
                    Electricity = selectedDebt.Electricity
                };
                // Если нужно передать владельца
                if (selectedDebt.Owner1 != null)
                {
                    detachedDebt.Owner1 = new Owner
                    {
                        Owner_id = selectedDebt.Owner1.Owner_id,
                        Name = selectedDebt.Owner1.Name
                    };
                }
                NavigationService.Navigate(new Pages.AddEditDebtPage((sender as Button).DataContext as Debt));
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Pages.AddEditDebtPage(null));
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var debtForRemoving = ListViewDebt.SelectedItems.Cast<Debt>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить записи в количестве {debtForRemoving.Count()} элементов?",
                "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Entities.GetContext().Debts.RemoveRange(debtForRemoving);
                    Entities.GetContext().SaveChanges();
                    MessageBox.Show("Данные успешно удалены!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }
    }
}

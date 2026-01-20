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
    /// Логика взаимодействия для OtchetPage.xaml
    /// </summary>
    public partial class OtchetPage : Page
    {
        private Entities _context = new Entities();
        public OtchetPage()
        {
            InitializeComponent();
            var currentDebt = _context.OtchetPoOplates
                    .Include(d => d.Owner1)
                    .AsNoTracking()
                    .ToList();
            ListViewOtchet.ItemsSource = currentDebt;
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).DataContext is OtchetPoOplate selectedOtchet)
            {
                var detachedOtchet = new OtchetPoOplate
                {
                    PaymentId = selectedOtchet.PaymentId,
                    Adress = selectedOtchet.Adress,
                    Apartment = selectedOtchet.Apartment,
                    Owner = selectedOtchet.Owner,
                    Period = selectedOtchet.Period,
                    Accrued = selectedOtchet.Accrued,
                    Paid = selectedOtchet.Paid
                };
                // Если нужно передать владельца
                if (selectedOtchet.Owner1 != null)
                {
                    detachedOtchet.Owner1 = new Owner
                    {
                        Owner_id = selectedOtchet.Owner1.Owner_id,
                        Name = selectedOtchet.Owner1.Name
                    };
                }
                NavigationService.Navigate(new Pages.AddEditOtchetPage((sender as Button).DataContext as OtchetPoOplate));
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Pages.AddEditOtchetPage(null));
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var otchetForRemoving = ListViewOtchet.SelectedItems.Cast<OtchetPoOplate>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить записи в количестве {otchetForRemoving.Count()} элементов?",
                "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Entities.GetContext().OtchetPoOplates.RemoveRange(otchetForRemoving);
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

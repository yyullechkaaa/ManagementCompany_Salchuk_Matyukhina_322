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
    /// Логика взаимодействия для OwnerPage.xaml
    /// </summary>
    public partial class OwnerPage : Page
    {
        private Entities _context = new Entities();
        public OwnerPage()
        {
            InitializeComponent();
            var currentOwner = _context.Owners.ToList();
            ListViewOwners.ItemsSource = currentOwner;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var ownerForRemoving = ListViewOwners.SelectedItems.Cast<Owner>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить записи в количестве {ownerForRemoving.Count()} элементов?",
                "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Entities.GetContext().Owners.RemoveRange(ownerForRemoving);
                    Entities.GetContext().SaveChanges();
                    MessageBox.Show("Данные успешно удалены!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Pages.AddEditOwnerPage(null));
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).DataContext is Owner selectedOwner)
            {
                var detachedOwner = new Owner
                {
                    Owner_id = selectedOwner.Owner_id,
                    Name = selectedOwner.Name
                };

                NavigationService.Navigate(new Pages.AddEditOwnerPage((sender as Button).DataContext as Owner));
            }
        }
    }
}

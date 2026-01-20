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
    /// Логика взаимодействия для ApartPage.xaml
    /// </summary>
    public partial class ApartPage : Page
    {
        private Entities _context = new Entities();
        public ApartPage()
        {
            InitializeComponent();
            var currentApart = _context.Apartments.ToList();
            ListViewApart.ItemsSource = currentApart;
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).DataContext is Apartment selectedApart)
            {
                var detachedApart = new Apartment
                {
                    Apartment_id = selectedApart.Apartment_id,
                    Number = selectedApart.Number
                };
                
                NavigationService.Navigate(new Pages.AddEditApartPage((sender as Button).DataContext as Apartment));
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Pages.AddEditApartPage(null));
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var apartForRemoving = ListViewApart.SelectedItems.Cast<Apartment>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить записи в количестве {apartForRemoving.Count()} элементов?",
                "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Entities.GetContext().Apartments.RemoveRange(apartForRemoving);
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

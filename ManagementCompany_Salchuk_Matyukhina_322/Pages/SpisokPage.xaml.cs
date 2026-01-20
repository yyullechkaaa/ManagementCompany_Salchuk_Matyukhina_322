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
    /// Логика взаимодействия для SpisokPage.xaml
    /// </summary>
    public partial class SpisokPage : Page
    {
        public SpisokPage()
        {
            InitializeComponent();
            var currentFond = Entities.GetContext().SpisokJilogoFondas.ToList();
            ListViewFond.ItemsSource = currentFond;
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Pages.AddEditSpisokPage((sender as Button).DataContext as SpisokJilogoFonda));
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Pages.AddEditSpisokPage(null));
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var spisokForRemoving = ListViewFond.SelectedItems.Cast<SpisokJilogoFonda>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить записи в количестве {spisokForRemoving.Count()} элементов?",
                "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Entities.GetContext().SpisokJilogoFondas.RemoveRange(spisokForRemoving);
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

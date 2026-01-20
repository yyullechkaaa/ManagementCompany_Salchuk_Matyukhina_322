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
    /// Логика взаимодействия для DirectorPage.xaml
    /// </summary>
    public partial class DirectorPage : Page
    {
        public DirectorPage()
        {
            InitializeComponent();
        }

        private void SpisokButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Pages.SpisokPage());
        }

        private void DebtButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Pages.DebtPage());
        }

        private void OtchetButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Pages.OtchetPage());
        }

        private void ApartButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Pages.ApartPage());
        }

        private void OwnerButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Pages.OwnerPage());
        }
    }
}

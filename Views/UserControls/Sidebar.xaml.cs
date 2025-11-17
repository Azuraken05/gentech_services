using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace gentech_services.Views.UserControls
{
    /// <summary>
    /// Interaction logic for Sidebar.xaml
    /// </summary>
    public partial class Sidebar : UserControl
    {
        public Sidebar()
        {
            InitializeComponent();
        }
        

        private void IconButton_Loaded_1(object sender, RoutedEventArgs e)
        {

        }

        private void Dashboard_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Services_Click(object sender, RoutedEventArgs e)
        {
            ServicesSubButtons.Visibility = ServicesSubButtons.Visibility == Visibility.Visible
                                            ? Visibility.Collapsed : Visibility.Visible;
        }

        private void ServiceManagement_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ServiceOrders_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Products_Click(object sender, RoutedEventArgs e)
        {
            ProductsSubButtons.Visibility = ProductsSubButtons.Visibility == Visibility.Visible
                                            ? Visibility.Collapsed : Visibility.Visible;
        }

        private void InventoryManagement_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ProductOrders_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Users_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {

        }
    }

}

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
        public event EventHandler<string> NavigationRequested;

        public Sidebar()
        {
            InitializeComponent();
        }
        

        private void IconButton_Loaded_1(object sender, RoutedEventArgs e)
        {

        }

        private void Dashboard_Click(object sender, RoutedEventArgs e)
        {
            NavigationRequested?.Invoke(this, "Dashboard");
        }

        private void Services_Click(object sender, RoutedEventArgs e)
        {
            ServicesSubButtons.Visibility = ServicesSubButtons.Visibility == Visibility.Visible
                                            ? Visibility.Collapsed : Visibility.Visible;
        }

        private void ServiceManagement_Click(object sender, RoutedEventArgs e)
        {
            NavigationRequested?.Invoke(this, "ServiceManagement");
        }

        private void ServiceOrders_Click(object sender, RoutedEventArgs e)
        {
            NavigationRequested?.Invoke(this, "ServiceOrders");
        }

        private void Products_Click(object sender, RoutedEventArgs e)
        {
            ProductsSubButtons.Visibility = ProductsSubButtons.Visibility == Visibility.Visible
                                            ? Visibility.Collapsed : Visibility.Visible;
        }

        private void InventoryManagement_Click(object sender, RoutedEventArgs e)
        {
            NavigationRequested?.Invoke(this, "InventoryManagement");
        }

        private void InventoryLog_Click(object sender, RoutedEventArgs e)
        {
            NavigationRequested?.Invoke(this, "InventoryLog");
        }

        private void ProductOrders_Click(object sender, RoutedEventArgs e)
        {
            NavigationRequested?.Invoke(this, "ProductOrders");
        }

        private void ProductOrderHistory_Click(object sender, RoutedEventArgs e)
        {
            NavigationRequested?.Invoke(this, "ProductOrderHistory");
        }

        private void Users_Click(object sender, RoutedEventArgs e)
        {
            NavigationRequested?.Invoke(this, "UserManagement");
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            NavigationRequested?.Invoke(this, "Logout");
        }

        private void IconButton_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }

}

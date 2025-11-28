using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using gentech_services.Views.Pages;

namespace gentech_services
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Navigate to Service Management by default
            NavigateToPage("ServiceManagement");
        }

        private void Sidebar_NavigationRequested(object sender, string pageName)
        {
            NavigateToPage(pageName);
        }

        private void NavigateToPage(string pageName)
        {
            UserControl page = pageName switch
            {
                "Dashboard" => new DashboardPage(),
                "ServiceManagement" => new ServiceManagementPage(),
                "ServiceOrders" => new ServiceOrderPage(),
                "InventoryManagement" => new InventoryManagementPage(),
                "ProductOrders" => new ProductOrdersPage(),
                "UserManagement" => new UserManagementPage(),
                _ => new ServiceManagementPage() // Default page
            };

            if (pageName == "Logout")
            {
                MessageBox.Show("Logout functionality will be implemented.", "Logout", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            MainContentFrame.Content = page;
        }

        // Placeholder pages for navigation (these need to be created)
        private class DashboardPage : UserControl
        {
            public DashboardPage()
            {
                Content = new TextBlock
                {
                    Text = "Dashboard Page - To be implemented",
                    FontSize = 24,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
            }
        }


    }
}
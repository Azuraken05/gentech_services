using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using gentech_services.ViewsModels;

namespace gentech_services.Views.Pages
{
    public partial class InventoryManagementPage : UserControl
    {
        private InventoryManagementViewModel ViewModel => (InventoryManagementViewModel)DataContext;

        public InventoryManagementPage()
        {
            InitializeComponent();
            DataContext = new InventoryManagementViewModel();
        }

        private void ProductActionButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var product = button?.DataContext as ProductViewModel;

            if (product == null) return;

            // Store the selected product in a way that the popup menu can access it
            ProductActionPopup.Tag = product;
            ProductActionPopup.PlacementTarget = button;
            ProductActionPopup.IsOpen = true;
        }

        private void ViewProduct_Click(object sender, RoutedEventArgs e)
        {
            ProductActionPopup.IsOpen = false;
            var product = ProductActionPopup.Tag as ProductViewModel;
            if (product != null)
            {
                ViewModel.ViewProductCommand.Execute(product);
            }
        }

        private void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            ProductActionPopup.IsOpen = false;
            var product = ProductActionPopup.Tag as ProductViewModel;
            if (product != null)
            {
                ViewModel.EditProductCommand.Execute(product);
            }
        }
    }
}

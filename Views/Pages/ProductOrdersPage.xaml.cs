using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using gentech_services.ViewsModels;

namespace gentech_services.Views.Pages
{
    public partial class ProductOrdersPage : UserControl
    {
        private ProductOrdersViewModel ViewModel => (ProductOrdersViewModel)DataContext;

        public ProductOrdersPage()
        {
            InitializeComponent();
            DataContext = new ProductOrdersViewModel();
        }

        private void ProductCard_Click(object sender, MouseButtonEventArgs e)
        {
            var border = sender as Border;
            var product = border?.DataContext as ProductCardViewModel;

            if (product != null)
            {
                ViewModel.AddToCartCommand.Execute(product);
            }
        }
    }
}

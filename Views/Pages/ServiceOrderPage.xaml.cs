using gentech_services.ViewsModels;
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

namespace gentech_services.Views.Pages
{
    /// <summary>
    /// Interaction logic for ServiceOrderPage.xaml
    /// </summary>
    public partial class ServiceOrderPage : UserControl
    {
        private ServiceOrderViewModel viewModel;

        public ServiceOrderPage()
        {
            InitializeComponent();

            viewModel = new ServiceOrderViewModel();
            DataContext = viewModel;

            // Wire up the modal actions
            viewModel.ShowViewOrderModal = (order) =>
            {
                ViewOrderModal.ShowModal(order);
            };

            viewModel.ShowEditOrderModal = (order, services, technicians) =>
            {
                EditOrderModal.ShowModal(order, services, technicians);
            };

            // Wire up save changes callback
            EditOrderModal.OnSaveChanges = (updatedOrder) =>
            {
                // Force UI refresh by removing and re-adding the item
                var index = viewModel.ServiceOrders.IndexOf(updatedOrder);
                if (index >= 0)
                {
                    viewModel.ServiceOrders.RemoveAt(index);
                    viewModel.ServiceOrders.Insert(index, updatedOrder);
                }
            };
        }
    }
}

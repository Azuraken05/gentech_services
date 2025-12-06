using gentech_services.Models;
using ProductServicesManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace gentech_services.Views.UserControls
{
    public partial class SelectServiceModal : UserControl
    {
        public Action<Service> OnServiceSelected { get; set; }

        public SelectServiceModal()
        {
            InitializeComponent();
        }

        public void ShowModal(IEnumerable<Service> availableServices)
        {
            if (availableServices == null) return;

            ServicesItemsControl.ItemsSource = availableServices.ToList();
            ModalOverlay.Visibility = Visibility.Visible;
        }

        private void ServiceItem_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.Tag is Service selectedService)
            {
                OnServiceSelected?.Invoke(selectedService);
                CloseModal();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            CloseModal();
        }

        public void CloseModal()
        {
            ModalOverlay.Visibility = Visibility.Collapsed;
            ServicesItemsControl.ItemsSource = null;
        }
    }
}

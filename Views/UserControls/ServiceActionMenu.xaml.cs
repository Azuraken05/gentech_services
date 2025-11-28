using System;
using System.Windows;
using System.Windows.Controls;
using gentech_services.Models;
using ProductServicesManagementSystem.Models;

namespace gentech_services.Views.UserControls
{
    public partial class ServiceActionMenu : UserControl
    {
        public event EventHandler<Service> OnEdit;
        public event EventHandler<Service> OnToggleActive;
        public event EventHandler<Service> OnDelete;

        private Service currentService;

        public ServiceActionMenu()
        {
            InitializeComponent();
        }

        public void SetService(Service service)
        {
            currentService = service;

            // Update toggle button text based on service status
            if (service != null)
            {
                ToggleActiveButton.Content = service.IsActive ? "Deactivate" : "Activate";
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            OnEdit?.Invoke(this, currentService);
        }

        private void ToggleActiveButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentService != null)
            {
                OnToggleActive?.Invoke(this, currentService);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentService != null)
            {
                OnDelete?.Invoke(this, currentService);
            }
        }
    }
}

using gentech_services.Models;
using ProductServicesManagementSystem.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace gentech_services.Views.UserControls
{
    public partial class EditOrderModal : UserControl, INotifyPropertyChanged
    {
        private ServiceOrder currentOrder;
        private ObservableCollection<OrderServiceItem> orderServices;
        private ObservableCollection<Service> availableServices;
        private ObservableCollection<User> availableTechnicians;
        private User selectedTechnician;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<User> AvailableTechnicians
        {
            get { return availableTechnicians; }
            set { availableTechnicians = value; OnPropertyChanged(nameof(AvailableTechnicians)); }
        }

        public User SelectedTechnician
        {
            get { return selectedTechnician; }
            set { selectedTechnician = value; OnPropertyChanged(nameof(SelectedTechnician)); }
        }

        public Action<ServiceOrder> OnSaveChanges { get; set; }
        public Action<ServiceOrder> OnServiceAdded { get; set; }

        public EditOrderModal()
        {
            InitializeComponent();
            orderServices = new ObservableCollection<OrderServiceItem>();
            DataContext = this;

            // Subscribe to technician selection changes for auto-save
            TechnicianComboBox.SelectionChanged += TechnicianComboBox_SelectionChanged;

            // Wire up select service modal callback
            SelectServiceModalControl.OnServiceSelected = (selectedService) =>
            {
                AddServiceToOrder(selectedService);
            };
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ShowModal(ServiceOrder order, ObservableCollection<Service> availableServices, ObservableCollection<User> availableTechnicians)
        {
            if (order == null) return;

            currentOrder = order;
            this.availableServices = availableServices;
            AvailableTechnicians = availableTechnicians;

            // Set order ID
            OrderIdText.Text = $"#S{order.SaleID:000}";

            // Populate services table (for now just show the single service)
            orderServices.Clear();
            if (order.Service != null)
            {
                orderServices.Add(new OrderServiceItem
                {
                    Service = order.Service,
                    Status = order.Status,
                    Technician = order.Technician
                });
            }
            ServicesListView.ItemsSource = orderServices;

            // Update total cost
            UpdateTotalCost();

            // Set selected technician
            SelectedTechnician = order.Technician;

            // Show the modal
            ModalOverlay.Visibility = Visibility.Visible;
        }

        public void ShowModal(System.Collections.Generic.List<ServiceOrder> orders, ObservableCollection<Service> availableServices, ObservableCollection<User> availableTechnicians)
        {
            if (orders == null || orders.Count == 0) return;

            currentOrder = orders[0];
            this.availableServices = availableServices;
            AvailableTechnicians = availableTechnicians;

            // Set order ID
            OrderIdText.Text = $"#S{orders[0].SaleID:000}";

            // Populate services table with all services in the appointment
            orderServices.Clear();
            foreach (var order in orders)
            {
                if (order.Service != null)
                {
                    orderServices.Add(new OrderServiceItem
                    {
                        Service = order.Service,
                        Status = order.Status,
                        Technician = order.Technician,
                        ServiceOrder = order
                    });
                }
            }
            ServicesListView.ItemsSource = orderServices;

            // Update total cost
            UpdateTotalCost();

            // Set selected technician from first order
            SelectedTechnician = orders[0].Technician;

            // Show the modal
            ModalOverlay.Visibility = Visibility.Visible;
        }

        private void UpdateTotalCost()
        {
            decimal totalCost = orderServices.Sum(os => os.Service.Price);
            TotalCostRun.Text = $"₱ {totalCost:N2}";
        }

        private void StatusButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is OrderServiceItem serviceItem)
            {
                string newStatus = "";
                string confirmMessage = "";

                // Determine new status and confirmation message
                if (serviceItem.Status == "Pending")
                {
                    newStatus = "Ongoing";
                    confirmMessage = "Are you sure you want to set this service to Ongoing?";
                }
                else if (serviceItem.Status == "Ongoing")
                {
                    newStatus = "Completed";
                    confirmMessage = "Are you sure you want to mark this service as Completed?";
                }
                else
                {
                    return; // No action for Completed or Cancelled status
                }

                // Show confirmation dialog
                var result = MessageBox.Show(confirmMessage, "Confirm Status Change", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    serviceItem.Status = newStatus;

                    // Auto-save changes
                    AutoSaveChanges();
                }
            }
        }

        private void CancelService_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is OrderServiceItem serviceItem)
            {
                // Don't allow cancelling completed or already cancelled services
                if (serviceItem.Status == "Completed" || serviceItem.Status == "Cancelled")
                {
                    return;
                }

                var result = MessageBox.Show("Are you sure you want to cancel this service?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    serviceItem.Status = "Cancelled";
                    // Don't remove from list, just update status

                    // Auto-save changes
                    AutoSaveChanges();
                }
            }
        }

        private void TechnicianComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Only auto-save if we have a current order (not during initial load)
            if (currentOrder != null && SelectedTechnician != null)
            {
                AutoSaveChanges();
            }
        }

        private void AutoSaveChanges()
        {
            if (currentOrder == null) return;

            // Update technician and status for each service order
            foreach (var orderServiceItem in orderServices)
            {
                if (orderServiceItem.ServiceOrder != null)
                {
                    // Update technician if selected
                    if (SelectedTechnician != null && SelectedTechnician.Name != "All Technicians")
                    {
                        orderServiceItem.ServiceOrder.Technician = SelectedTechnician;
                        orderServiceItem.Technician = SelectedTechnician;
                    }

                    // Update status
                    orderServiceItem.ServiceOrder.Status = orderServiceItem.Status;
                }
            }

            // Notify parent with first order (for compatibility)
            OnSaveChanges?.Invoke(currentOrder);
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (currentOrder == null) return;

            // Update technician if selected
            if (SelectedTechnician != null && SelectedTechnician.Name != "All Technicians")
            {
                currentOrder.Technician = SelectedTechnician;

                // Update technician for all order service items
                foreach (var orderServiceItem in orderServices)
                {
                    orderServiceItem.Technician = SelectedTechnician;
                }
            }

            // Update order status from service items
            if (orderServices.Count > 0)
            {
                // Use the first service item's status as the order status
                currentOrder.Status = orderServices[0].Status;
            }

            // Notify parent
            OnSaveChanges?.Invoke(currentOrder);

            MessageBox.Show("Services updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            CloseModal();
        }

        private void AddService_Click(object sender, RoutedEventArgs e)
        {
            if (availableServices == null || !availableServices.Any())
            {
                MessageBox.Show("No services available to add.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Get services that are not already in the order
            var currentServiceIds = orderServices.Select(os => os.Service.ServiceID).ToList();
            var unselectedServices = availableServices.Where(s => !currentServiceIds.Contains(s.ServiceID)).ToList();

            if (!unselectedServices.Any())
            {
                MessageBox.Show("All available services are already in this order.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Show the select service modal
            SelectServiceModalControl.ShowModal(unselectedServices);
        }

        private void AddServiceToOrder(Service selectedService)
        {
            if (selectedService == null || currentOrder == null) return;

            // Create a new ServiceOrder for this service
            var newServiceOrder = new ServiceOrder
            {
                SaleID = currentOrder.SaleID,
                Service = selectedService,
                Technician = SelectedTechnician ?? currentOrder.Technician,
                Status = "Pending",
                AppointmentDate = currentOrder.AppointmentDate,
                Customer = currentOrder.Customer,
                PaymentMethod = currentOrder.PaymentMethod
            };

            // Add to the UI list
            orderServices.Add(new OrderServiceItem
            {
                Service = selectedService,
                Status = "Pending",
                Technician = SelectedTechnician ?? currentOrder.Technician,
                ServiceOrder = newServiceOrder
            });

            // Update total cost
            UpdateTotalCost();

            // Notify parent to add to actual data collections
            OnServiceAdded?.Invoke(newServiceOrder);

            MessageBox.Show($"Service '{selectedService.Name}' added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            CloseModal();
        }

        public void CloseModal()
        {
            ModalOverlay.Visibility = Visibility.Collapsed;
            currentOrder = null;
            orderServices.Clear();
        }
    }
}

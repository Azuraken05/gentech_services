using gentech_services.Models;
using ProductServicesManagementSystem.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace gentech_services.Views.UserControls
{
    public partial class EditOrderModal : UserControl
    {
        private ServiceOrder currentOrder;
        public Action<ServiceOrder> OnSaveChanges { get; set; }

        public EditOrderModal()
        {
            InitializeComponent();
        }

        public void ShowModal(ServiceOrder order, ObservableCollection<Service> availableServices, ObservableCollection<User> availableTechnicians)
        {
            if (order == null) return;

            currentOrder = order;

            // Set header info
            OrderIdText.Text = $"#S{order.SaleID:000}";

            // Populate services table (for now just show the single service)
            var serviceItems = new ObservableCollection<ServiceItem>();
            if (order.Service != null)
            {
                serviceItems.Add(new ServiceItem
                {
                    ServiceName = order.Service.Name,
                    Price = order.Service.Price,
                    Status = "Pending", // Default status
                    StatusButtonText = "Set to Ongoing",
                    StatusButtonBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFA500"))
                });
            }
            ServicesListView.ItemsSource = serviceItems;

            // Set total cost
            TotalCostRun.Text = $"Total cost: ₱ {order.Service?.Price:N2}";

            // Set issue description
            IssueDescriptionTextBox.Text = order.Service?.Description ?? "";

            // Show the modal
            ModalOverlay.Visibility = Visibility.Visible;
        }

        private void AddService_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Add Service functionality to be implemented", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void AssignTechnician_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Assign Technician functionality to be implemented", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void StatusButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is ServiceItem serviceItem)
            {
                // Toggle status
                if (serviceItem.Status == "Pending")
                {
                    serviceItem.Status = "Ongoing";
                    serviceItem.StatusButtonText = "Set to Completed";
                    serviceItem.StatusButtonBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00C206"));
                }
                else if (serviceItem.Status == "Ongoing")
                {
                    serviceItem.Status = "Completed";
                    serviceItem.StatusButtonText = "Completed";
                    serviceItem.StatusButtonBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#888888"));
                }
            }
        }

        private void CancelService_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is ServiceItem serviceItem)
            {
                var result = MessageBox.Show("Are you sure you want to cancel this service?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    // Remove service from list
                    var items = ServicesListView.ItemsSource as ObservableCollection<ServiceItem>;
                    items?.Remove(serviceItem);
                }
            }
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (currentOrder == null) return;

            // Update description
            if (currentOrder.Service != null)
            {
                currentOrder.Service.Description = IssueDescriptionTextBox.Text;
            }

            // Notify parent
            OnSaveChanges?.Invoke(currentOrder);

            MessageBox.Show("Changes saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            CloseModal();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            CloseModal();
        }

        public void CloseModal()
        {
            ModalOverlay.Visibility = Visibility.Collapsed;
            currentOrder = null;
        }
    }

    // Helper class for service items in the table
    public class ServiceItem : INotifyPropertyChanged
    {
        private string serviceName;
        private decimal price;
        private string status;
        private string statusButtonText;
        private Brush statusButtonBackground;

        public string ServiceName
        {
            get => serviceName;
            set { serviceName = value; OnPropertyChanged(nameof(ServiceName)); }
        }

        public decimal Price
        {
            get => price;
            set { price = value; OnPropertyChanged(nameof(Price)); }
        }

        public string Status
        {
            get => status;
            set { status = value; OnPropertyChanged(nameof(Status)); }
        }

        public string StatusButtonText
        {
            get => statusButtonText;
            set { statusButtonText = value; OnPropertyChanged(nameof(StatusButtonText)); }
        }

        public Brush StatusButtonBackground
        {
            get => statusButtonBackground;
            set { statusButtonBackground = value; OnPropertyChanged(nameof(StatusButtonBackground)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

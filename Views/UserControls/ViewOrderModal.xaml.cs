using gentech_services.Models;
using ProductServicesManagementSystem.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace gentech_services.Views.UserControls
{
    public partial class ViewOrderModal : UserControl
    {
        public ViewOrderModal()
        {
            InitializeComponent();
        }

        public void ShowModal(ServiceOrder order)
        {
            if (order == null) return;

            // Set order details
            OrderIdText.Text = $"#S{order.SaleID:000}";
            CreatedDateText.Text = $"Appointment: {order.AppointmentDate:MM/dd/yyyy}";

            // Set status badge
            StatusText.Text = order.Status;
            SetStatusBadgeColor(order.Status);

            // Customer details
            if (order.Customer != null)
            {
                CustomerNameText.Text = $"{order.Customer.FirstName} {order.Customer.LastName}";
                CustomerEmailText.Text = order.Customer.Email ?? "N/A";
                CustomerPhoneText.Text = order.Customer.Phone ?? "N/A";
            }

            // Service details
            if (order.Service != null)
            {
                ServiceTypeText.Text = order.Service.Name ?? "N/A";
                DeviceText.Text = order.Service.Category?.Name ?? "N/A";
                CostText.Text = $"₱ {order.Service.Price:N0}";
            }

            // Technician
            if (order.Technician != null)
            {
                TechnicianText.Text = order.Technician.Name ?? "Unassigned";
            }

            // Description (using issue description if available, otherwise service description)
            DescriptionText.Text = order.Service?.Description ?? "No description available";

            // Show the modal
            ModalOverlay.Visibility = Visibility.Visible;
        }

        private void SetStatusBadgeColor(string status)
        {
            switch (status?.ToLower())
            {
                case "completed":
                    StatusBadge.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E5F8E5"));
                    StatusText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#44AA44"));
                    break;
                case "pending":
                    StatusBadge.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF4E5"));
                    StatusText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF8800"));
                    break;
                case "cancelled":
                    StatusBadge.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F0F0F0"));
                    StatusText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#888888"));
                    break;
                case "ongoing":
                default:
                    StatusBadge.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE5E5"));
                    StatusText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF4444"));
                    break;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            ModalOverlay.Visibility = Visibility.Collapsed;
        }

        // Public method to close modal programmatically
        public void CloseModal()
        {
            ModalOverlay.Visibility = Visibility.Collapsed;
        }
    }
}

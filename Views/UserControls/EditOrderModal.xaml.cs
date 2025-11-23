using gentech_services.Models;
using ProductServicesManagementSystem.Models;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace gentech_services.Views.UserControls
{
    public partial class EditOrderModal : UserControl
    {
        private ServiceOrder currentOrder;
        private bool hasChanges = false;

        // Store original values to detect changes
        private string originalCustomerFirstName;
        private string originalCustomerLastName;
        private string originalCustomerEmail;
        private string originalCustomerPhone;
        private string originalDevice;
        private decimal originalCost;
        private Service originalService;
        private User originalTechnician;
        private DateTime originalAppointmentDate;

        public Action<ServiceOrder> OnSaveChanges { get; set; }

        public EditOrderModal()
        {
            InitializeComponent();

            // Track changes on all input fields
            CustomerFirstNameTextBox.TextChanged += (s, e) => CheckForChanges();
            CustomerLastNameTextBox.TextChanged += (s, e) => CheckForChanges();
            CustomerEmailTextBox.TextChanged += (s, e) => CheckForChanges();
            CustomerPhoneTextBox.TextChanged += (s, e) => CheckForChanges();
            DeviceTextBox.TextChanged += (s, e) => CheckForChanges();
            CostTextBox.TextChanged += (s, e) => CheckForChanges();
            ServiceTypeComboBox.SelectionChanged += (s, e) => CheckForChanges();
            TechnicianComboBox.SelectionChanged += (s, e) => CheckForChanges();
            AppointmentDatePicker.SelectedDateChanged += (s, e) => CheckForChanges();
        }

        public void ShowModal(ServiceOrder order, ObservableCollection<Service> availableServices, ObservableCollection<User> availableTechnicians)
        {
            if (order == null) return;

            currentOrder = order;
            hasChanges = false;

            // Set header info
            OrderIdText.Text = $"#S{order.SaleID:000}";
            CreatedDateText.Text = $"Appointment: {order.AppointmentDate:MM/dd/yyyy}";

            // Set status badge
            StatusText.Text = order.Status;
            SetStatusBadgeColor(order.Status);

            // Store original values
            if (order.Customer != null)
            {
                originalCustomerFirstName = order.Customer.FirstName ?? "";
                originalCustomerLastName = order.Customer.LastName ?? "";
                originalCustomerEmail = order.Customer.Email ?? "";
                originalCustomerPhone = order.Customer.Phone ?? "";

                CustomerFirstNameTextBox.Text = originalCustomerFirstName;
                CustomerLastNameTextBox.Text = originalCustomerLastName;
                CustomerEmailTextBox.Text = originalCustomerEmail;
                CustomerPhoneTextBox.Text = originalCustomerPhone;
            }

            if (order.Service != null)
            {
                originalDevice = order.Service.Category?.Name ?? "";
                originalCost = order.Service.Price;
                originalService = order.Service;

                DeviceTextBox.Text = originalDevice;
                CostTextBox.Text = originalCost.ToString();

                // Populate services
                ServiceTypeComboBox.ItemsSource = availableServices;
                ServiceTypeComboBox.SelectedItem = order.Service;
            }

            // Populate technicians
            originalTechnician = order.Technician;
            if (availableTechnicians != null)
            {
                TechnicianComboBox.ItemsSource = availableTechnicians;
                TechnicianComboBox.DisplayMemberPath = "Name";
                TechnicianComboBox.SelectedItem = order.Technician;
            }

            // Set appointment date
            originalAppointmentDate = order.AppointmentDate;
            AppointmentDatePicker.SelectedDate = order.AppointmentDate;

            // Reset change tracking
            hasChanges = false;

            // Show the modal
            ModalOverlay.Visibility = Visibility.Visible;
        }

        private void CheckForChanges()
        {
            if (currentOrder == null) return;

            hasChanges = CustomerFirstNameTextBox.Text != originalCustomerFirstName ||
                        CustomerLastNameTextBox.Text != originalCustomerLastName ||
                        CustomerEmailTextBox.Text != originalCustomerEmail ||
                        CustomerPhoneTextBox.Text != originalCustomerPhone ||
                        DeviceTextBox.Text != originalDevice ||
                        (decimal.TryParse(CostTextBox.Text, out decimal newCost) && newCost != originalCost) ||
                        ServiceTypeComboBox.SelectedItem != originalService ||
                        TechnicianComboBox.SelectedItem != originalTechnician ||
                        (AppointmentDatePicker.SelectedDate.HasValue && AppointmentDatePicker.SelectedDate.Value.Date != originalAppointmentDate.Date);
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
            if (hasChanges)
            {
                var result = MessageBox.Show(
                    "You have unsaved changes. Do you want to save them?",
                    "Save Changes",
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // Try to save - if validation fails, don't close
                    if (!ValidateFields())
                    {
                        return; // Don't close if validation fails
                    }
                    SaveChanges();
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    return; // Don't close
                }
            }

            CloseModal();
        }

        private void SaveChanges()
        {
            if (currentOrder == null) return;

            // Update customer info
            if (currentOrder.Customer != null)
            {
                currentOrder.Customer.FirstName = CustomerFirstNameTextBox.Text.Trim();
                currentOrder.Customer.LastName = CustomerLastNameTextBox.Text.Trim();
                currentOrder.Customer.Email = CustomerEmailTextBox.Text.Trim();
                currentOrder.Customer.Phone = CustomerPhoneTextBox.Text.Trim();
            }

            // Update service
            if (ServiceTypeComboBox.SelectedItem is Service selectedService)
            {
                currentOrder.Service = selectedService;
            }

            // Update technician (can be null if unassigned)
            if (TechnicianComboBox.SelectedItem is User selectedTechnician)
            {
                currentOrder.Technician = selectedTechnician;
            }
            else
            {
                // Set to unassigned if no technician selected
                currentOrder.Technician = new User { Name = "Unassigned" };
            }

            // Update cost
            if (decimal.TryParse(CostTextBox.Text, out decimal newCost) && currentOrder.Service != null)
            {
                currentOrder.Service.Price = newCost;
            }

            // Update appointment date
            if (AppointmentDatePicker.SelectedDate.HasValue)
            {
                currentOrder.AppointmentDate = AppointmentDatePicker.SelectedDate.Value;
            }

            // Notify parent
            OnSaveChanges?.Invoke(currentOrder);

            MessageBox.Show("Changes saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            hasChanges = false;
        }

        private bool ValidateFields()
        {
            // Validate First Name
            if (string.IsNullOrWhiteSpace(CustomerFirstNameTextBox.Text))
            {
                MessageBox.Show("First name is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                CustomerFirstNameTextBox.Focus();
                return false;
            }

            // Validate Last Name
            if (string.IsNullOrWhiteSpace(CustomerLastNameTextBox.Text))
            {
                MessageBox.Show("Last name is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                CustomerLastNameTextBox.Focus();
                return false;
            }

            // Validate Email
            if (string.IsNullOrWhiteSpace(CustomerEmailTextBox.Text))
            {
                MessageBox.Show("Email is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                CustomerEmailTextBox.Focus();
                return false;
            }

            if (!IsValidEmail(CustomerEmailTextBox.Text))
            {
                MessageBox.Show("Please enter a valid email address.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                CustomerEmailTextBox.Focus();
                return false;
            }

            // Validate Phone
            if (string.IsNullOrWhiteSpace(CustomerPhoneTextBox.Text))
            {
                MessageBox.Show("Phone number is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                CustomerPhoneTextBox.Focus();
                return false;
            }

            if (!IsValidPhone(CustomerPhoneTextBox.Text))
            {
                MessageBox.Show("Please enter a valid phone number (10-11 digits).", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                CustomerPhoneTextBox.Focus();
                return false;
            }

            // Validate Service
            if (ServiceTypeComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a service type.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                ServiceTypeComboBox.Focus();
                return false;
            }

            // Technician is optional - no validation needed

            // Validate Cost
            if (string.IsNullOrWhiteSpace(CostTextBox.Text))
            {
                MessageBox.Show("Cost is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                CostTextBox.Focus();
                return false;
            }

            if (!decimal.TryParse(CostTextBox.Text, out decimal cost) || cost <= 0)
            {
                MessageBox.Show("Please enter a valid cost greater than zero.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                CostTextBox.Focus();
                return false;
            }

            // Validate Appointment Date
            if (!AppointmentDatePicker.SelectedDate.HasValue)
            {
                MessageBox.Show("Appointment date is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                AppointmentDatePicker.Focus();
                return false;
            }

            if (AppointmentDatePicker.SelectedDate.Value.Date < DateTime.Now.Date)
            {
                MessageBox.Show("Appointment date cannot be in the past.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                AppointmentDatePicker.Focus();
                return false;
            }

            return true;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhone(string phone)
        {
            // Remove common formatting characters
            string cleaned = phone.Replace("-", "").Replace(" ", "").Replace("(", "").Replace(")", "");

            // Check if it's all digits and has 10-11 digits (Philippine format)
            return cleaned.Length >= 10 && cleaned.Length <= 11 && cleaned.All(char.IsDigit);
        }

        public void CloseModal()
        {
            ModalOverlay.Visibility = Visibility.Collapsed;
            hasChanges = false;
            currentOrder = null;
        }
    }
}

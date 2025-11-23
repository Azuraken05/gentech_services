using gentech_services.Models;
using gentech_services.MVVM;
using ProductServicesManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace gentech_services.ViewsModels
{
    internal class ServiceOrderViewModel : ViewModelBase
    {
        private Appointment currentAppointment;
        private ObservableCollection<ServiceOrder> serviceOrders;
        private ObservableCollection<ServiceOrder> allServiceOrders; // Store unfiltered list
        private ServiceOrder selectedOrder;
        private ObservableCollection<User> availableTechnicians;

        // Action to trigger modal display from View
        public Action<ServiceOrder> ShowViewOrderModal { get; set; }
        public Action<ServiceOrder, ObservableCollection<Service>, ObservableCollection<User>> ShowEditOrderModal { get; set; }

        // Form input properties
        private string firstName;
        private string lastName;
        private string email;
        private string phone;
        private Service selectedService;
        private DateTime? selectedDate;
        private string issueDescription;
        private ObservableCollection<Service> availableServices;

        // Validation error properties
        private string firstNameError;
        private string lastNameError;
        private string emailError;
        private string phoneError;
        private string serviceError;
        private string dateError;

        // Filter properties
        private string selectedStatusFilter;
        private User selectedTechnicianFilter;
        private ObservableCollection<string> statusFilters;

        public ObservableCollection<ServiceOrder> ServiceOrders {
            get { return serviceOrders; } }

        public ServiceOrder SelectedOrder
        {
            get { return selectedOrder; }
            set { selectedOrder = value; OnPropertyChanged(); }
        }

        public Appointment CurrentAppointment
        {
            get { return currentAppointment; }
            set { currentAppointment = value; OnPropertyChanged(); }
        }

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; OnPropertyChanged(); }
        }

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; OnPropertyChanged(); }
        }

        public string Email
        {
            get { return email; }
            set { email = value; OnPropertyChanged(); }
        }

        public string Phone
        {
            get { return phone; }
            set { phone = value; OnPropertyChanged(); }
        }

        public Service SelectedService
        {
            get { return selectedService; }
            set { selectedService = value; OnPropertyChanged(); }
        }

        public DateTime? SelectedDate
        {
            get { return selectedDate; }
            set { selectedDate = value; OnPropertyChanged(); }
        }

        public string IssueDescription
        {
            get { return issueDescription; }
            set { issueDescription = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Service> AvailableServices
        {
            get { return availableServices; }
            set { availableServices = value; OnPropertyChanged(); }
        }

        public ObservableCollection<User> AvailableTechnicians
        {
            get { return availableTechnicians; }
            set { availableTechnicians = value; OnPropertyChanged(); }
        }

        public string FirstNameError
        {
            get { return firstNameError; }
            set { firstNameError = value; OnPropertyChanged(); }
        }

        public string LastNameError
        {
            get { return lastNameError; }
            set { lastNameError = value; OnPropertyChanged(); }
        }

        public string EmailError
        {
            get { return emailError; }
            set { emailError = value; OnPropertyChanged(); }
        }

        public string PhoneError
        {
            get { return phoneError; }
            set { phoneError = value; OnPropertyChanged(); }
        }

        public string ServiceError
        {
            get { return serviceError; }
            set { serviceError = value; OnPropertyChanged(); }
        }

        public string DateError
        {
            get { return dateError; }
            set { dateError = value; OnPropertyChanged(); }
        }

        public ObservableCollection<string> StatusFilters
        {
            get { return statusFilters; }
            set { statusFilters = value; OnPropertyChanged(); }
        }

        public string SelectedStatusFilter
        {
            get { return selectedStatusFilter; }
            set
            {
                selectedStatusFilter = value;
                OnPropertyChanged();
                ApplyFilters();
            }
        }

        public User SelectedTechnicianFilter
        {
            get { return selectedTechnicianFilter; }
            set
            {
                selectedTechnicianFilter = value;
                OnPropertyChanged();
                ApplyFilters();
            }
        }


        public RelayCommand ViewDetailsCommand { get; private set; }
        public RelayCommand EditCommand { get; private set; }
        public RelayCommand SetToOngoingCommand { get; private set; }
        public RelayCommand SetToCompletedCommand { get; private set; }
        public RelayCommand CancelAppointmentCommand { get; private set; }
        public RelayCommand RescheduleCommand { get; private set; }
        public RelayCommand DeleteCommand { get; private set; }
        public RelayCommand SetAppointmentCommand { get; private set; }
        public RelayCommand ClearFormCommand { get; private set; }
        public ServiceOrderViewModel()
        {
            currentAppointment = new Appointment();
            serviceOrders = new ObservableCollection<ServiceOrder>();
            allServiceOrders = new ObservableCollection<ServiceOrder>();

            // Initialize filter collections
            statusFilters = new ObservableCollection<string>
            {
                "All Statuses",
                "Pending",
                "Ongoing",
                "Completed",
                "Cancelled"
            };
            selectedStatusFilter = "All Statuses";

            ViewDetailsCommand = new RelayCommand(obj => ViewDetails(obj as ServiceOrder));
            EditCommand = new RelayCommand(obj => Edit(obj as ServiceOrder));
            SetToOngoingCommand = new RelayCommand(obj => SetToOngoing(obj as ServiceOrder));
            SetToCompletedCommand = new RelayCommand(obj => SetToCompleted(obj as ServiceOrder));
            CancelAppointmentCommand = new RelayCommand(obj => CancelAppointment(obj as ServiceOrder));
            RescheduleCommand = new RelayCommand(obj => Reschedule(obj as ServiceOrder));
            DeleteCommand = new RelayCommand(obj => Delete(obj as ServiceOrder));
            SetAppointmentCommand = new RelayCommand(obj => SetAppointment(), obj => CanSetAppointment());
            ClearFormCommand = new RelayCommand(obj => ClearForm());

            LoadData();
        }
        private void ViewDetails(ServiceOrder order)
        {
            if (order != null)
            {
                SelectedOrder = order;
                ShowViewOrderModal?.Invoke(order);
            }
        }

        private void Edit(ServiceOrder order)
        {
            if (order != null)
            {
                SelectedOrder = order;
                ShowEditOrderModal?.Invoke(order, availableServices, availableTechnicians);
            }
        }

        private void CancelAppointment(ServiceOrder order)
        {
            if (order != null)
            {
                // Don't allow cancelling completed orders
                if (order.Status == "Completed")
                {
                    MessageBox.Show(
                        "Cannot cancel a completed order.",
                        "Cancel Not Allowed",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    return;
                }

                var result = MessageBox.Show(
                    $"Are you sure you want to cancel the appointment for {order.Customer.FirstName} {order.Customer.LastName}?\n\n" +
                    $"Order ID: #S{order.SaleID:000}\n" +
                    $"Service: {order.Service.Name}\n" +
                    $"Date: {order.AppointmentDate:dd/MM/yyyy}",
                    "Cancel Appointment",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    order.Status = "Cancelled";

                    // Force UI refresh
                    var index = serviceOrders.IndexOf(order);
                    if (index >= 0)
                    {
                        serviceOrders.RemoveAt(index);
                        serviceOrders.Insert(index, order);
                    }

                    MessageBox.Show(
                        "Appointment cancelled successfully.",
                        "Success",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
            }
        }

        private void SetToOngoing(ServiceOrder order)
        {
            if (order != null)
            {
                if (order.Status == "Ongoing")
                {
                    MessageBox.Show(
                        "This order is already ongoing.",
                        "Already Ongoing",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    return;
                }

                if (order.Status == "Completed")
                {
                    MessageBox.Show(
                        "Cannot change status of a completed order.",
                        "Action Not Allowed",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    return;
                }

                if (order.Status == "Cancelled")
                {
                    MessageBox.Show(
                        "Cannot change status of a cancelled order.",
                        "Action Not Allowed",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    return;
                }

                var result = MessageBox.Show(
                    $"Start working on this order?\n\n" +
                    $"Order ID: #S{order.SaleID:000}\n" +
                    $"Customer: {order.Customer.FirstName} {order.Customer.LastName}\n" +
                    $"Service: {order.Service.Name}",
                    "Set to Ongoing",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    order.Status = "Ongoing";

                    MessageBox.Show(
                        "Order status updated to Ongoing.",
                        "Success",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
            }
        }

        private void SetToCompleted(ServiceOrder order)
        {
            if (order != null)
            {
                if (order.Status == "Completed")
                {
                    MessageBox.Show(
                        "This order is already completed.",
                        "Already Completed",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    return;
                }

                if (order.Status == "Pending")
                {
                    MessageBox.Show(
                        "Cannot complete a pending order. Please set it to Ongoing first.",
                        "Action Not Allowed",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    return;
                }

                if (order.Status == "Cancelled")
                {
                    MessageBox.Show(
                        "Cannot mark a cancelled order as completed.",
                        "Action Not Allowed",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    return;
                }

                var result = MessageBox.Show(
                    $"Mark this order as completed?\n\n" +
                    $"Order ID: #S{order.SaleID:000}\n" +
                    $"Customer: {order.Customer.FirstName} {order.Customer.LastName}\n" +
                    $"Service: {order.Service.Name}",
                    "Set to Completed",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    order.Status = "Completed";

                    MessageBox.Show(
                        "Order marked as completed successfully.",
                        "Success",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
            }
        }

        private void Reschedule(ServiceOrder order)
        {
            if (order != null)
            {
                if (order.Status == "Completed")
                {
                    MessageBox.Show(
                        "Cannot reschedule a completed order.",
                        "Action Not Allowed",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    return;
                }

                if (order.Status == "Cancelled")
                {
                    MessageBox.Show(
                        "Cannot reschedule a cancelled order.",
                        "Action Not Allowed",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    return;
                }

                // For now, show a simple date picker dialog using MessageBox
                // In a real implementation, you'd want a custom dialog with a DatePicker
                MessageBox.Show(
                    $"Reschedule feature coming soon.\n\n" +
                    $"Order ID: #S{order.SaleID:000}\n" +
                    $"Current Date: {order.AppointmentDate:dd/MM/yyyy}\n\n" +
                    $"Please use the Edit option to change the appointment date.",
                    "Reschedule Order",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

        private void Delete(ServiceOrder order)
        {
            if (order != null)
            {
                var result = MessageBox.Show(
                    $"Are you sure you want to delete this order? This action cannot be undone.\n\n" +
                    $"Order ID: #S{order.SaleID:000}\n" +
                    $"Customer: {order.Customer.FirstName} {order.Customer.LastName}\n" +
                    $"Service: {order.Service.Name}",
                    "Delete Order",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    allServiceOrders.Remove(order);
                    serviceOrders.Remove(order);
                    MessageBox.Show(
                        "Order deleted successfully.",
                        "Success",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
            }
        }

        private void ApplyFilters()
        {
            IEnumerable<ServiceOrder> filteredOrders = allServiceOrders;

            // Filter by status
            if (!string.IsNullOrEmpty(SelectedStatusFilter) && SelectedStatusFilter != "All Statuses")
            {
                filteredOrders = filteredOrders.Where(o => o.Status == SelectedStatusFilter);
            }

            // Filter by technician
            if (SelectedTechnicianFilter != null && SelectedTechnicianFilter.Name != "All Technicians")
            {
                filteredOrders = filteredOrders.Where(o => o.Technician?.Name == SelectedTechnicianFilter.Name);
            }

            // Update the visible collection
            serviceOrders.Clear();
            foreach (var order in filteredOrders)
            {
                serviceOrders.Add(order);
            }
        }

        private bool ValidateForm()
        {
            bool isValid = true;

            // Clear previous errors
            FirstNameError = string.Empty;
            LastNameError = string.Empty;
            EmailError = string.Empty;
            PhoneError = string.Empty;
            ServiceError = string.Empty;
            DateError = string.Empty;

            // Validate First Name
            if (string.IsNullOrWhiteSpace(FirstName))
            {
                FirstNameError = "First name is required";
                isValid = false;
            }

            // Validate Last Name
            if (string.IsNullOrWhiteSpace(LastName))
            {
                LastNameError = "Last name is required";
                isValid = false;
            }

            // Validate Email
            if (string.IsNullOrWhiteSpace(Email))
            {
                EmailError = "Email is required";
                isValid = false;
            }
            else if (!IsValidEmail(Email))
            {
                EmailError = "Invalid email format";
                isValid = false;
            }

            // Validate Phone
            if (string.IsNullOrWhiteSpace(Phone))
            {
                PhoneError = "Phone number is required";
                isValid = false;
            }
            else if (!IsValidPhone(Phone))
            {
                PhoneError = "Invalid phone number format";
                isValid = false;
            }

            // Validate Service
            if (SelectedService == null)
            {
                ServiceError = "Please select a service";
                isValid = false;
            }

            // Validate Date
            if (!SelectedDate.HasValue)
            {
                DateError = "Date is required";
                isValid = false;
            }
            else if (SelectedDate.Value.Date < DateTime.Now.Date)
            {
                DateError = "Date cannot be in the past";
                isValid = false;
            }

            return isValid;
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

        private bool CanSetAppointment()
        {
            return !string.IsNullOrWhiteSpace(FirstName) &&
                   !string.IsNullOrWhiteSpace(LastName) &&
                   !string.IsNullOrWhiteSpace(Email) &&
                   !string.IsNullOrWhiteSpace(Phone) &&
                   SelectedService != null &&
                   SelectedDate.HasValue;
        }

        private void SetAppointment()
        {
            if (!ValidateForm())
            {
                return;
            }

            // Create new customer
            var customer = new Customer
            {
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                Phone = Phone,
                CreatedAt = DateTime.Now
            };

            // Create new service order
            var serviceOrder = new ServiceOrder
            {
                SaleID = serviceOrders.Count + 1, // Simple ID generation
                Customer = customer,
                Service = SelectedService,
                AppointmentDate = SelectedDate.Value,
                Status = "Pending",
                PaymentMethod = "Not Set",
                Technician = new User { Name = "Unassigned" } // Default technician
            };

            // Add to both collections
            allServiceOrders.Add(serviceOrder);
            serviceOrders.Add(serviceOrder);

            // Show success message
            MessageBox.Show($"Appointment created successfully!\n\n" +
                           $"Customer: {customer.FirstName} {customer.LastName}\n" +
                           $"Service: {SelectedService.Name}\n" +
                           $"Date: {SelectedDate.Value:dd/MM/yyyy}",
                           "Success",
                           MessageBoxButton.OK,
                           MessageBoxImage.Information);

            // Clear form
            ClearForm();
        }

        private void ClearForm()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
            Phone = string.Empty;
            SelectedService = null;
            SelectedDate = null;
            IssueDescription = string.Empty;

            // Clear errors
            FirstNameError = string.Empty;
            LastNameError = string.Empty;
            EmailError = string.Empty;
            PhoneError = string.Empty;
            ServiceError = string.Empty;
            DateError = string.Empty;
        }

        private void LoadData()
        {
            User technician1 = new User{
                Name = "Keith",
                UserID = 1,
                Email = "keith@gmail.com",
                PasswordHash = "password123",
                Role="Technician",
                CreatedAt = DateTime.Now,
            };

            Service service1 = new Service {
                ServiceID = 1,
                Name = "Motherboard Repair",
                Description = "Lorem Ipsum dolor set",
                Price = 1500,
                CreatedAt = DateTime.Now,
                IsActive = true,
                CategoryID = 1,
                Category = new Category
                {
                    CategoryID = 1,
                    Name = "Repair",
                    Type = "Repair"
                }
            };

            Service service2 = new Service
            {
                ServiceID = 2,
                Name = "RAM Upgrade",
                Description = "Upgrade system RAM",
                Price = 2000,
                CreatedAt = DateTime.Now,
                IsActive = true,
                CategoryID = 2,
                Category = new Category
                {
                    CategoryID = 2,
                    Name = "Upgrade",
                    Type = "Upgrade"
                }
            };

            Service service3 = new Service
            {
                ServiceID = 3,
                Name = "Virus Removal",
                Description = "Remove malware and viruses",
                Price = 800,
                CreatedAt = DateTime.Now,
                IsActive = true,
                CategoryID = 3,
                Category = new Category
                {
                    CategoryID = 3,
                    Name = "Maintenance",
                    Type = "Maintenance"
                }
            };

            // Create additional technicians
            User technician2 = new User
            {
                Name = "M. Soriano",
                UserID = 2,
                Email = "msoriano@gmail.com",
                PasswordHash = "password123",
                Role = "Technician",
                CreatedAt = DateTime.Now,
            };

            User technician3 = new User
            {
                Name = "J. Reyes",
                UserID = 3,
                Email = "jreyes@gmail.com",
                PasswordHash = "password123",
                Role = "Technician",
                CreatedAt = DateTime.Now,
            };

            // Populate available services
            availableServices = new ObservableCollection<Service>
            {
                service1,
                service2,
                service3
            };
            OnPropertyChanged(nameof(AvailableServices));

            // Populate available technicians with "All Technicians" option
            availableTechnicians = new ObservableCollection<User>
            {
                new User { Name = "All Technicians" }, // Filter option
                technician1,
                technician2,
                technician3
            };
            selectedTechnicianFilter = availableTechnicians[0]; // Set default to "All Technicians"
            OnPropertyChanged(nameof(AvailableTechnicians));

            // Create sample data - 25 orders with various statuses
            var random = new Random();
            var statuses = new[] { "Pending", "Ongoing", "Completed", "Cancelled" };
            var firstNames = new[] { "Nicole", "John", "Maria", "David", "Sarah", "Michael", "Emma", "James", "Olivia", "William" };
            var lastNames = new[] { "Juntilla", "Smith", "Garcia", "Johnson", "Williams", "Brown", "Jones", "Miller", "Davis", "Martinez" };
            var technicians = new[] { technician1, technician2, technician3 };
            var services = new[] { service1, service2, service3 };
            var paymentMethods = new[] { "Cash", "Credit Card", "GCash", "PayPal", "Bank Transfer" };

            for (int i = 1; i <= 25; i++)
            {
                var firstName = firstNames[random.Next(firstNames.Length)];
                var lastName = lastNames[random.Next(lastNames.Length)];
                var status = statuses[random.Next(statuses.Length)];
                var daysOffset = random.Next(-10, 30);

                var order = new ServiceOrder
                {
                    SaleID = i,
                    AppointmentDate = DateTime.Now.AddDays(daysOffset),
                    Status = status,
                    Technician = status == "Pending" && random.Next(3) == 0
                        ? new User { Name = "Unassigned" }
                        : technicians[random.Next(technicians.Length)],
                    Service = services[random.Next(services.Length)],
                    PaymentMethod = paymentMethods[random.Next(paymentMethods.Length)],
                    Customer = new Customer
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        Email = $"{firstName.ToLower()}.{lastName.ToLower()}@email.com",
                        Phone = $"09{random.Next(100000000, 999999999)}",
                        CreatedAt = DateTime.Now.AddDays(-random.Next(1, 60))
                    }
                };

                allServiceOrders.Add(order);
                serviceOrders.Add(order);
            }
        }
    }
}

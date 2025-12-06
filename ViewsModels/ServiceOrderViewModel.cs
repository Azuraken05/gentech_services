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
        public Action<ServiceOrder> ShowEditAppointmentModal { get; set; }

        private string customerName;
        private string email;
        private string phone;
        private Service selectedService;
        private DateTime? selectedDate;
        private string issueDescription;
        private ObservableCollection<SelectableService> selectableServices;
        private ObservableCollection<OrderServiceItem> orderServices;
        private ObservableCollection<Service> availableServicesToAdd;
        private bool isServiceSelectorVisible;
        private User selectedTechnician;

        private string customerNameError;
        private string emailError;
        private string phoneError;
        private string serviceError;
        private string dateError;

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

        public string CustomerName
        {
            get { return customerName; }
            set { customerName = value; OnPropertyChanged(); }
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

        public ObservableCollection<SelectableService> SelectableServices
        {
            get { return selectableServices; }
            set { selectableServices = value; OnPropertyChanged(); }
        }

        public ObservableCollection<OrderServiceItem> OrderServices
        {
            get { return orderServices; }
            set { orderServices = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Service> AvailableServicesToAdd
        {
            get { return availableServicesToAdd; }
            set { availableServicesToAdd = value; OnPropertyChanged(); }
        }

        public bool IsServiceSelectorVisible
        {
            get { return isServiceSelectorVisible; }
            set { isServiceSelectorVisible = value; OnPropertyChanged(); }
        }

        public ObservableCollection<User> AvailableTechnicians
        {
            get { return availableTechnicians; }
            set { availableTechnicians = value; OnPropertyChanged(); }
        }

        public User SelectedTechnician
        {
            get { return selectedTechnician; }
            set { selectedTechnician = value; OnPropertyChanged(); }
        }

        public string CustomerNameError
        {
            get { return customerNameError; }
            set { customerNameError = value; OnPropertyChanged(); }
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
        public RelayCommand EditAppointmentCommand { get; private set; }
        public RelayCommand SetToOngoingCommand { get; private set; }
        public RelayCommand SetToCompletedCommand { get; private set; }
        public RelayCommand CancelAppointmentCommand { get; private set; }
        public RelayCommand RescheduleCommand { get; private set; }
        public RelayCommand DeleteCommand { get; private set; }
        public RelayCommand SetAppointmentCommand { get; private set; }
        public RelayCommand ClearFormCommand { get; private set; }
        public RelayCommand ShowServiceSelectorCommand { get; private set; }
        public RelayCommand AddServiceToOrderCommand { get; private set; }
        public RelayCommand RemoveServiceFromOrderCommand { get; private set; }
        public ServiceOrderViewModel()
        {
            currentAppointment = new Appointment();
            serviceOrders = new ObservableCollection<ServiceOrder>();
            allServiceOrders = new ObservableCollection<ServiceOrder>();
            orderServices = new ObservableCollection<OrderServiceItem>();
            availableServicesToAdd = new ObservableCollection<Service>();
            selectableServices = new ObservableCollection<SelectableService>();

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
            EditAppointmentCommand = new RelayCommand(obj => EditAppointment(obj as ServiceOrder));
            SetToOngoingCommand = new RelayCommand(obj => SetToOngoing(obj as ServiceOrder));
            SetToCompletedCommand = new RelayCommand(obj => SetToCompleted(obj as ServiceOrder));
            CancelAppointmentCommand = new RelayCommand(obj => CancelAppointment(obj as ServiceOrder));
            RescheduleCommand = new RelayCommand(obj => Reschedule(obj as ServiceOrder));
            DeleteCommand = new RelayCommand(obj => Delete(obj as ServiceOrder));
            SetAppointmentCommand = new RelayCommand(obj => SetAppointment(), obj => CanSetAppointment());
            ClearFormCommand = new RelayCommand(obj => ClearForm());
            ShowServiceSelectorCommand = new RelayCommand(obj => ShowServiceSelector());
            AddServiceToOrderCommand = new RelayCommand(obj => AddServiceToOrder(obj as Service));
            RemoveServiceFromOrderCommand = new RelayCommand(obj => RemoveServiceFromOrder(obj as OrderServiceItem));

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
                var services = new ObservableCollection<Service>(selectableServices.Select(s => s.Service));
                ShowEditOrderModal?.Invoke(order, services, availableTechnicians);
            }
        }

        private void EditAppointment(ServiceOrder order)
        {
            if (order != null)
            {
                SelectedOrder = order;
                ShowEditAppointmentModal?.Invoke(order);
            }
        }

        private void CancelAppointment(ServiceOrder order)
        {
            if (order != null)
            {
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

            if (!string.IsNullOrEmpty(SelectedStatusFilter) && SelectedStatusFilter != "All Statuses")
            {
                filteredOrders = filteredOrders.Where(o => o.Status == SelectedStatusFilter);
            }

            if (SelectedTechnicianFilter != null && SelectedTechnicianFilter.Name != "All Technicians")
            {
                filteredOrders = filteredOrders.Where(o => o.Technician?.Name == SelectedTechnicianFilter.Name);
            }

            serviceOrders.Clear();
            foreach (var order in filteredOrders)
            {
                serviceOrders.Add(order);
            }
        }

        private bool ValidateForm()
        {
            bool isValid = true;

            CustomerNameError = string.Empty;
            EmailError = string.Empty;
            PhoneError = string.Empty;
            ServiceError = string.Empty;
            DateError = string.Empty;

            if (string.IsNullOrWhiteSpace(CustomerName))
            {
                CustomerNameError = "Customer name is required";
                isValid = false;
            }

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

            if (OrderServices.Count == 0)
            {
                ServiceError = "Please add at least one service";
                isValid = false;
            }

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
            string cleaned = phone.Replace("-", "").Replace(" ", "").Replace("(", "").Replace(")", "");

            return cleaned.Length >= 10 && cleaned.Length <= 11 && cleaned.All(char.IsDigit);
        }

        private bool CanSetAppointment()
        {
            return !string.IsNullOrWhiteSpace(CustomerName) &&
                   !string.IsNullOrWhiteSpace(Email) &&
                   !string.IsNullOrWhiteSpace(Phone) &&
                   OrderServices.Count > 0 &&
                   SelectedDate.HasValue;
        }

        private void SetAppointment()
        {
            if (!ValidateForm())
            {
                return;
            }

            // Split customer name into first and last name
            string[] nameParts = CustomerName.Trim().Split(new[] { ' ' }, 2);
            string firstName = nameParts[0];
            string lastName = nameParts.Length > 1 ? nameParts[1] : "";

            var customer = new Customer
            {
                FirstName = firstName,
                LastName = lastName,
                Email = Email,
                Phone = Phone,
                CreatedAt = DateTime.Now
            };

            // Create service orders for each service with single technician
            foreach (var orderServiceItem in OrderServices)
            {
                SaleID = serviceOrders.Count + 1, // Simple ID generation
                Customer = customer,
                Service = SelectedService,
                AppointmentDate = SelectedDate.Value,
                Status = "Pending",
                Technician = new User { Name = "Unassigned" } // Default technician
            };

                allServiceOrders.Add(serviceOrder);
                serviceOrders.Add(serviceOrder);
            }

            // Calculate total cost
            decimal totalCost = OrderServices.Sum(os => os.Service.Price);

            // Show success message
            MessageBox.Show($"Appointment created successfully!\n\n" +
                           $"Customer: {CustomerName}\n" +
                           $"Services: {OrderServices.Count}\n" +
                           $"Total Cost: ₱{totalCost:N2}\n" +
                           $"Date: {SelectedDate.Value:dd/MM/yyyy}",
                           "Success",
                           MessageBoxButton.OK,
                           MessageBoxImage.Information);

            // Clear form
            ClearForm();
        }

        private void ShowServiceSelector()
        {
            UpdateAvailableServicesToAdd();
            IsServiceSelectorVisible = true;
        }

        private void AddServiceToOrder(Service service)
        {
            if (service != null && !OrderServices.Any(os => os.Service.ServiceID == service.ServiceID))
            {
                var orderServiceItem = new OrderServiceItem
                {
                    Service = service,
                    Technician = new User { Name = "Select Technician" },
                    Status = "Pending"
                };
                OrderServices.Add(orderServiceItem);
                UpdateAvailableServicesToAdd();
                IsServiceSelectorVisible = false;
            }
        }

        private void RemoveServiceFromOrder(OrderServiceItem orderServiceItem)
        {
            if (orderServiceItem != null)
            {
                OrderServices.Remove(orderServiceItem);
                UpdateAvailableServicesToAdd();
            }
        }

        private void UpdateAvailableServicesToAdd()
        {
            AvailableServicesToAdd.Clear();
            foreach (var selectableService in selectableServices)
            {
                if (!OrderServices.Any(os => os.Service.ServiceID == selectableService.Service.ServiceID))
                {
                    AvailableServicesToAdd.Add(selectableService.Service);
                }
            }
        }

        private void SelectableService_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectableService.IsSelected))
            {
                var selectableService = sender as SelectableService;
                if (selectableService != null)
                {
                    if (selectableService.IsSelected)
                    {
                        // Add to OrderServices if not already there
                        if (!OrderServices.Any(os => os.Service.ServiceID == selectableService.Service.ServiceID))
                        {
                            var orderServiceItem = new OrderServiceItem
                            {
                                Service = selectableService.Service,
                                Status = "Pending"
                            };
                            OrderServices.Add(orderServiceItem);
                        }
                    }
                    else
                    {
                        // Remove from OrderServices
                        var itemToRemove = OrderServices.FirstOrDefault(os => os.Service.ServiceID == selectableService.Service.ServiceID);
                        if (itemToRemove != null)
                        {
                            OrderServices.Remove(itemToRemove);
                        }
                    }
                }
            }
        }

        private void ClearForm()
        {
            CustomerName = string.Empty;
            Email = string.Empty;
            Phone = string.Empty;
            SelectedService = null;
            SelectedDate = null;
            IssueDescription = string.Empty;
            SelectedTechnician = null;
            OrderServices.Clear();

            // Uncheck all selectable services
            foreach (var service in SelectableServices)
            {
                service.IsSelected = false;
            }

            UpdateAvailableServicesToAdd();

            // Clear errors
            CustomerNameError = string.Empty;
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

            Service service4 = new Service
            {
                ServiceID = 4,
                Name = "OS Install",
                Description = "Operating system installation",
                Price = 5999,
                CreatedAt = DateTime.Now,
                IsActive = true,
                CategoryID = 4,
                Category = new Category
                {
                    CategoryID = 4,
                    Name = "Software & System Services",
                    Type = "Software & System Services"
                }
            };

            Service service5 = new Service
            {
                ServiceID = 5,
                Name = "Hardware Diagnostics",
                Description = "Complete hardware diagnostic",
                Price = 1200,
                CreatedAt = DateTime.Now,
                IsActive = true,
                CategoryID = 5,
                Category = new Category
                {
                    CategoryID = 5,
                    Name = "Hardware Services",
                    Type = "Hardware Services"
                }
            };

            var servicesList = new List<Service>
            {
                service1,
                service2,
                service3,
                service4,
                service5
            };

            // Create SelectableServices for checkbox selection
            selectableServices = new ObservableCollection<SelectableService>();
            foreach (var service in servicesList)
            {
                var selectableService = new SelectableService
                {
                    Service = service,
                    IsSelected = false
                };
                selectableService.PropertyChanged += SelectableService_PropertyChanged;
                selectableServices.Add(selectableService);
            }

            UpdateAvailableServicesToAdd();

            availableTechnicians = new ObservableCollection<User>
            {
                new User { Name = "All Technicians" }, // Filter option
                technician1,
                technician2,
                technician3
            };
            selectedTechnicianFilter = availableTechnicians[0]; 
            OnPropertyChanged(nameof(AvailableTechnicians));

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

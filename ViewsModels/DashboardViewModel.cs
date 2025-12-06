using gentech_services.Models;
using gentech_services.MVVM;
using ProductServicesManagementSystem.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace gentech_services.ViewsModels
{
    public class DashboardViewModel : INotifyPropertyChanged
    {
        private decimal todaysSales;
        private int lowStockCount;
        private int openServiceJobs;
        private int totalTransactions;

        public decimal TodaysSales
        {
            get { return todaysSales; }
            set
            {
                todaysSales = value;
                OnPropertyChanged(nameof(TodaysSales));
            }
        }

        public int LowStockCount
        {
            get { return lowStockCount; }
            set
            {
                lowStockCount = value;
                OnPropertyChanged(nameof(LowStockCount));
            }
        }

        public int OpenServiceJobs
        {
            get { return openServiceJobs; }
            set
            {
                openServiceJobs = value;
                OnPropertyChanged(nameof(OpenServiceJobs));
            }
        }

        public int TotalTransactions
        {
            get { return totalTransactions; }
            set
            {
                totalTransactions = value;
                OnPropertyChanged(nameof(TotalTransactions));
            }
        }

        public ObservableCollection<Product> LowStockItems { get; set; }
        public ObservableCollection<Sale> RecentTransactions { get; set; }
        public ObservableCollection<Sale> PendingServiceJobs { get; set; }

        public ICommand NavigateToPOSCommand { get; }
        public ICommand NavigateToInventoryCommand { get; }
        public ICommand NavigateToProductOrdersCommand { get; }
        public ICommand NavigateToServiceOrdersCommand { get; }
        public ICommand NavigateToUsersCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public DashboardViewModel()
        {
            LowStockItems = new ObservableCollection<Product>();
            RecentTransactions = new ObservableCollection<Sale>();
            PendingServiceJobs = new ObservableCollection<Sale>();

            NavigateToPOSCommand = new RelayCommand(NavigateToPOS);
            NavigateToInventoryCommand = new RelayCommand(NavigateToInventory);
            NavigateToProductOrdersCommand = new RelayCommand(NavigateToProductOrders);
            NavigateToServiceOrdersCommand = new RelayCommand(NavigateToServiceOrders);
            NavigateToUsersCommand = new RelayCommand(NavigateToUsers);

            LoadDashboardData();
        }

        private void LoadDashboardData()
        {
       
        }

        private void NavigateToPOS(object parameter)
        {
        
        }

        private void NavigateToInventory(object parameter)
        {
        }

        private void NavigateToProductOrders(object parameter)
        {
        }

        private void NavigateToServiceOrders(object parameter)
        {
        }

        private void NavigateToUsers(object parameter)
        {
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

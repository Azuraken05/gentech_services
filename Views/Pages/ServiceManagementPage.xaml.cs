using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using gentech_services.Models;
using ProductServicesManagementSystem.Models;

namespace gentech_services.Views.Pages
{
    /// <summary>
    /// Interaction logic for ServiceManagementPage.xaml
    /// </summary>
    public partial class ServiceManagementPage : UserControl
    {
        public ObservableCollection<Service> Services { get; set; }
        public ObservableCollection<Category> Categories { get; set; }

        public ServiceManagementPage()
        {
            InitializeComponent();
            LoadSampleData();
            DataContext = this;
        }

        public class CategoryViewModel
        {
            public int CategoryID { get; set; }
            public string Name { get; set; }
            public int ServicesCount { get; set; }
        }

        private void LoadSampleData()
        {
            // Load sample categories
            Categories = new ObservableCollection<Category>
            {
                new Category { CategoryID = 1, Name = "Hardware Services", Type = "Service" },
                new Category { CategoryID = 2, Name = "Software Services", Type = "Service" },
                new Category { CategoryID = 3, Name = "Repair Services", Type = "Service" }
            };

            // Load sample services
            Services = new ObservableCollection<Service>
            {
                new Service
                {
                    ServiceID = 1,
                    Name = "Broken Motherboard",
                    Description = "Motherboard repair or replacement",
                    Price = 5999.00m,
                    CategoryID = 3,
                    Category = Categories[2],
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddMonths(-2),
                    UpdatedAt = new DateTime(2025, 2, 11)
                },
                new Service
                {
                    ServiceID = 2,
                    Name = "RAM Upgrade",
                    Description = "Memory upgrade service",
                    Price = 3500.00m,
                    CategoryID = 1,
                    Category = Categories[0],
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddMonths(-3),
                    UpdatedAt = new DateTime(2025, 1, 15)
                },
                new Service
                {
                    ServiceID = 3,
                    Name = "Windows Installation",
                    Description = "Fresh Windows OS installation",
                    Price = 1500.00m,
                    CategoryID = 2,
                    Category = Categories[1],
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddMonths(-1),
                    UpdatedAt = new DateTime(2025, 2, 20)
                },
                new Service
                {
                    ServiceID = 4,
                    Name = "Virus Removal",
                    Description = "Complete virus and malware removal",
                    Price = 1000.00m,
                    CategoryID = 2,
                    Category = Categories[1],
                    IsActive = false,
                    CreatedAt = DateTime.Now.AddMonths(-4),
                    UpdatedAt = new DateTime(2025, 1, 5)
                },
                new Service
                {
                    ServiceID = 5,
                    Name = "Screen Replacement",
                    Description = "Laptop screen replacement",
                    Price = 4500.00m,
                    CategoryID = 3,
                    Category = Categories[2],
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddMonths(-2),
                    UpdatedAt = new DateTime(2025, 2, 8)
                }
            };

            ServicesItemsControl.ItemsSource = Services;

            // Create category view models with service counts
            var categoryViewModels = Categories.Select(c => new CategoryViewModel
            {
                CategoryID = c.CategoryID,
                Name = c.Name,
                ServicesCount = Services.Count(s => s.CategoryID == c.CategoryID)
            }).ToList();

            CategoriesItemsControl.ItemsSource = categoryViewModels;
        }
    }
}

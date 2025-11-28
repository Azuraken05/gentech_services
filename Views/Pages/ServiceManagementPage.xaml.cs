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
        private ObservableCollection<CategoryViewModel> categoryViewModels;
        private ObservableCollection<Service> filteredServices;

        public ServiceManagementPage()
        {
            InitializeComponent();
            LoadSampleData();
            DataContext = this;

            // Wire up modal event
            AddServiceModalControl.OnServiceSaved += AddServiceModalControl_OnServiceSaved;

            // Wire up edit modal event
            EditServiceModalControl.OnServiceUpdated += EditServiceModalControl_OnServiceUpdated;

            // Wire up action menu events
            ServiceActionMenuControl.OnEdit += ServiceActionMenuControl_OnEdit;
            ServiceActionMenuControl.OnToggleActive += ServiceActionMenuControl_OnToggleActive;
            ServiceActionMenuControl.OnDelete += ServiceActionMenuControl_OnDelete;
        }

        public class CategoryViewModel
        {
            public int CategoryID { get; set; }
            public string Name { get; set; }
            public int ServicesCount { get; set; }
            public Category OriginalCategory { get; set; }
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

            RefreshCategoryList();
        }

        private void RefreshCategoryList()
        {
            // Create category view models with service counts
            categoryViewModels = new ObservableCollection<CategoryViewModel>(
                Categories.Select(c => new CategoryViewModel
                {
                    CategoryID = c.CategoryID,
                    Name = c.Name,
                    ServicesCount = Services.Count(s => s.CategoryID == c.CategoryID),
                    OriginalCategory = c
                })
            );

            CategoriesItemsControl.ItemsSource = categoryViewModels;
        }

        private void RenameCategory_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var categoryVM = button?.DataContext as CategoryViewModel;
            if (categoryVM == null) return;

            // Create input dialog
            var inputDialog = new Window
            {
                Title = "Rename Category",
                Width = 400,
                Height = 220,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize,
                Background = Brushes.White
            };

            var grid = new Grid { Margin = new Thickness(20) };
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            var label = new TextBlock
            {
                Text = "Category Name:",
                FontSize = 14,
                FontWeight = FontWeights.SemiBold,
                Margin = new Thickness(0, 0, 0, 8)
            };

            var textBox = new TextBox
            {
                Text = categoryVM.Name,
                Height = 35,
                FontSize = 13,
                Padding = new Thickness(8),
                Margin = new Thickness(0, 0, 0, 20),
                VerticalContentAlignment = VerticalAlignment.Center
            };

            var buttonPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 20, 0, 0)
            };

            var cancelButton = new Button
            {
                Content = "Cancel",
                Width = 100,
                Height = 35,
                Margin = new Thickness(0, 0, 10, 0),
                Background = Brushes.White,
                BorderBrush = Brushes.Gray,
                BorderThickness = new Thickness(1),
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center
            };
            cancelButton.Click += (s, args) => inputDialog.Close();

            var saveButton = new Button
            {
                Content = "Save",
                Width = 100,
                Height = 35,
                Background = (Brush)new BrushConverter().ConvertFrom("#0000FF"),
                Foreground = Brushes.White,
                BorderThickness = new Thickness(0),
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center
            };
            saveButton.Click += (s, args) =>
            {
                var newName = textBox.Text.Trim();
                if (!string.IsNullOrEmpty(newName))
                {
                    categoryVM.OriginalCategory.Name = newName;

                    // Update all services that reference this category
                    foreach (var service in Services.Where(s => s.CategoryID == categoryVM.CategoryID))
                    {
                        service.Category = categoryVM.OriginalCategory;
                    }

                    // Refresh both lists
                    RefreshCategoryList();
                    ServicesItemsControl.ItemsSource = null;
                    ServicesItemsControl.ItemsSource = Services;

                    inputDialog.Close();
                }
                else
                {
                    MessageBox.Show("Category name cannot be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            };

            buttonPanel.Children.Add(cancelButton);
            buttonPanel.Children.Add(saveButton);

            Grid.SetRow(label, 0);
            Grid.SetRow(textBox, 1);
            Grid.SetRow(buttonPanel, 2);

            grid.Children.Add(label);
            grid.Children.Add(textBox);
            grid.Children.Add(buttonPanel);

            inputDialog.Content = grid;
            textBox.Focus();
            textBox.SelectAll();
            inputDialog.ShowDialog();
        }

        private void DeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var categoryVM = button?.DataContext as CategoryViewModel;
            if (categoryVM == null) return;

            // Check if category has services
            if (categoryVM.ServicesCount > 0)
            {
                MessageBox.Show(
                    $"Cannot delete '{categoryVM.Name}' because it has {categoryVM.ServicesCount} service(s) assigned to it.\n\nPlease reassign or delete those services first.",
                    "Cannot Delete Category",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }

            var result = MessageBox.Show(
                $"Are you sure you want to delete the category '{categoryVM.Name}'?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.Yes)
            {
                Categories.Remove(categoryVM.OriginalCategory);
                RefreshCategoryList();
            }
        }

        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            var categoryName = NewCategoryTextBox.Text.Trim();

            if (string.IsNullOrEmpty(categoryName))
            {
                MessageBox.Show("Please enter a category name.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Check if category already exists
            if (Categories.Any(c => c.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("A category with this name already exists.", "Duplicate Category", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Get next ID
            int nextId = Categories.Any() ? Categories.Max(c => c.CategoryID) + 1 : 1;

            var newCategory = new Category
            {
                CategoryID = nextId,
                Name = categoryName,
                Type = "Service"
            };

            Categories.Add(newCategory);
            RefreshCategoryList();

            // Clear the textbox
            NewCategoryTextBox.Text = string.Empty;

            MessageBox.Show($"Category '{categoryName}' has been added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void NewService_Click(object sender, RoutedEventArgs e)
        {
            // Initialize modal with categories
            AddServiceModalControl.Initialize(Categories);

            // Show modal
            ModalOverlay.Visibility = Visibility.Visible;
        }

        private void ModalOverlay_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Close modal when clicking on overlay (outside the modal)
            if (e.OriginalSource == ModalOverlay)
            {
                ModalOverlay.Visibility = Visibility.Collapsed;
            }
        }

        private void EditModalOverlay_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Close edit modal when clicking on overlay (outside the modal)
            if (e.OriginalSource == EditModalOverlay)
            {
                EditModalOverlay.Visibility = Visibility.Collapsed;
            }
        }

        private void EditServiceModalControl_OnServiceUpdated(object sender, Service updatedService)
        {
            // Refresh services table
            ServicesItemsControl.ItemsSource = null;
            ServicesItemsControl.ItemsSource = Services;

            // Refresh categories to update service counts
            RefreshCategoryList();

            // Close edit modal
            EditModalOverlay.Visibility = Visibility.Collapsed;

            MessageBox.Show($"Service '{updatedService.Name}' has been updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void AddServiceModalControl_OnServiceSaved(object sender, Service newService)
        {
            // Get next ServiceID
            int nextId = Services.Any() ? Services.Max(s => s.ServiceID) + 1 : 1;
            newService.ServiceID = nextId;

            // Add to collection
            Services.Add(newService);

            // Refresh services table
            ServicesItemsControl.ItemsSource = null;
            ServicesItemsControl.ItemsSource = Services;

            // Refresh categories to update service counts
            RefreshCategoryList();

            // Close modal
            ModalOverlay.Visibility = Visibility.Collapsed;

            MessageBox.Show($"Service '{newService.Name}' has been added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ServiceActionButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var service = button?.DataContext as Service;
            if (service == null) return;

            // Set the service in the action menu
            ServiceActionMenuControl.SetService(service);

            // Position and show the popup
            ServiceActionPopup.PlacementTarget = button;
            ServiceActionPopup.IsOpen = true;
        }

        private void ServiceActionMenuControl_OnEdit(object sender, Service service)
        {
            ServiceActionPopup.IsOpen = false;

            // Initialize edit modal with categories and service
            EditServiceModalControl.Initialize(Categories, service);

            // Show edit modal
            EditModalOverlay.Visibility = Visibility.Visible;
        }

        private void ServiceActionMenuControl_OnToggleActive(object sender, Service service)
        {
            ServiceActionPopup.IsOpen = false;

            // Toggle the IsActive status
            service.IsActive = !service.IsActive;
            service.UpdatedAt = DateTime.Now;

            // Refresh the services table to show updated status
            ServicesItemsControl.ItemsSource = null;
            ServicesItemsControl.ItemsSource = Services;

            string status = service.IsActive ? "activated" : "deactivated";
            MessageBox.Show($"Service '{service.Name}' has been {status}.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ServiceActionMenuControl_OnDelete(object sender, Service service)
        {
            ServiceActionPopup.IsOpen = false;

            var result = MessageBox.Show(
                $"Are you sure you want to delete the service '{service.Name}'?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.Yes)
            {
                Services.Remove(service);

                // Refresh services table
                ServicesItemsControl.ItemsSource = null;
                ServicesItemsControl.ItemsSource = Services;

                // Refresh categories to update service counts
                RefreshCategoryList();

                MessageBox.Show($"Service '{service.Name}' has been deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchTextBox.Text.ToLower().Trim();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                // Show all services if search is empty
                ServicesItemsControl.ItemsSource = Services;
            }
            else
            {
                // Filter services based on search text
                filteredServices = new ObservableCollection<Service>(
                    Services.Where(s =>
                        s.Name.ToLower().Contains(searchText) ||
                        s.Description.ToLower().Contains(searchText) ||
                        s.Category.Name.ToLower().Contains(searchText) ||
                        s.Price.ToString().Contains(searchText)
                    )
                );

                ServicesItemsControl.ItemsSource = filteredServices;
            }
        }
    }
}

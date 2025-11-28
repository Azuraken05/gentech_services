using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using gentech_services.Models;
using ProductServicesManagementSystem.Models;

namespace gentech_services.Views.UserControls
{
    public partial class EditServiceModal : UserControl
    {
        public event EventHandler<Service> OnServiceUpdated;
        private Service currentService;

        public EditServiceModal()
        {
            InitializeComponent();
        }

        public void Initialize(ObservableCollection<Category> categories, Service service)
        {
            currentService = service;

            // Populate category dropdown
            ServiceCategoryComboBox.ItemsSource = categories;

            // Pre-fill form with service data
            ServiceNameTextBox.Text = service.Name;
            ServiceDescriptionTextBox.Text = service.Description;
            ServicePriceTextBox.Text = service.Price.ToString("F2");

            // Select the current category
            ServiceCategoryComboBox.SelectedItem = categories.FirstOrDefault(c => c.CategoryID == service.CategoryID);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(ServiceNameTextBox.Text))
            {
                MessageBox.Show("Please enter a service name.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(ServicePriceTextBox.Text))
            {
                MessageBox.Show("Please enter a price.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(ServicePriceTextBox.Text, out decimal price))
            {
                MessageBox.Show("Please enter a valid price.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (ServiceCategoryComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a category.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Update service properties
            currentService.Name = ServiceNameTextBox.Text.Trim();
            currentService.Description = ServiceDescriptionTextBox.Text.Trim();
            currentService.Price = price;

            var selectedCategory = ServiceCategoryComboBox.SelectedItem as Category;
            currentService.CategoryID = selectedCategory.CategoryID;
            currentService.Category = selectedCategory;
            currentService.UpdatedAt = DateTime.Now;

            // Raise event
            OnServiceUpdated?.Invoke(this, currentService);
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            // Reset form to original service data
            if (currentService != null)
            {
                ServiceNameTextBox.Text = currentService.Name;
                ServiceDescriptionTextBox.Text = currentService.Description;
                ServicePriceTextBox.Text = currentService.Price.ToString("F2");

                var categories = ServiceCategoryComboBox.ItemsSource as ObservableCollection<Category>;
                ServiceCategoryComboBox.SelectedItem = categories?.FirstOrDefault(c => c.CategoryID == currentService.CategoryID);
            }
        }
    }
}

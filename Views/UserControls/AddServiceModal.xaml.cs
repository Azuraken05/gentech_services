using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using gentech_services.Models;
using ProductServicesManagementSystem.Models;

namespace gentech_services.Views.UserControls
{
    public partial class AddServiceModal : UserControl
    {
        public event EventHandler<Service> OnServiceSaved;
        public IEnumerable<Category> Categories { get; set; }

        public AddServiceModal()
        {
            InitializeComponent();
        }

        public void Initialize(IEnumerable<Category> categories)
        {
            Categories = categories;
            ServiceCategoryComboBox.ItemsSource = Categories;

            // Select first category by default
            if (Categories.Any())
            {
                ServiceCategoryComboBox.SelectedIndex = 0;
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ServiceNameTextBox.Text = string.Empty;
            ServiceDescriptionTextBox.Text = string.Empty;
            ServicePriceTextBox.Text = string.Empty;
            ServiceCategoryComboBox.SelectedIndex = 0;
            ServiceActiveCheckBox.IsChecked = true;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(ServiceNameTextBox.Text))
            {
                MessageBox.Show("Please enter a service name.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(ServiceDescriptionTextBox.Text))
            {
                MessageBox.Show("Please enter a description.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(ServicePriceTextBox.Text))
            {
                MessageBox.Show("Please enter a price.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Parse price
            decimal price;
            var priceText = ServicePriceTextBox.Text.Replace("₱", "").Replace(",", "").Trim();
            if (!decimal.TryParse(priceText, out price) || price <= 0)
            {
                MessageBox.Show("Please enter a valid price.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (ServiceCategoryComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a category.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selectedCategory = ServiceCategoryComboBox.SelectedItem as Category;

            // Create new service
            var newService = new Service
            {
                Name = ServiceNameTextBox.Text.Trim(),
                Description = ServiceDescriptionTextBox.Text.Trim(),
                Price = price,
                CategoryID = selectedCategory.CategoryID,
                Category = selectedCategory,
                IsActive = ServiceActiveCheckBox.IsChecked ?? true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            // Raise event
            OnServiceSaved?.Invoke(this, newService);

            // Clear form
            ClearButton_Click(null, null);
        }
    }
}

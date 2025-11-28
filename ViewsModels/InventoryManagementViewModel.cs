using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using gentech_services.MVVM;
using gentech_services.Models;

namespace gentech_services.ViewsModels
{
    internal class InventoryManagementViewModel : ViewModelBase
    {
        private ObservableCollection<ProductViewModel> products;
        private ObservableCollection<ProductViewModel> allProducts;
        private ProductViewModel selectedProduct;
        private string searchText;

        // Low stock products
        private ObservableCollection<ProductViewModel> lowStockProducts;
        private bool hasLowStockItems;

        // Add Product Form Properties
        private string productName;
        private string sku;
        private string selectedCategory;
        private decimal? price;
        private int? stockQuantity;
        private string description;

        // Edit Product Form Properties
        private string editProductName;
        private string editSku;
        private string editSelectedCategory;
        private decimal? editPrice;
        private int? editStockQuantity;
        private string editDescription;

        // View Product Properties
        private string viewSku;
        private string viewCategory;
        private string viewStatus;
        private string viewPrice;
        private string viewStock;
        private string viewDescription;

        // Modal Visibility
        private bool isAddModalVisible;
        private bool isEditModalVisible;
        private bool isViewModalVisible;

        public ObservableCollection<ProductViewModel> Products
        {
            get { return products; }
            set
            {
                products = value;
                OnPropertyChanged(nameof(Products));
            }
        }

        public ProductViewModel SelectedProduct
        {
            get { return selectedProduct; }
            set
            {
                selectedProduct = value;
                OnPropertyChanged(nameof(SelectedProduct));
            }
        }

        public string SearchText
        {
            get { return searchText; }
            set
            {
                searchText = value;
                OnPropertyChanged(nameof(SearchText));
                ApplySearch();
            }
        }

        public ObservableCollection<ProductViewModel> LowStockProducts
        {
            get { return lowStockProducts; }
            set
            {
                lowStockProducts = value;
                OnPropertyChanged(nameof(LowStockProducts));
            }
        }

        public bool HasLowStockItems
        {
            get { return hasLowStockItems; }
            set
            {
                hasLowStockItems = value;
                OnPropertyChanged(nameof(HasLowStockItems));
            }
        }

        // Add Product Form Properties
        public string ProductName
        {
            get { return productName; }
            set
            {
                productName = value;
                OnPropertyChanged(nameof(ProductName));
                AddProductCommand.RaiseCanExecuteChanged();
            }
        }

        public string SKU
        {
            get { return sku; }
            set
            {
                sku = value;
                OnPropertyChanged(nameof(SKU));
                AddProductCommand.RaiseCanExecuteChanged();
            }
        }

        public string SelectedCategory
        {
            get { return selectedCategory; }
            set
            {
                selectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
                AddProductCommand.RaiseCanExecuteChanged();
            }
        }

        public decimal? Price
        {
            get { return price; }
            set
            {
                price = value;
                OnPropertyChanged(nameof(Price));
                AddProductCommand.RaiseCanExecuteChanged();
            }
        }

        public int? StockQuantity
        {
            get { return stockQuantity; }
            set
            {
                stockQuantity = value;
                OnPropertyChanged(nameof(StockQuantity));
                AddProductCommand.RaiseCanExecuteChanged();
            }
        }

        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        // Edit Product Form Properties
        public string EditProductName
        {
            get { return editProductName; }
            set
            {
                editProductName = value;
                OnPropertyChanged(nameof(EditProductName));
                SaveEditCommand.RaiseCanExecuteChanged();
            }
        }

        public string EditSKU
        {
            get { return editSku; }
            set
            {
                editSku = value;
                OnPropertyChanged(nameof(EditSKU));
                SaveEditCommand.RaiseCanExecuteChanged();
            }
        }

        public string EditSelectedCategory
        {
            get { return editSelectedCategory; }
            set
            {
                editSelectedCategory = value;
                OnPropertyChanged(nameof(EditSelectedCategory));
            }
        }

        public decimal? EditPrice
        {
            get { return editPrice; }
            set
            {
                editPrice = value;
                OnPropertyChanged(nameof(EditPrice));
                SaveEditCommand.RaiseCanExecuteChanged();
            }
        }

        public int? EditStockQuantity
        {
            get { return editStockQuantity; }
            set
            {
                editStockQuantity = value;
                OnPropertyChanged(nameof(EditStockQuantity));
                SaveEditCommand.RaiseCanExecuteChanged();
            }
        }

        public string EditDescription
        {
            get { return editDescription; }
            set
            {
                editDescription = value;
                OnPropertyChanged(nameof(EditDescription));
            }
        }

        // View Product Properties
        public string ViewSKU
        {
            get { return viewSku; }
            set
            {
                viewSku = value;
                OnPropertyChanged(nameof(ViewSKU));
            }
        }

        public string ViewCategory
        {
            get { return viewCategory; }
            set
            {
                viewCategory = value;
                OnPropertyChanged(nameof(ViewCategory));
            }
        }

        public string ViewStatus
        {
            get { return viewStatus; }
            set
            {
                viewStatus = value;
                OnPropertyChanged(nameof(ViewStatus));
            }
        }

        public string ViewPrice
        {
            get { return viewPrice; }
            set
            {
                viewPrice = value;
                OnPropertyChanged(nameof(ViewPrice));
            }
        }

        public string ViewStock
        {
            get { return viewStock; }
            set
            {
                viewStock = value;
                OnPropertyChanged(nameof(ViewStock));
            }
        }

        public string ViewDescription
        {
            get { return viewDescription; }
            set
            {
                viewDescription = value;
                OnPropertyChanged(nameof(ViewDescription));
            }
        }

        // Modal Visibility
        public bool IsAddModalVisible
        {
            get { return isAddModalVisible; }
            set
            {
                isAddModalVisible = value;
                OnPropertyChanged(nameof(IsAddModalVisible));
            }
        }

        public bool IsEditModalVisible
        {
            get { return isEditModalVisible; }
            set
            {
                isEditModalVisible = value;
                OnPropertyChanged(nameof(IsEditModalVisible));
            }
        }

        public bool IsViewModalVisible
        {
            get { return isViewModalVisible; }
            set
            {
                isViewModalVisible = value;
                OnPropertyChanged(nameof(IsViewModalVisible));
            }
        }

        // Commands
        public RelayCommand NewProductCommand { get; private set; }
        public RelayCommand AddProductCommand { get; private set; }
        public RelayCommand CancelAddCommand { get; private set; }
        public RelayCommand EditProductCommand { get; private set; }
        public RelayCommand SaveEditCommand { get; private set; }
        public RelayCommand ClearEditCommand { get; private set; }
        public RelayCommand ViewProductCommand { get; private set; }
        public RelayCommand CloseViewModalCommand { get; private set; }

        public InventoryManagementViewModel()
        {
            NewProductCommand = new RelayCommand(obj => OpenAddModal());
            AddProductCommand = new RelayCommand(obj => AddProduct(), obj => CanAddProduct());
            CancelAddCommand = new RelayCommand(obj => CloseAddModal());
            EditProductCommand = new RelayCommand(obj => OpenEditModal(obj as ProductViewModel));
            SaveEditCommand = new RelayCommand(obj => SaveEdit(), obj => CanSaveEdit());
            ClearEditCommand = new RelayCommand(obj => ClearEditForm());
            ViewProductCommand = new RelayCommand(obj => OpenViewModal(obj as ProductViewModel));
            CloseViewModalCommand = new RelayCommand(obj => CloseViewModal());

            LoadSampleData();
            UpdateLowStockAlert();
        }

        private void LoadSampleData()
        {
            allProducts = new ObservableCollection<ProductViewModel>
            {
                new ProductViewModel
                {
                    ProductID = 1,
                    Name = "LENOVO LEGION 3",
                    SKU = "83M00051PH",
                    CategoryName = "LAPTOP",
                    Price = 49999m,
                    StockQuanity = 300,
                    IsActive = true,
                    Description = "High Performing Laptop",
                    CreatedAt = DateTime.Now.AddMonths(-6)
                },
                new ProductViewModel
                {
                    ProductID = 2,
                    Name = "Gaming Chair",
                    SKU = "GC-2024-BLK",
                    CategoryName = "Accessories",
                    Price = 15999m,
                    StockQuanity = 8,
                    IsActive = true,
                    Description = "Ergonomic gaming chair with lumbar support",
                    CreatedAt = DateTime.Now.AddMonths(-5)
                },
                new ProductViewModel
                {
                    ProductID = 3,
                    Name = "GTX 7090",
                    SKU = "GPU-GTX7090-24GB",
                    CategoryName = "Parts",
                    Price = 89990m,
                    StockQuanity = 5,
                    IsActive = true,
                    Description = "High-end graphics card for gaming",
                    CreatedAt = DateTime.Now.AddMonths(-4)
                },
                new ProductViewModel
                {
                    ProductID = 4,
                    Name = "Mechanical Keyboard RGB",
                    SKU = "KB-MECH-RGB-001",
                    CategoryName = "Accessories",
                    Price = 8990m,
                    StockQuanity = 45,
                    IsActive = true,
                    Description = "RGB mechanical keyboard with blue switches",
                    CreatedAt = DateTime.Now.AddMonths(-4)
                },
                new ProductViewModel
                {
                    ProductID = 5,
                    Name = "Wireless Gaming Mouse",
                    SKU = "MS-WGAME-001",
                    CategoryName = "Accessories",
                    Price = 3490m,
                    StockQuanity = 60,
                    IsActive = true,
                    Description = "Wireless gaming mouse with 16000 DPI",
                    CreatedAt = DateTime.Now.AddMonths(-3)
                },
                new ProductViewModel
                {
                    ProductID = 6,
                    Name = "RAM DDR5 32GB Kit",
                    SKU = "RAM-DDR5-32GB-KIT",
                    CategoryName = "Parts",
                    Price = 12990m,
                    StockQuanity = 25,
                    IsActive = true,
                    Description = "High-speed DDR5 RAM 32GB (2x16GB) kit",
                    CreatedAt = DateTime.Now.AddMonths(-3)
                },
                new ProductViewModel
                {
                    ProductID = 7,
                    Name = "SSD NVMe 2TB",
                    SKU = "SSD-NVM-2TB-001",
                    CategoryName = "Parts",
                    Price = 10490m,
                    StockQuanity = 18,
                    IsActive = true,
                    Description = "2TB NVMe SSD with 7000MB/s read speed",
                    CreatedAt = DateTime.Now.AddMonths(-2)
                },
                new ProductViewModel
                {
                    ProductID = 8,
                    Name = "27\" 4K Monitor",
                    SKU = "MON-4K-27-001",
                    CategoryName = "Electronics",
                    Price = 28990m,
                    StockQuanity = 12,
                    IsActive = true,
                    Description = "27-inch 4K IPS monitor with 144Hz",
                    CreatedAt = DateTime.Now.AddMonths(-2)
                },
                new ProductViewModel
                {
                    ProductID = 9,
                    Name = "CPU Ryzen 9 7950X",
                    SKU = "CPU-AMD-7950X",
                    CategoryName = "Parts",
                    Price = 35990m,
                    StockQuanity = 7,
                    IsActive = true,
                    Description = "16-core 32-thread flagship processor",
                    CreatedAt = DateTime.Now.AddMonths(-1)
                },
                new ProductViewModel
                {
                    ProductID = 10,
                    Name = "Motherboard X670E",
                    SKU = "MB-AMD-X670E",
                    CategoryName = "Parts",
                    Price = 18990m,
                    StockQuanity = 15,
                    IsActive = true,
                    Description = "ATX motherboard for AMD Ryzen 7000 series",
                    CreatedAt = DateTime.Now.AddMonths(-1)
                },
                new ProductViewModel
                {
                    ProductID = 11,
                    Name = "Power Supply 850W Gold",
                    SKU = "PSU-850W-GOLD",
                    CategoryName = "Parts",
                    Price = 7490m,
                    StockQuanity = 22,
                    IsActive = true,
                    Description = "850W 80+ Gold certified modular PSU",
                    CreatedAt = DateTime.Now.AddDays(-25)
                },
                new ProductViewModel
                {
                    ProductID = 12,
                    Name = "PC Case Mid Tower",
                    SKU = "CASE-MID-RGB-001",
                    CategoryName = "Accessories",
                    Price = 5990m,
                    StockQuanity = 30,
                    IsActive = true,
                    Description = "Mid tower case with tempered glass and RGB",
                    CreatedAt = DateTime.Now.AddDays(-20)
                },
                new ProductViewModel
                {
                    ProductID = 13,
                    Name = "CPU Cooler AIO 360mm",
                    SKU = "COOL-AIO-360",
                    CategoryName = "Parts",
                    Price = 9990m,
                    StockQuanity = 14,
                    IsActive = true,
                    Description = "360mm AIO liquid cooler with RGB fans",
                    CreatedAt = DateTime.Now.AddDays(-15)
                },
                new ProductViewModel
                {
                    ProductID = 14,
                    Name = "Webcam 4K",
                    SKU = "CAM-4K-001",
                    CategoryName = "Accessories",
                    Price = 4990m,
                    StockQuanity = 38,
                    IsActive = true,
                    Description = "4K webcam with auto-focus and noise reduction",
                    CreatedAt = DateTime.Now.AddDays(-10)
                },
                new ProductViewModel
                {
                    ProductID = 15,
                    Name = "Headset 7.1 Surround",
                    SKU = "HS-71-RGB-001",
                    CategoryName = "Accessories",
                    Price = 6490m,
                    StockQuanity = 42,
                    IsActive = true,
                    Description = "7.1 surround sound gaming headset",
                    CreatedAt = DateTime.Now.AddDays(-5)
                }
            };

            Products = new ObservableCollection<ProductViewModel>(allProducts);
        }

        private void UpdateLowStockAlert()
        {
            var lowStock = allProducts.Where(p => p.StockQuanity <= 10).ToList();
            LowStockProducts = new ObservableCollection<ProductViewModel>(lowStock);
            HasLowStockItems = lowStock.Any();
        }

        private void ApplySearch()
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                Products = new ObservableCollection<ProductViewModel>(allProducts);
            }
            else
            {
                string search = searchText.ToLower().Trim();
                var filtered = allProducts.Where(p =>
                    p.ProductID.ToString().Contains(search) ||
                    p.Name.ToLower().Contains(search) ||
                    p.SKU.ToLower().Contains(search) ||
                    p.CategoryName.ToLower().Contains(search)
                ).ToList();

                Products = new ObservableCollection<ProductViewModel>(filtered);
            }
        }

        private void OpenAddModal()
        {
            IsAddModalVisible = true;
        }

        private void CloseAddModal()
        {
            IsAddModalVisible = false;
            ClearAddForm();
        }

        private void ClearAddForm()
        {
            ProductName = string.Empty;
            SKU = string.Empty;
            SelectedCategory = null;
            Price = null;
            StockQuantity = null;
            Description = string.Empty;
        }

        private bool CanAddProduct()
        {
            return !string.IsNullOrWhiteSpace(ProductName) &&
                   !string.IsNullOrWhiteSpace(SKU) &&
                   !string.IsNullOrWhiteSpace(SelectedCategory) &&
                   Price.HasValue && Price.Value > 0 &&
                   StockQuantity.HasValue && StockQuantity.Value >= 0;
        }

        private void AddProduct()
        {
            int nextID = allProducts.Count > 0 ? allProducts.Max(p => p.ProductID) + 1 : 1;

            var newProduct = new ProductViewModel
            {
                ProductID = nextID,
                Name = ProductName.Trim(),
                SKU = SKU.Trim(),
                CategoryName = SelectedCategory,
                Price = Price.Value,
                StockQuanity = StockQuantity.Value,
                IsActive = true,
                Description = Description?.Trim() ?? string.Empty,
                CreatedAt = DateTime.Now
            };

            allProducts.Add(newProduct);
            Products.Add(newProduct);
            UpdateLowStockAlert();

            MessageBox.Show($"Product '{newProduct.Name}' has been added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            CloseAddModal();
        }

        private void OpenEditModal(ProductViewModel product)
        {
            if (product == null) return;

            SelectedProduct = product;

            EditProductName = product.Name;
            EditSKU = product.SKU;
            EditSelectedCategory = product.CategoryName;
            EditPrice = product.Price;
            EditStockQuantity = product.StockQuanity;
            EditDescription = product.Description;

            IsEditModalVisible = true;
        }

        private void ClearEditForm()
        {
            EditProductName = string.Empty;
            EditSKU = string.Empty;
            EditSelectedCategory = null;
            EditPrice = null;
            EditStockQuantity = null;
            EditDescription = string.Empty;
        }

        private bool CanSaveEdit()
        {
            return !string.IsNullOrWhiteSpace(EditProductName) &&
                   !string.IsNullOrWhiteSpace(EditSKU) &&
                   EditPrice.HasValue && EditPrice.Value > 0 &&
                   EditStockQuantity.HasValue && EditStockQuantity.Value >= 0;
        }

        private void SaveEdit()
        {
            if (SelectedProduct == null) return;

            SelectedProduct.Name = EditProductName.Trim();
            SelectedProduct.SKU = EditSKU.Trim();
            SelectedProduct.Price = EditPrice.Value;
            SelectedProduct.StockQuanity = EditStockQuantity.Value;
            SelectedProduct.Description = EditDescription?.Trim() ?? string.Empty;
            SelectedProduct.CategoryName = EditSelectedCategory ?? SelectedProduct.CategoryName;

            // Refresh the Products collection to trigger UI update
            var temp = Products;
            Products = null;
            Products = temp;

            UpdateLowStockAlert();

            MessageBox.Show($"Product '{SelectedProduct.Name}' has been updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            IsEditModalVisible = false;
            ClearEditForm();
        }

        private void OpenViewModal(ProductViewModel product)
        {
            if (product == null) return;

            ViewSKU = product.SKU;
            ViewCategory = product.CategoryName;
            ViewStatus = product.StockQuanity > 10 ? "On Stock" : "Low Stock";
            ViewPrice = $"₱{product.Price:N0}";
            ViewStock = product.StockQuanity.ToString();
            ViewDescription = product.Description;

            IsViewModalVisible = true;
        }

        private void CloseViewModal()
        {
            IsViewModalVisible = false;
        }
    }

    // ViewModel for Product display
    public class ProductViewModel : INotifyPropertyChanged
    {
        private int productID;
        private string name;
        private string sku;
        private string categoryName;
        private decimal price;
        private int stockQuanity;
        private bool isActive;
        private string description;
        private DateTime createdAt;

        public int ProductID
        {
            get { return productID; }
            set
            {
                productID = value;
                OnPropertyChanged(nameof(ProductID));
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string SKU
        {
            get { return sku; }
            set
            {
                sku = value;
                OnPropertyChanged(nameof(SKU));
            }
        }

        public string CategoryName
        {
            get { return categoryName; }
            set
            {
                categoryName = value;
                OnPropertyChanged(nameof(CategoryName));
            }
        }

        public decimal Price
        {
            get { return price; }
            set
            {
                price = value;
                OnPropertyChanged(nameof(Price));
            }
        }

        public int StockQuanity
        {
            get { return stockQuanity; }
            set
            {
                stockQuanity = value;
                OnPropertyChanged(nameof(StockQuanity));
            }
        }

        public bool IsActive
        {
            get { return isActive; }
            set
            {
                isActive = value;
                OnPropertyChanged(nameof(IsActive));
            }
        }

        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public DateTime CreatedAt
        {
            get { return createdAt; }
            set
            {
                createdAt = value;
                OnPropertyChanged(nameof(CreatedAt));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

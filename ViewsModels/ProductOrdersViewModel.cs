using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using gentech_services.MVVM;

namespace gentech_services.ViewsModels
{
    internal class ProductOrdersViewModel : ViewModelBase
    {
        private ObservableCollection<ProductCardViewModel> allProducts;
        private ObservableCollection<ProductCardViewModel> filteredProducts;
        private ObservableCollection<CartItemViewModel> cartItems;
        private int cartItemCounter;

        private string searchText;
        private string selectedCategoryFilter;
        private string customerName;
        private string customerEmail;
        private string customerPhone;
        private decimal totalAmount;
        private bool isCartEmpty;
        private string nameError;
        private string emailError;
        private string phoneError;

        public ObservableCollection<ProductCardViewModel> FilteredProducts
        {
            get { return filteredProducts; }
            set
            {
                filteredProducts = value;
                OnPropertyChanged(nameof(FilteredProducts));
            }
        }

        public ObservableCollection<CartItemViewModel> CartItems
        {
            get { return cartItems; }
            set
            {
                cartItems = value;
                OnPropertyChanged(nameof(CartItems));
            }
        }

        public string SearchText
        {
            get { return searchText; }
            set
            {
                searchText = value;
                OnPropertyChanged(nameof(SearchText));
                ApplyFilter();
            }
        }

        public string SelectedCategoryFilter
        {
            get { return selectedCategoryFilter; }
            set
            {
                selectedCategoryFilter = value;
                OnPropertyChanged(nameof(SelectedCategoryFilter));
                ApplyFilter();
            }
        }

        public string CustomerName
        {
            get { return customerName; }
            set
            {
                customerName = value;
                OnPropertyChanged(nameof(CustomerName));
                ValidateName();
                ProcessPaymentCommand.RaiseCanExecuteChanged();
            }
        }

        public string CustomerEmail
        {
            get { return customerEmail; }
            set
            {
                customerEmail = value;
                OnPropertyChanged(nameof(CustomerEmail));
                ValidateEmail();
                ProcessPaymentCommand.RaiseCanExecuteChanged();
            }
        }

        public string CustomerPhone
        {
            get { return customerPhone; }
            set
            {
                customerPhone = value;
                OnPropertyChanged(nameof(CustomerPhone));
                ValidatePhone();
                ProcessPaymentCommand.RaiseCanExecuteChanged();
            }
        }

        public string NameError
        {
            get { return nameError; }
            set
            {
                nameError = value;
                OnPropertyChanged(nameof(NameError));
            }
        }

        public string EmailError
        {
            get { return emailError; }
            set
            {
                emailError = value;
                OnPropertyChanged(nameof(EmailError));
            }
        }

        public string PhoneError
        {
            get { return phoneError; }
            set
            {
                phoneError = value;
                OnPropertyChanged(nameof(PhoneError));
            }
        }

        public decimal TotalAmount
        {
            get { return totalAmount; }
            set
            {
                totalAmount = value;
                OnPropertyChanged(nameof(TotalAmount));
            }
        }

        public bool IsCartEmpty
        {
            get { return isCartEmpty; }
            set
            {
                isCartEmpty = value;
                OnPropertyChanged(nameof(IsCartEmpty));
            }
        }

        // Commands
        public RelayCommand AddToCartCommand { get; private set; }
        public RelayCommand RemoveFromCartCommand { get; private set; }
        public RelayCommand IncreaseQuantityCommand { get; private set; }
        public RelayCommand DecreaseQuantityCommand { get; private set; }
        public RelayCommand ProcessPaymentCommand { get; private set; }
        public RelayCommand ClearCartCommand { get; private set; }

        public ProductOrdersViewModel()
        {
            AddToCartCommand = new RelayCommand(obj => AddToCart(obj as ProductCardViewModel));
            RemoveFromCartCommand = new RelayCommand(obj => RemoveFromCart(obj as CartItemViewModel));
            IncreaseQuantityCommand = new RelayCommand(obj => IncreaseQuantity(obj as CartItemViewModel));
            DecreaseQuantityCommand = new RelayCommand(obj => DecreaseQuantity(obj as CartItemViewModel));
            ProcessPaymentCommand = new RelayCommand(obj => ProcessPayment(), obj => CanProcessPayment());
            ClearCartCommand = new RelayCommand(obj => ClearCart());

            cartItems = new ObservableCollection<CartItemViewModel>();
            cartItemCounter = 0;
            IsCartEmpty = true;

            LoadProducts();
        }

        private void LoadProducts()
        {
            allProducts = new ObservableCollection<ProductCardViewModel>
            {
                new ProductCardViewModel
                {
                    ProductID = 1,
                    Name = "AMD Ryzen 5 5600",
                    CategoryName = "Parts",
                    Price = 8950m,
                    StockQuanity = 100,
                    StockStatus = "In stock"
                },
                new ProductCardViewModel
                {
                    ProductID = 2,
                    Name = "Intel Core i7-13700K",
                    CategoryName = "Parts",
                    Price = 21990m,
                    StockQuanity = 50,
                    StockStatus = "Low stock"
                },
                new ProductCardViewModel
                {
                    ProductID = 3,
                    Name = "NVIDIA RTX 4070",
                    CategoryName = "Parts",
                    Price = 35990m,
                    StockQuanity = 0,
                    StockStatus = "Out of stock"
                },
                new ProductCardViewModel
                {
                    ProductID = 4,
                    Name = "Corsair Vengeance DDR5",
                    CategoryName = "Parts",
                    Price = 6490m,
                    StockQuanity = 100,
                    StockStatus = "In stock"
                },
                new ProductCardViewModel
                {
                    ProductID = 5,
                    Name = "Samsung 980 Pro 1TB",
                    CategoryName = "Parts",
                    Price = 7990m,
                    StockQuanity = 75,
                    StockStatus = "In stock"
                },
                new ProductCardViewModel
                {
                    ProductID = 6,
                    Name = "Logitech G Pro X",
                    CategoryName = "Accessories",
                    Price = 5490m,
                    StockQuanity = 50,
                    StockStatus = "Low stock"
                },
                new ProductCardViewModel
                {
                    ProductID = 7,
                    Name = "Razer DeathAdder V3",
                    CategoryName = "Accessories",
                    Price = 3990m,
                    StockQuanity = 0,
                    StockStatus = "Out of stock"
                },
                new ProductCardViewModel
                {
                    ProductID = 8,
                    Name = "ASUS ROG Swift Monitor",
                    CategoryName = "Electronics",
                    Price = 28990m,
                    StockQuanity = 100,
                    StockStatus = "In stock"
                },
                new ProductCardViewModel
                {
                    ProductID = 9,
                    Name = "Secretlab Titan Chair",
                    CategoryName = "Accessories",
                    Price = 24990m,
                    StockQuanity = 15,
                    StockStatus = "In stock"
                },
                new ProductCardViewModel
                {
                    ProductID = 10,
                    Name = "Cooler Master H500",
                    CategoryName = "Parts",
                    Price = 8490m,
                    StockQuanity = 50,
                    StockStatus = "Low stock"
                },
                new ProductCardViewModel
                {
                    ProductID = 11,
                    Name = "NZXT Kraken Z73",
                    CategoryName = "Parts",
                    Price = 14990m,
                    StockQuanity = 30,
                    StockStatus = "In stock"
                },
                new ProductCardViewModel
                {
                    ProductID = 12,
                    Name = "Corsair RM850x PSU",
                    CategoryName = "Parts",
                    Price = 7990m,
                    StockQuanity = 100,
                    StockStatus = "In stock"
                },
                new ProductCardViewModel
                {
                    ProductID = 13,
                    Name = "Windows 11 Pro",
                    CategoryName = "Software",
                    Price = 10990m,
                    StockQuanity = 200,
                    StockStatus = "In stock"
                },
                new ProductCardViewModel
                {
                    ProductID = 14,
                    Name = "Office 365 Personal",
                    CategoryName = "Software",
                    Price = 3490m,
                    StockQuanity = 150,
                    StockStatus = "In stock"
                },
                new ProductCardViewModel
                {
                    ProductID = 15,
                    Name = "Lenovo Legion 5",
                    CategoryName = "Laptop",
                    Price = 54990m,
                    StockQuanity = 25,
                    StockStatus = "In stock"
                },
                new ProductCardViewModel
                {
                    ProductID = 16,
                    Name = "Dell XPS 15",
                    CategoryName = "Laptop",
                    Price = 84990m,
                    StockQuanity = 10,
                    StockStatus = "Low stock"
                }
            };

            FilteredProducts = new ObservableCollection<ProductCardViewModel>(allProducts);
        }

        private void ApplyFilter()
        {
            var filtered = allProducts.AsEnumerable();

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                string search = searchText.ToLower().Trim();
                filtered = filtered.Where(p =>
                    p.Name.ToLower().Contains(search) ||
                    p.CategoryName.ToLower().Contains(search));
            }

            // Apply category filter
            if (!string.IsNullOrWhiteSpace(selectedCategoryFilter) && selectedCategoryFilter != "All Categories")
            {
                filtered = filtered.Where(p => p.CategoryName == selectedCategoryFilter);
            }

            FilteredProducts = new ObservableCollection<ProductCardViewModel>(filtered);
        }

        private void AddToCart(ProductCardViewModel product)
        {
            if (product == null || product.StockStatus == "Out of stock") return;

            var existingItem = cartItems.FirstOrDefault(item => item.ProductID == product.ProductID);

            if (existingItem != null)
            {
                existingItem.Quantity++;
                existingItem.Subtotal = existingItem.Quantity * existingItem.UnitPrice;
            }
            else
            {
                cartItemCounter++;
                cartItems.Add(new CartItemViewModel
                {
                    ItemNumber = cartItemCounter,
                    ProductID = product.ProductID,
                    ProductName = product.Name,
                    Quantity = 1,
                    UnitPrice = product.Price,
                    Subtotal = product.Price
                });
            }

            UpdateTotal();
            IsCartEmpty = cartItems.Count == 0;
        }

        private void RemoveFromCart(CartItemViewModel item)
        {
            if (item != null)
            {
                cartItems.Remove(item);
                UpdateTotal();
                IsCartEmpty = cartItems.Count == 0;
            }
        }

        private void IncreaseQuantity(CartItemViewModel item)
        {
            if (item != null)
            {
                item.Quantity++;
                item.Subtotal = item.Quantity * item.UnitPrice;
                UpdateTotal();
            }
        }

        private void DecreaseQuantity(CartItemViewModel item)
        {
            if (item != null)
            {
                if (item.Quantity > 1)
                {
                    item.Quantity--;
                    item.Subtotal = item.Quantity * item.UnitPrice;
                    UpdateTotal();
                }
                else
                {
                    RemoveFromCart(item);
                }
            }
        }

        private void UpdateTotal()
        {
            TotalAmount = cartItems.Sum(item => item.Subtotal);
        }

        private bool CanProcessPayment()
        {
            return cartItems.Count > 0 &&
                   !string.IsNullOrWhiteSpace(customerName) &&
                   !string.IsNullOrWhiteSpace(customerEmail) &&
                   !string.IsNullOrWhiteSpace(customerPhone) &&
                   string.IsNullOrEmpty(nameError) &&
                   string.IsNullOrEmpty(emailError) &&
                   string.IsNullOrEmpty(phoneError);
        }

        private void ValidateName()
        {
            if (string.IsNullOrWhiteSpace(customerName))
            {
                NameError = "Name is required";
            }
            else if (customerName.Length < 2)
            {
                NameError = "Name must be at least 2 characters";
            }
            else
            {
                NameError = string.Empty;
            }
        }

        private void ValidateEmail()
        {
            if (string.IsNullOrWhiteSpace(customerEmail))
            {
                EmailError = "Email is required";
            }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(customerEmail,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                EmailError = "Invalid email format";
            }
            else
            {
                EmailError = string.Empty;
            }
        }

        private void ValidatePhone()
        {
            if (string.IsNullOrWhiteSpace(customerPhone))
            {
                PhoneError = "Phone number is required";
            }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(customerPhone,
                 @"^(?:\+63|63|0)(?:9\d{9}|(2|[3-8]\d)\d{7})$"))
            {
                PhoneError = "Invalid phone number format";
            }
            else if (customerPhone.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace("+", "").Length < 10)
            {
                PhoneError = "Phone number must be at least 10 digits";
            }
            else
            {
                PhoneError = string.Empty;
            }
        }

        private void ProcessPayment()
        {
            decimal total = cartItems.Sum(item => item.Subtotal);
            int itemCount = cartItems.Sum(item => item.Quantity);

            var result = MessageBox.Show(
                $"Process payment for:\n\n" +
                $"Customer: {CustomerName}\n" +
                $"Items: {itemCount}\n" +
                $"Total: ₱{total:N2}\n\n" +
                $"Continue with payment?",
                "Process Payment",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                MessageBox.Show(
                    $"Payment processed successfully!\n\n" +
                    $"Order ID: ORD-{DateTime.Now:yyyyMMddHHmmss}\n" +
                    $"Total: ₱{total:N2}",
                    "Success",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                ClearCart();
            }
        }

        private void CancelOrder()
        {
            if (cartItems.Count == 0) return;

            var result = MessageBox.Show(
                "Are you sure you want to cancel this order?\n\nAll items in the cart will be removed.",
                "Cancel Order",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                ClearCart();
            }
        }

        private void ClearCart()
        {
            cartItems.Clear();
            CustomerName = string.Empty;
            CustomerEmail = string.Empty;
            CustomerPhone = string.Empty;
            cartItemCounter = 0;
            UpdateTotal();
            IsCartEmpty = true;
        }
    }

    // ViewModels
    public class ProductCardViewModel : INotifyPropertyChanged
    {
        private int productID;
        private string name;
        private string categoryName;
        private decimal price;
        private int stockQuanity;
        private string stockStatus;

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

        public string StockStatus
        {
            get { return stockStatus; }
            set
            {
                stockStatus = value;
                OnPropertyChanged(nameof(StockStatus));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class CartItemViewModel : INotifyPropertyChanged
    {
        private int itemNumber;
        private int productID;
        private string productName;
        private int quantity;
        private decimal unitPrice;
        private decimal subtotal;

        public int ItemNumber
        {
            get { return itemNumber; }
            set
            {
                itemNumber = value;
                OnPropertyChanged(nameof(ItemNumber));
            }
        }

        public int ProductID
        {
            get { return productID; }
            set
            {
                productID = value;
                OnPropertyChanged(nameof(ProductID));
            }
        }

        public string ProductName
        {
            get { return productName; }
            set
            {
                productName = value;
                OnPropertyChanged(nameof(ProductName));
            }
        }

        public int Quantity
        {
            get { return quantity; }
            set
            {
                quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }

        public decimal UnitPrice
        {
            get { return unitPrice; }
            set
            {
                unitPrice = value;
                OnPropertyChanged(nameof(UnitPrice));
            }
        }

        public decimal Subtotal
        {
            get { return subtotal; }
            set
            {
                subtotal = value;
                OnPropertyChanged(nameof(Subtotal));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

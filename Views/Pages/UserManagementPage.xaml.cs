using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using gentech_services.Models;

namespace gentech_services.Views.Pages
{
    public partial class UserManagementPage : UserControl
    {
        public ObservableCollection<User> Users { get; set; }
        private ObservableCollection<User> allUsers;
        private User selectedUser;

        public string CurrentUserName { get; set; } = "Admin/Owner";
        public string CurrentUserRole { get; set; } = "Admin";

        public UserManagementPage()
        {
            InitializeComponent();
            LoadSampleData();
            DataContext = this;
        }

        private void LoadSampleData()
        {
            Users = new ObservableCollection<User>
            {
                new User
                {
                    UserID = 1,
                    Name = "Store Admin",
                    Username = "Admin",
                    PIN = "1234",
                    Role = "Admin",
                    IsActive = true,
                    Email = "admin@gentech.com",
                    PasswordHash = "hashed_password",
                    CreatedAt = DateTime.Now.AddMonths(-6)
                },
                new User
                {
                    UserID = 2,
                    Name = "Aldous",
                    Username = "Admin",
                    PIN = "1234",
                    Role = "Manager",
                    IsActive = true,
                    Email = "aldous@gentech.com",
                    PasswordHash = "hashed_password",
                    CreatedAt = DateTime.Now.AddMonths(-5)
                },
                new User
                {
                    UserID = 3,
                    Name = "Fanny",
                    Username = "Admin",
                    PIN = "1234",
                    Role = "Cashier",
                    IsActive = true,
                    Email = "fanny@gentech.com",
                    PasswordHash = "hashed_password",
                    CreatedAt = DateTime.Now.AddMonths(-4)
                },
                new User
                {
                    UserID = 4,
                    Name = "Bogart",
                    Username = "Admin",
                    PIN = "1234",
                    Role = "Staff",
                    IsActive = true,
                    Email = "bogart@gentech.com",
                    PasswordHash = "hashed_password",
                    CreatedAt = DateTime.Now.AddMonths(-3)
                },
                new User
                {
                    UserID = 5,
                    Name = "Roronoa Zoro",
                    Username = "Admin",
                    PIN = "1234",
                    Role = "Staff",
                    IsActive = true,
                    Email = "roronoa@gentech.com",
                    PasswordHash = "hashed_password",
                    CreatedAt = DateTime.Now.AddMonths(-2)
                },
                new User
                {
                    UserID = 6,
                    Name = "Emith",
                    Username = "Admin",
                    PIN = "1234",
                    Role = "Staff",
                    IsActive = true,
                    Email = "emith@gentech.com",
                    PasswordHash = "hashed_password",
                    CreatedAt = DateTime.Now.AddMonths(-1)
                },
                new User
                {
                    UserID = 7,
                    Name = "John Doe",
                    Username = "johndoe",
                    PIN = "5678",
                    Role = "Staff",
                    IsActive = true,
                    Email = "johndoe@gentech.com",
                    PasswordHash = "hashed_password",
                    CreatedAt = DateTime.Now.AddDays(-20)
                },
                new User
                {
                    UserID = 8,
                    Name = "Jane Smith",
                    Username = "janesmith",
                    PIN = "9012",
                    Role = "Cashier",
                    IsActive = false,
                    Email = "janesmith@gentech.com",
                    PasswordHash = "hashed_password",
                    CreatedAt = DateTime.Now.AddDays(-18)
                },
                new User
                {
                    UserID = 9,
                    Name = "Michael Johnson",
                    Username = "mjohnson",
                    PIN = "3456",
                    Role = "Staff",
                    IsActive = true,
                    Email = "mjohnson@gentech.com",
                    PasswordHash = "hashed_password",
                    CreatedAt = DateTime.Now.AddDays(-15)
                },
                new User
                {
                    UserID = 10,
                    Name = "Sarah Williams",
                    Username = "swilliams",
                    PIN = "7890",
                    Role = "Manager",
                    IsActive = true,
                    Email = "swilliams@gentech.com",
                    PasswordHash = "hashed_password",
                    CreatedAt = DateTime.Now.AddDays(-12)
                },
                new User
                {
                    UserID = 11,
                    Name = "David Brown",
                    Username = "dbrown",
                    PIN = "2345",
                    Role = "Staff",
                    IsActive = false,
                    Email = "dbrown@gentech.com",
                    PasswordHash = "hashed_password",
                    CreatedAt = DateTime.Now.AddDays(-10)
                },
                new User
                {
                    UserID = 12,
                    Name = "Emily Davis",
                    Username = "edavis",
                    PIN = "6789",
                    Role = "Cashier",
                    IsActive = true,
                    Email = "edavis@gentech.com",
                    PasswordHash = "hashed_password",
                    CreatedAt = DateTime.Now.AddDays(-8)
                },
                new User
                {
                    UserID = 13,
                    Name = "Robert Miller",
                    Username = "rmiller",
                    PIN = "1357",
                    Role = "Staff",
                    IsActive = true,
                    Email = "rmiller@gentech.com",
                    PasswordHash = "hashed_password",
                    CreatedAt = DateTime.Now.AddDays(-6)
                },
                new User
                {
                    UserID = 14,
                    Name = "Lisa Anderson",
                    Username = "landerson",
                    PIN = "2468",
                    Role = "Staff",
                    IsActive = true,
                    Email = "landerson@gentech.com",
                    PasswordHash = "hashed_password",
                    CreatedAt = DateTime.Now.AddDays(-4)
                },
                new User
                {
                    UserID = 15,
                    Name = "James Wilson",
                    Username = "jwilson",
                    PIN = "9876",
                    Role = "Manager",
                    IsActive = false,
                    Email = "jwilson@gentech.com",
                    PasswordHash = "hashed_password",
                    CreatedAt = DateTime.Now.AddDays(-2)
                }
            };

            allUsers = new ObservableCollection<User>(Users);
            UsersItemsControl.ItemsSource = Users;
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchTextBox.Text.ToLower().Trim();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                UsersItemsControl.ItemsSource = allUsers;
            }
            else
            {
                var filtered = allUsers.Where(u =>
                    u.UserID.ToString().Contains(searchText) ||
                    u.Name.ToLower().Contains(searchText) ||
                    u.Username.ToLower().Contains(searchText) ||
                    u.Role.ToLower().Contains(searchText)
                ).ToList();

                UsersItemsControl.ItemsSource = new ObservableCollection<User>(filtered);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(FullNameTextBox.Text))
            {
                MessageBox.Show("Please enter a full name.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(UsernameTextBox.Text))
            {
                MessageBox.Show("Please enter a username.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(PINTextBox.Text) || PINTextBox.Text.Length < 4)
            {
                MessageBox.Show("Please enter a PIN with at least 4 digits.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(PINTextBox.Text, out _))
            {
                MessageBox.Show("PIN must be numeric.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Get next UserID
            int nextID = allUsers.Count > 0 ? allUsers.Max(u => u.UserID) + 1 : 1;

            // Create new user
            var newUser = new User
            {
                UserID = nextID,
                Name = FullNameTextBox.Text.Trim(),
                Username = UsernameTextBox.Text.Trim(),
                PIN = PINTextBox.Text.Trim(),
                Role = (RoleComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Staff",
                IsActive = true,
                Email = $"{UsernameTextBox.Text.Trim().ToLower()}@gentech.com",
                PasswordHash = "hashed_password",
                CreatedAt = DateTime.Now
            };

            // Add to collections
            allUsers.Add(newUser);
            Users.Add(newUser);

            // Refresh the display
            UsersItemsControl.ItemsSource = null;
            UsersItemsControl.ItemsSource = allUsers;

            // Clear form
            ClearForm();

            MessageBox.Show($"User '{newUser.Name}' has been added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            FullNameTextBox.Text = string.Empty;
            UsernameTextBox.Text = string.Empty;
            PINTextBox.Text = string.Empty;
            RoleComboBox.SelectedIndex = 3; // Default to Staff
        }

        private void UserActionButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            selectedUser = button?.DataContext as User;

            if (selectedUser == null) return;

            // Position and show popup
            UserActionPopup.PlacementTarget = button;
            UserActionPopup.IsOpen = true;
        }

        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            UserActionPopup.IsOpen = false;

            if (selectedUser == null) return;

            // Populate edit form
            EditFullNameTextBox.Text = selectedUser.Name;
            EditUsernameTextBox.Text = selectedUser.Username;
            EditPINTextBox.Text = selectedUser.PIN;

            // Set role combo box
            for (int i = 0; i < EditRoleComboBox.Items.Count; i++)
            {
                if ((EditRoleComboBox.Items[i] as ComboBoxItem)?.Content.ToString() == selectedUser.Role)
                {
                    EditRoleComboBox.SelectedIndex = i;
                    break;
                }
            }

            // Show edit modal
            EditModalOverlay.Visibility = Visibility.Visible;
        }

        private void DeactivateUser_Click(object sender, RoutedEventArgs e)
        {
            UserActionPopup.IsOpen = false;

            if (selectedUser == null) return;

            string action = selectedUser.IsActive ? "deactivate" : "activate";
            string actionCaps = selectedUser.IsActive ? "Deactivate" : "Activate";

            var result = MessageBox.Show(
                $"Are you sure you want to {action} user '{selectedUser.Name}'?",
                $"{actionCaps} User",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                selectedUser.IsActive = !selectedUser.IsActive;

                // Refresh display
                RefreshUsersList();

                MessageBox.Show(
                    $"User '{selectedUser.Name}' has been {action}d successfully.",
                    "Success",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            UserActionPopup.IsOpen = false;

            if (selectedUser == null) return;

            var result = MessageBox.Show(
                $"Are you sure you want to delete user '{selectedUser.Name}'?\n\nThis action cannot be undone.",
                "Delete User",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                allUsers.Remove(selectedUser);
                Users.Remove(selectedUser);

                // Refresh display
                RefreshUsersList();

                MessageBox.Show(
                    $"User '{selectedUser.Name}' has been deleted successfully.",
                    "Success",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

        private void CancelEditButton_Click(object sender, RoutedEventArgs e)
        {
            EditModalOverlay.Visibility = Visibility.Collapsed;
            ClearEditForm();
        }

        private void SaveEditButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedUser == null) return;

            // Validate inputs
            if (string.IsNullOrWhiteSpace(EditFullNameTextBox.Text))
            {
                MessageBox.Show("Please enter a full name.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(EditUsernameTextBox.Text))
            {
                MessageBox.Show("Please enter a username.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(EditPINTextBox.Text) || EditPINTextBox.Text.Length < 4)
            {
                MessageBox.Show("Please enter a PIN with at least 4 digits.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(EditPINTextBox.Text, out _))
            {
                MessageBox.Show("PIN must be numeric.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Update user
            selectedUser.Name = EditFullNameTextBox.Text.Trim();
            selectedUser.Username = EditUsernameTextBox.Text.Trim();
            selectedUser.PIN = EditPINTextBox.Text.Trim();
            selectedUser.Role = (EditRoleComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? selectedUser.Role;

            // Refresh display
            RefreshUsersList();

            // Close modal
            EditModalOverlay.Visibility = Visibility.Collapsed;
            ClearEditForm();

            MessageBox.Show(
                $"User '{selectedUser.Name}' has been updated successfully.",
                "Success",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void ClearEditForm()
        {
            EditFullNameTextBox.Text = string.Empty;
            EditUsernameTextBox.Text = string.Empty;
            EditPINTextBox.Text = string.Empty;
            EditRoleComboBox.SelectedIndex = 0;
        }

        private void RefreshUsersList()
        {
            var currentSource = UsersItemsControl.ItemsSource;
            UsersItemsControl.ItemsSource = null;
            UsersItemsControl.ItemsSource = currentSource;
        }
    }
}

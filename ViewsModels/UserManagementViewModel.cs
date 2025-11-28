using gentech_services.Models;
using gentech_services.MVVM;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace gentech_services.ViewsModels
{
    internal class UserManagementViewModel : ViewModelBase
    {
        private ObservableCollection<User> users;
        private ObservableCollection<User> allUsers;
        private User selectedUser;
        private string searchText;
        private string currentUserName;
        private string currentUserRole;

        // Add User Form Properties
        private string fullName;
        private string username;
        private string pin;
        private string selectedRole;

        // Edit User Properties
        private string editFullName;
        private string editUsername;
        private string editPin;
        private string editRole;

        public ObservableCollection<User> Users
        {
            get { return users; }
            set { users = value; OnPropertyChanged(); }
        }

        public User SelectedUser
        {
            get { return selectedUser; }
            set { selectedUser = value; OnPropertyChanged(); }
        }

        public string SearchText
        {
            get { return searchText; }
            set
            {
                searchText = value;
                OnPropertyChanged();
                ApplySearch();
            }
        }

        public string CurrentUserName
        {
            get { return currentUserName; }
            set { currentUserName = value; OnPropertyChanged(); }
        }

        public string CurrentUserRole
        {
            get { return currentUserRole; }
            set { currentUserRole = value; OnPropertyChanged(); }
        }

        public string FullName
        {
            get { return fullName; }
            set { fullName = value; OnPropertyChanged(); }
        }

        public string Username
        {
            get { return username; }
            set { username = value; OnPropertyChanged(); }
        }

        public string PIN
        {
            get { return pin; }
            set { pin = value; OnPropertyChanged(); }
        }

        public string SelectedRole
        {
            get { return selectedRole; }
            set { selectedRole = value; OnPropertyChanged(); }
        }

        public string EditFullName
        {
            get { return editFullName; }
            set { editFullName = value; OnPropertyChanged(); }
        }

        public string EditUsername
        {
            get { return editUsername; }
            set { editUsername = value; OnPropertyChanged(); }
        }

        public string EditPIN
        {
            get { return editPin; }
            set { editPin = value; OnPropertyChanged(); }
        }

        public string EditRole
        {
            get { return editRole; }
            set { editRole = value; OnPropertyChanged(); }
        }

        public RelayCommand AddUserCommand { get; private set; }
        public RelayCommand EditUserCommand { get; private set; }
        public RelayCommand SaveEditCommand { get; private set; }
        public RelayCommand CancelAddCommand { get; private set; }
        public RelayCommand CancelEditCommand { get; private set; }
        public RelayCommand ToggleActiveCommand { get; private set; }
        public RelayCommand DeleteUserCommand { get; private set; }

        public UserManagementViewModel()
        {
            users = new ObservableCollection<User>();
            allUsers = new ObservableCollection<User>();
            currentUserName = "Admin/Owner";
            currentUserRole = "Admin";
            selectedRole = "Staff";

            AddUserCommand = new RelayCommand(obj => AddUser(), obj => CanAddUser());
            SaveEditCommand = new RelayCommand(obj => SaveEdit(), obj => CanSaveEdit());
            CancelAddCommand = new RelayCommand(obj => ClearAddForm());
            CancelEditCommand = new RelayCommand(obj => ClearEditForm());
            ToggleActiveCommand = new RelayCommand(obj => ToggleActive(obj as User));
            DeleteUserCommand = new RelayCommand(obj => DeleteUser(obj as User));

            LoadSampleData();
        }

        private void LoadSampleData()
        {
            var sampleUsers = new[]
            {
                new User { UserID = 1, Name = "Store Admin", Username = "Admin", PIN = "1234", Role = "Admin", IsActive = true, Email = "admin@gentech.com", PasswordHash = "hashed_password", CreatedAt = DateTime.Now.AddMonths(-6) },
                new User { UserID = 2, Name = "Aldous", Username = "Admin", PIN = "1234", Role = "Manager", IsActive = true, Email = "aldous@gentech.com", PasswordHash = "hashed_password", CreatedAt = DateTime.Now.AddMonths(-5) },
                new User { UserID = 3, Name = "Fanny", Username = "Admin", PIN = "1234", Role = "Cashier", IsActive = true, Email = "fanny@gentech.com", PasswordHash = "hashed_password", CreatedAt = DateTime.Now.AddMonths(-4) },
                new User { UserID = 4, Name = "Bogart", Username = "Admin", PIN = "1234", Role = "Staff", IsActive = true, Email = "bogart@gentech.com", PasswordHash = "hashed_password", CreatedAt = DateTime.Now.AddMonths(-3) },
                new User { UserID = 5, Name = "Roronoa Zoro", Username = "Admin", PIN = "1234", Role = "Staff", IsActive = true, Email = "roronoa@gentech.com", PasswordHash = "hashed_password", CreatedAt = DateTime.Now.AddMonths(-2) },
                new User { UserID = 6, Name = "Emith", Username = "Admin", PIN = "1234", Role = "Staff", IsActive = true, Email = "emith@gentech.com", PasswordHash = "hashed_password", CreatedAt = DateTime.Now.AddMonths(-1) },
                new User { UserID = 7, Name = "John Doe", Username = "johndoe", PIN = "5678", Role = "Staff", IsActive = true, Email = "johndoe@gentech.com", PasswordHash = "hashed_password", CreatedAt = DateTime.Now.AddDays(-20) },
                new User { UserID = 8, Name = "Jane Smith", Username = "janesmith", PIN = "9012", Role = "Cashier", IsActive = false, Email = "janesmith@gentech.com", PasswordHash = "hashed_password", CreatedAt = DateTime.Now.AddDays(-18) },
                new User { UserID = 9, Name = "Michael Johnson", Username = "mjohnson", PIN = "3456", Role = "Staff", IsActive = true, Email = "mjohnson@gentech.com", PasswordHash = "hashed_password", CreatedAt = DateTime.Now.AddDays(-15) },
                new User { UserID = 10, Name = "Sarah Williams", Username = "swilliams", PIN = "7890", Role = "Manager", IsActive = true, Email = "swilliams@gentech.com", PasswordHash = "hashed_password", CreatedAt = DateTime.Now.AddDays(-12) },
                new User { UserID = 11, Name = "David Brown", Username = "dbrown", PIN = "2345", Role = "Staff", IsActive = false, Email = "dbrown@gentech.com", PasswordHash = "hashed_password", CreatedAt = DateTime.Now.AddDays(-10) },
                new User { UserID = 12, Name = "Emily Davis", Username = "edavis", PIN = "6789", Role = "Cashier", IsActive = true, Email = "edavis@gentech.com", PasswordHash = "hashed_password", CreatedAt = DateTime.Now.AddDays(-8) },
                new User { UserID = 13, Name = "Robert Miller", Username = "rmiller", PIN = "1357", Role = "Staff", IsActive = true, Email = "rmiller@gentech.com", PasswordHash = "hashed_password", CreatedAt = DateTime.Now.AddDays(-6) },
                new User { UserID = 14, Name = "Lisa Anderson", Username = "landerson", PIN = "2468", Role = "Staff", IsActive = true, Email = "landerson@gentech.com", PasswordHash = "hashed_password", CreatedAt = DateTime.Now.AddDays(-4) },
                new User { UserID = 15, Name = "James Wilson", Username = "jwilson", PIN = "9876", Role = "Manager", IsActive = false, Email = "jwilson@gentech.com", PasswordHash = "hashed_password", CreatedAt = DateTime.Now.AddDays(-2) }
            };

            foreach (var user in sampleUsers)
            {
                allUsers.Add(user);
                users.Add(user);
            }
        }

        private void ApplySearch()
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                Users = new ObservableCollection<User>(allUsers);
            }
            else
            {
                var filtered = allUsers.Where(u =>
                    u.UserID.ToString().Contains(searchText.ToLower()) ||
                    u.Name.ToLower().Contains(searchText.ToLower()) ||
                    u.Username.ToLower().Contains(searchText.ToLower()) ||
                    u.Role.ToLower().Contains(searchText.ToLower())
                ).ToList();

                Users = new ObservableCollection<User>(filtered);
            }
        }

        private bool CanAddUser()
        {
            return !string.IsNullOrWhiteSpace(FullName) &&
                   !string.IsNullOrWhiteSpace(Username) &&
                   !string.IsNullOrWhiteSpace(PIN) &&
                   PIN.Length >= 4;
        }

        private void AddUser()
        {
            if (!int.TryParse(PIN, out _))
            {
                MessageBox.Show("PIN must be numeric.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int nextID = allUsers.Count > 0 ? allUsers.Max(u => u.UserID) + 1 : 1;

            var newUser = new User
            {
                UserID = nextID,
                Name = FullName.Trim(),
                Username = Username.Trim(),
                PIN = PIN.Trim(),
                Role = SelectedRole ?? "Staff",
                IsActive = true,
                Email = $"{Username.Trim().ToLower()}@gentech.com",
                PasswordHash = "hashed_password",
                CreatedAt = DateTime.Now
            };

            allUsers.Add(newUser);
            ApplySearch();

            MessageBox.Show($"User '{newUser.Name}' has been added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            ClearAddForm();
        }

        private void ClearAddForm()
        {
            FullName = string.Empty;
            Username = string.Empty;
            PIN = string.Empty;
            SelectedRole = "Staff";
        }

        private bool CanSaveEdit()
        {
            return selectedUser != null &&
                   !string.IsNullOrWhiteSpace(EditFullName) &&
                   !string.IsNullOrWhiteSpace(EditUsername) &&
                   !string.IsNullOrWhiteSpace(EditPIN) &&
                   EditPIN.Length >= 4;
        }

        private void SaveEdit()
        {
            if (selectedUser == null) return;

            if (!int.TryParse(EditPIN, out _))
            {
                MessageBox.Show("PIN must be numeric.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            selectedUser.Name = EditFullName.Trim();
            selectedUser.Username = EditUsername.Trim();
            selectedUser.PIN = EditPIN.Trim();
            selectedUser.Role = EditRole ?? selectedUser.Role;

            // Refresh the collection
            ApplySearch();

            MessageBox.Show($"User '{selectedUser.Name}' has been updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            ClearEditForm();
        }

        private void ClearEditForm()
        {
            EditFullName = string.Empty;
            EditUsername = string.Empty;
            EditPIN = string.Empty;
            EditRole = string.Empty;
        }

        public void PopulateEditForm(User user)
        {
            SelectedUser = user;
            EditFullName = user.Name;
            EditUsername = user.Username;
            EditPIN = user.PIN;
            EditRole = user.Role;
        }

        private void ToggleActive(User user)
        {
            if (user == null) return;

            string action = user.IsActive ? "deactivate" : "activate";
            string actionCaps = user.IsActive ? "Deactivate" : "Activate";

            var result = MessageBox.Show(
                $"Are you sure you want to {action} user '{user.Name}'?",
                $"{actionCaps} User",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                user.IsActive = !user.IsActive;

                // Force UI refresh
                var index = users.IndexOf(user);
                if (index >= 0)
                {
                    users.RemoveAt(index);
                    users.Insert(index, user);
                }

                MessageBox.Show(
                    $"User '{user.Name}' has been {action}d successfully.",
                    "Success",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

        private void DeleteUser(User user)
        {
            if (user == null) return;

            var result = MessageBox.Show(
                $"Are you sure you want to delete user '{user.Name}'?\n\nThis action cannot be undone.",
                "Delete User",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                allUsers.Remove(user);
                users.Remove(user);

                MessageBox.Show(
                    $"User '{user.Name}' has been deleted successfully.",
                    "Success",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }
    }
}

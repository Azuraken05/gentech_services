using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace gentech_services.Views.UserControls
{
    /// <summary>
    /// Interaction logic for EditUser.xaml
    /// </summary>
    public partial class EditUser : UserControl
    {
        public EditUser()
        {
            InitializeComponent();
        }
        private static readonly Regex _nonDigit = new Regex(@"\D", RegexOptions.Compiled);

        

        private void Pin_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = _nonDigit.IsMatch(e.Text);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            //var vm = DataContext as EditUserViewModel;
            //MessageBox.Show($"Saved:\n{}\n{vm.Username}\nRole: {vm.SelectedRole}\nPIN: {vm.Pin}", "Saved");
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Cancelled", "Cancel");
        }
    }
}

using gentech_services.Models;
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

namespace gentech_services.Views.UserControls
{
    /// <summary>
    /// Interaction logic for ProductManagementTable.xaml
    /// </summary>
    public partial class ProductManagementTable : UserControl
    {

        public static readonly DependencyProperty ProductsProperty =
            DependencyProperty.Register("Products",
                typeof(ObservableCollection<Product>),
                typeof(ProductManagementTable),
                new PropertyMetadata(null));

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel",
                typeof(object),
                typeof(ProductManagementTable),
                new PropertyMetadata(null, OnViewModelChanged));

        private static void OnViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ProductManagementTable control)
            {
                control.DataContext = e.NewValue;
            }
        }

        public ObservableCollection<Product> Products
        {
            get { return (ObservableCollection<Product>)GetValue(ProductsProperty); }
            set { SetValue(ProductsProperty, value); }
        }

        public object ViewModel
        {
            get { return GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public ProductManagementTable()
        {
            InitializeComponent();
        }
        private void KebabMenu_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button?.ContextMenu != null)
            {
                button.ContextMenu.PlacementTarget = button;
                button.ContextMenu.DataContext = this.DataContext;
                button.ContextMenu.Tag = button.Tag; // Store the Product
                button.ContextMenu.IsOpen = true;
            }
        }

        private void ViewMenuItem_Click(object sender, MouseButtonEventArgs e)
        {
            var contextMenu = FindContextMenu(sender);
            if (contextMenu?.Tag is Product order)
            {
                dynamic vm = DataContext;
                vm.ViewDetailsCommand?.Execute(order);
                contextMenu.IsOpen = false;
            }
        }

        private void EditMenuItem_Click(object sender, MouseButtonEventArgs e)
        {
            var contextMenu = FindContextMenu(sender);
            if (contextMenu?.Tag is Product order)
            {
                dynamic vm = DataContext;
                vm.EditCommand?.Execute(order);
                contextMenu.IsOpen = false;
            }
        }

        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            var contextMenu = FindContextMenu(sender);
            if (contextMenu?.Tag is Product order)
            {
                dynamic vm = DataContext;
                vm.CancelAppointmentCommand?.Execute(order);
                contextMenu.IsOpen = false;
            }
        }

        private ContextMenu FindContextMenu(object sender)
        {
            DependencyObject element = sender as DependencyObject;
            while (element != null)
            {
                if (element is ContextMenu contextMenu)
                    return contextMenu;
                element = System.Windows.Media.VisualTreeHelper.GetParent(element);
            }
            return null;
        }

        private void MenuItem_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Border border)
            {
                border.Background = new System.Windows.Media.SolidColorBrush(
                    (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#EBEBEB"));
            }
        }

        private void MenuItem_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Border border)
            {
                border.Background = System.Windows.Media.Brushes.Transparent;
            }
        }
    }
}

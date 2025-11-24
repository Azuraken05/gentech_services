using System;
using System.Collections.Generic;
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
    /// Interaction logic for LowStockAlert.xaml
    /// </summary>
    public partial class LowStockAlert : UserControl
    {
        private string productName;
        private string quantity;

        public string ProductName { get;set; }
        public string Quantity { get;set; }
        public LowStockAlert()
        {
            InitializeComponent();
        }
    }
}

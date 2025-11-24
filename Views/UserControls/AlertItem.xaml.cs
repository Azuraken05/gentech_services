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
    /// Interaction logic for AlertItem.xaml
    /// </summary>
    public partial class AlertItem : UserControl
    {


        public AlertItem()
        {
            InitializeComponent();
        }
        public static readonly DependencyProperty TextNameProperty =
           DependencyProperty.Register("TextName", typeof(string), typeof(AlertItem));
        public string TextName
        {
            get { return (string)GetValue(TextNameProperty); }
            set { SetValue(TextNameProperty, value); }
        }

        public static readonly DependencyProperty TextStockProperty =
          DependencyProperty.Register("TextStock", typeof(int), typeof(AlertItem));

        public int TextStock
        {
            get { return (int)GetValue(TextStockProperty); }
            set { SetValue(TextStockProperty, value); }
        }
    }
}

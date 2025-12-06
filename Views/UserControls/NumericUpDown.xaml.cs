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
    /// Interaction logic for NumericUpDown.xaml
    /// </summary>
    public partial class NumericUpDown : UserControl
    {
        
        public NumericUpDown()
        {
            InitializeComponent();
            
        }

        public int Value
        {
            get => int.Parse(itemQty.Text);
            set => itemQty.Text = value.ToString();
        }

        private void Minus_Click(object sender, RoutedEventArgs e)
        {
            if(Value >1) Value--;
        }

        private void Plus_Click(object sender, RoutedEventArgs e)
        {
            Value++;
        }

        private void itemQty_Loaded(object sender, RoutedEventArgs e)
        {
            itemQty.Text = "0";
        }
    }
}

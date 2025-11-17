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
    /// Interaction logic for RichTextBox.xaml
    /// </summary>
    /// 

    public partial class RichTextBox : UserControl
    {
        public static DependencyProperty PlaceholderProperty = DependencyProperty.Register("Placeholder",
    typeof(string), typeof(RichTextBox), new PropertyMetadata("Default Placeholder"));

        public static DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(string), typeof(RichTextBox), 
            new PropertyMetadata(null));

        public string Placeholder
        {
            get { return (string) GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        public string Value
        {
            get { return (string)(GetValue(ValueProperty)); }
            set { SetValue(PlaceholderProperty, value); }
        }
        public RichTextBox()
        {
            InitializeComponent();
        }
    }
}

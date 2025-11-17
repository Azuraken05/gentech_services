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
    /// Interaction logic for Card.xaml
    /// </summary>
    public partial class Card : UserControl
    {
        public Card()
        {
            InitializeComponent();
            BorderContainer.MouseLeftButtonUp += (s, e) =>
            {
                RaiseEvent(new RoutedEventArgs(ClickEvent));
                IsSelected = !IsSelected;
            };
        }

        public static readonly DependencyProperty TextNameProperty =
            DependencyProperty.Register("TextName", typeof(string), typeof(Card));

        public string TextName
        {
            get {  return (string)GetValue(TextNameProperty);}
            set { SetValue(TextNameProperty, value); }
        }
        
        public static readonly DependencyProperty TextCategoryProperty =
           DependencyProperty.Register("TextCategory", typeof(string), typeof(Card));

        public string TextCategory
        {
            get { return (string)GetValue(TextCategoryProperty); }
            set { SetValue(TextCategoryProperty, value); }
        }

        public static readonly DependencyProperty TextPriceProperty =
           DependencyProperty.Register("TextPrice", typeof(string), typeof(Card));

        public string TextPrice
        {
            get { return (string)GetValue(TextPriceProperty); }
            set { SetValue(TextPriceProperty, value); }
        }

        
        // click event
        public static readonly RoutedEvent ClickEvent =
            EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Card));

        public event RoutedEventHandler Click
        {
            add => AddHandler(ClickEvent, value);
            remove => RemoveHandler(ClickEvent, value);
        }

        //select event
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(Card), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault) );

        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }
    }
}

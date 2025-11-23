using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace gentech_services.Converters
{
    public class StatusToSetOngoingVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Only show "Set to Ongoing" for Pending orders
            if (value is string status && status == "Pending")
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

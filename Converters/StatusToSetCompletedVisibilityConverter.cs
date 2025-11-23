using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace gentech_services.Converters
{
    public class StatusToSetCompletedVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Only show "Set to Completed" for Ongoing orders
            if (value is string status && status == "Ongoing")
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

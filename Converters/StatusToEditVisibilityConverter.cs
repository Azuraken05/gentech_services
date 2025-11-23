using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace gentech_services.Converters
{
    public class StatusToEditVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string status && (status == "Completed" || status == "Cancelled"))
            {
                return Visibility.Collapsed;
            }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace gentech_services.Converters
{
    public class BoolToStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isActive)
            {
                if (parameter?.ToString() == "Text")
                {
                    return isActive ? "Active" : "Inactive";
                }
                else if (parameter?.ToString() == "Background")
                {
                    return isActive ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8F5E9"))
                                    : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFEBEE"));
                }
                else if (parameter?.ToString() == "Foreground")
                {
                    return isActive ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4CAF50"))
                                    : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F44336"));
                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace gentech_services.Converters
{
    public class TagToCornerRadiusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return new CornerRadius(0);

            if (value is string str && double.TryParse(str, out double radius))
                return new CornerRadius(radius);

            if (value is double d)
                return new CornerRadius(d);

            if (value is int i)
                return new CornerRadius(i);

            return new CornerRadius(0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

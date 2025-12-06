using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;


namespace gentech_services.Views.Controls
{
    class IconBtn : Button
    {
        // Icon property

        public static readonly DependencyProperty IconImgProperty =
            DependencyProperty.Register(
                nameof(IconImg),
                typeof(ImageSource),
                typeof(IconBtn));
        public ImageSource IconImg
        {
            get => (ImageSource)GetValue(IconImgProperty);
            set => SetValue(IconImgProperty, value);
        }

        
    }
}

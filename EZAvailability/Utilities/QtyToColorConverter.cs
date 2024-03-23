using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Graphics;

namespace EZAvailability.Utilities
{
    public class QtyToColorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null && values.Length == 2)
            {
                if (values[0] is int qty && values[1] is int lowStockAlert)
                {
                    // Assuming "Success" and "Warning" are resources defined in your App.xaml
                    if (qty == 0)
                    {
                        // Return red color if quantity is zero
                        return Color.FromHex("#DC3545");
                    }
                    else if (qty <= lowStockAlert)
                    {
                        // Return yellow color if quantity is less than low_stock_alert
                        return Color.FromHex("#fca553");
                    }
                    else
                    {
                        // Return default color or any other color if needed
                        return Color.FromHex("#198754");
                    }
                }
                else
                {
                    // Return default color or any other color if needed
                    return Colors.Green;
                }
            } else
            {
                return Colors.Grey;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}


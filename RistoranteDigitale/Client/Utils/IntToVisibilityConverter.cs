using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace RistoranteDigitaleClient.Utils
{
    public class IntToVisibilityConverter : IValueConverter
    {
        private int val;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int intParam = (int)parameter;
            val = (int)value;

            return (intParam & val) != 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

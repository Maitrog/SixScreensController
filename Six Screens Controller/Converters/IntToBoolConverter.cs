using System;
using System.Globalization;
using System.Windows.Data;

namespace Six_Screens_Controller.Converters
{
    /// <summary>
    /// Convert int to bool
    /// </summary>
    public class IntToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
                if ((int)value > 0)
                    return true;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

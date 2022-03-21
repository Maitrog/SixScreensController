using System;
using System.Globalization;
using System.Windows.Data;

namespace Six_Screens_Controller.Converters
{
    public class TemplateHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value / System.Convert.ToDouble((string)parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

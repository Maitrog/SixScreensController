﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace Six_Screens_Controller.Converters
{
    /// <summary>
    /// Invert bool variable
    /// </summary>
    public class InvertBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null ? !(bool)value : value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

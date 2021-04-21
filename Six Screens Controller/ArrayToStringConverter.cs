using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace Six_Screens_Controller
{
    /// <summary>
    /// Convert string array to parameters string
    /// </summary>
    public class ArrayToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string args = "";
            if (value != null)
            {
                args += ((string[])value)[0];
                for (int i = 1; i < ((string[])value).Length; i++)
                {
                    args += " " + ((string[])value)[i];
                }
            }
            return args;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string[] args;
            args = ((string)value).Split(" ");
            return args;
        }
    }
}

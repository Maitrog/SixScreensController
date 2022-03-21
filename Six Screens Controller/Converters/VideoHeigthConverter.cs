using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace Six_Screens_Controller.Converters
{
    internal class VideoHeigthConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string[] input = ((string)parameter).Split(",");
            double videoWidth = double.Parse(input[0]);
            double videoHeight = double.Parse(input[1]);
            double actualWidth = (double)values[0];
            double actualHeight = (double)values[1];
            double videoProportion = videoWidth / videoHeight;
            double actualProportion = actualWidth / actualHeight;
            if (videoProportion >= 0.5)
            {
                return actualHeight;
            }
            else
            {
                return actualHeight * videoWidth / videoHeight;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

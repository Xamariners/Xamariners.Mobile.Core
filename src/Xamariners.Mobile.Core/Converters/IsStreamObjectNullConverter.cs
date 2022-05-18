using System;
using System.Globalization;
using System.IO;
using Xamarin.Forms;

namespace Xamariners.Mobile.Core.Converters
{
    /// <summary>
    /// check if a stream object is empty or not 
    /// </summary>
    public class IsStreamObjectNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if (((Stream) value).Length > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

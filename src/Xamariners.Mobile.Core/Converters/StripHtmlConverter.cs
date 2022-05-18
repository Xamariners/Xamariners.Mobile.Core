using System;
using System.Globalization;
using Xamarin.Forms;
using Xamariners.Core.Common.Helpers;

namespace Xamariners.Mobile.Core.Converters
{
    public class StripHtmlConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null ? StringHelpers.StripHTML(value.ToString()) : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

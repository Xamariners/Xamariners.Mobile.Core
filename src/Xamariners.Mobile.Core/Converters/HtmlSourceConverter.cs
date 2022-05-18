using System;
using Xamarin.Forms;

namespace Xamariners.Mobile.Core.Converters
{
    public class HtmlSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var html = new HtmlWebViewSource();

            if (value != null)
            {
                html.Html = value.ToString();
            }

            return html;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

}

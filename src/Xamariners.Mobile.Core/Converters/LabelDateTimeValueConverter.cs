using System;
using Xamarin.Forms;

namespace Xamariners.Mobile.Core.Converters
{
    public class LabelDateTimeValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return "-";
            }

            var dateTimeValue = (DateTime) value;

            if(dateTimeValue == DateTime.Parse("1/1/1900 12:00:00 AM") ||
               dateTimeValue == DateTime.MinValue ||
               dateTimeValue == default(DateTime))
            {
                return "-";
            }
            else
            {
                return (dateTimeValue).ToString("dd MMM yyyy");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            return (string) value == "-" ? null : value;
        }
    }
}
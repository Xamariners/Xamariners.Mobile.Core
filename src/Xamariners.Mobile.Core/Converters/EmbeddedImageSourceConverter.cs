using System;
using System.Globalization;
using Xamarin.Forms;

namespace Xamariners.Mobile.Core.Converters
{
    public class EmbeddedImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
                return null;

            return ImageSource.FromResource((string)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;

namespace Xamariners.Mobile.Core.Converters
{
    /// <summary>
    /// Return a Color object based on bound bool value
    /// Pass in the Color preferences for the bool value
    /// as Red|Green as a parameter
    /// </summary>
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            Color trueColor;
            Color falseColor;

            var paramString = ((string)parameter);
            string[] colorStringList = paramString.Split('|');

            var colorTypeConverter = new ColorTypeConverter();
            var colorObject1 = colorTypeConverter.ConvertFromInvariantString(colorStringList.First());
            var colorObject2 = colorTypeConverter.ConvertFromInvariantString(colorStringList.Last());
            trueColor = (Color)colorObject1;
            falseColor = (Color)colorObject2;

            if (((bool)value))
            {
                return trueColor;
            }
            else
            {
                return falseColor;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
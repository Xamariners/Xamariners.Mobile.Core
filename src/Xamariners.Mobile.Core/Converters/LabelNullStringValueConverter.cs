using System;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace Xamariners.Mobile.Core.Converters
{
    /// <summary>
    /// To indicate null strings as N/A on Label controls
    /// </summary>
    public class LabelNullStringValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string displayField = null;

            if (parameter != null)
                displayField = (string)parameter;
           
            if (value == null)
            {
                if (!string.IsNullOrEmpty(displayField))
                {
                    var o = Activator.CreateInstance(targetType);
                    var prop = value.GetType().GetRuntimeProperties().FirstOrDefault(p => string.Equals(p.Name, displayField, StringComparison.OrdinalIgnoreCase));
                    prop?.SetValue(o, "-");
                    return o;
                }

                return "-";
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var text = (string) value;
            return (string)value == "-" ? null : value;
        }
    }
}
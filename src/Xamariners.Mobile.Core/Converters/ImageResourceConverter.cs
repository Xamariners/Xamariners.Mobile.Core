using System;
using System.Globalization;
using System.Reflection;
using PCLAppConfig;
using Xamarin.Forms;
using Xamariners.Core.Common.Helpers;

namespace Xamariners.Mobile.Core.Converters
{
    /// <summary>
    /// Image Resource Loading converter compatible
    /// with Fake Data EmbeddedResource images and hosted images
    /// Not Cache enabled, only to be used with Xamarin.Forms.Image
    /// </summary>
    public class ImageResourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null) return null;

            ImageSource imageSource = null;
            string imageValue = (string)value;

            if (imageValue.StartsWith("http"))
            {
                imageSource = ImageSource.FromUri(new Uri(imageValue));
            }
            else
            {
                // retrieve the Image from fake data
                var fakeAssemblyName = ConfigurationManager.AppSettings["fakeassembly.name"];

                Assembly assemblyName =
                    ApplicationDomainHelpers.GetAssembly(fakeAssemblyName);

                imageSource = ImageSource.FromResource(imageValue, assemblyName);
            }

            return imageSource;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

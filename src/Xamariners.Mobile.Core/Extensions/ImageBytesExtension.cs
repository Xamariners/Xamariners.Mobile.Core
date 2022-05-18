using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xamariners.Mobile.Core.Extensions
{
    [ContentProperty("Source")]
    public class ImageBytesExtension : IMarkupExtension
    {
        public byte[] Source { get; set; }

      
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return Source == null ? null : ImageSource.FromStream(() => new MemoryStream(Source));
        }
    }
}

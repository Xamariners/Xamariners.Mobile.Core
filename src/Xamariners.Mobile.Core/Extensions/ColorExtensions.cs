namespace Xamariners.Mobile.Core.Extensions
{
    public static class ColorExtensions
    {
        /// <summary>
        /// Get Hex string of Xamarin.Forms.Color object
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string GetHexString(this Xamarin.Forms.Color color)
        {
            var red = (int)(color.R * 255);
            var green = (int)(color.G * 255);
            var blue = (int)(color.B * 255);
            var alpha = (int)(color.A * 255);
            var hex = $"#{alpha:X2}{red:X2}{green:X2}{blue:X2}";

            return hex;
        }
    }
}

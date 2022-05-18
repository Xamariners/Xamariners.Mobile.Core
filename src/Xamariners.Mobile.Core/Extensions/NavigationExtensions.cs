using System.Linq;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace Xamariners.Mobile.Core.Extensions
{
    public static class NavigationExtensions
    {
        public static async Task PushPopupAsyncSafe(this INavigation sender, PopupPage page, IPopupNavigation popupNavigation, bool animate = true)
        {
            if (Device.RuntimePlatform == "Test" || page is null) return;

            // check if already pushed
            if (popupNavigation.PopupStack.Count > 0 &&
                popupNavigation.PopupStack.Last() == page)
                return;

            await popupNavigation.PushAsync(page, animate).ConfigureAwait(true);

            // trigger OnShellNavigatingIn on popup page, like on shell pages
            var pageName = page.GetType().ToString().Split('.').LastOrDefault()?.Replace(".", "").ToLower();
            MessagingCenter.Send<string, ShellNavigatingEventArgs>(nameof(Application), $"{pageName}OnShellNavigatingIn", null);
        }

        public static async Task PopPopupAsyncSafe(this INavigation sender, IPopupNavigation popupNavigation, bool animate = true)
        {
            if (Device.RuntimePlatform == "Test") return;

            // trigger OnShellNavigatingOut on popup page, like on shell pages
            var page = popupNavigation.PopupStack.LastOrDefault();
            if (page != null)
            {
                var pageName = page.GetType().ToString().Split('.').LastOrDefault()?.Replace(".", "").ToLower();
                MessagingCenter.Send<string, ShellNavigatingEventArgs>(nameof(Application), $"{pageName}OnShellNavigatingOut", null);
            }

            await popupNavigation.PopAsync(animate).ConfigureAwait(true);
        }

        public static async Task PopAllPopupAsyncSafe(this INavigation sender, IPopupNavigation popupNavigation, bool animate = true)
        {
            if (Device.RuntimePlatform == "Test") return;
            if (popupNavigation.PopupStack.Any())
            {
                await popupNavigation.PopAllAsync(animate).ConfigureAwait(true);
            }
        }

        public static async Task RemovePopupPageAsyncSafe(this INavigation sender, PopupPage page, IPopupNavigation popupNavigation, bool animate = true)
        {
            if (Device.RuntimePlatform == "Test") return;
            if (popupNavigation.PopupStack.FirstOrDefault(x => x == page) is null)
            {
                return;
            }
            // trigger OnShellNavigatingOut on popup page, like on shell pages
            var pageName = page.GetType().ToString().Split('.').LastOrDefault()?.Replace(".", "").ToLower();
            MessagingCenter.Send<string, ShellNavigatingEventArgs>(nameof(Application), $"{pageName}OnShellNavigatingOut", null);

            await popupNavigation.RemovePageAsync(page, animate).ConfigureAwait(true);
        }
    }
}
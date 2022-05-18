using System;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamariners.Core.Common.Enum;
using Xamariners.Mobile.Core.Services;

namespace Xamariners.Mobile.Core.Interfaces
{
    public interface INavigationService
    {
        INavigation Navigation { get; }
        Shell Shell { get; }
        Page CurrentPage { get; }
        NavigationDirection NavigationDirection { get; set; }
        Task GoToAsync<TViewModel>(string route, Action<TViewModel> initialiser = null, bool animate = true);
        Task GoToAsync(string route, bool animate = true);
        Task GoBackAsync<TViewModel>(Action<TViewModel> initialiser);
        Task GoBackAsync();
        Task GoToRootAsync(bool animate = true);
        Task GoToShellAsync(string title, bool animate = true);
        Task GoToShellRootAsync(bool animate = true);

        Task GoToPopupAsync<TViewModel>(PopupPage page, Action<TViewModel> initialiser = null, bool animate = true);
        Task GoToPopupAsync(PopupPage page, bool animate = true);
        Task GoBackPopupAsync(bool animate = true);
        Task RemoveAllPopupAsync(bool animate = true);
        Task RemovePopupAsync(PopupPage page, bool animate = true);

        void Initialise();

        event EventHandler<NavigationService.TabChangedEventArgs> OnTabChanged;
        TabBar CurrentTabBar { get; }
        void ClearTabBarStack();
    }
}
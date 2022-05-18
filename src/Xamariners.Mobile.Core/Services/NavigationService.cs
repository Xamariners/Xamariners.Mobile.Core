using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamariners.Core.Common.Enum;
using Xamariners.Mobile.Core.Extensions;
using Xamariners.Mobile.Core.Helpers;
using Xamariners.Mobile.Core.Interfaces;

namespace Xamariners.Mobile.Core.Services
{
    public class NavigationService : INavigationService
    {
        private readonly IInitialiserService _initialiserService;
        public INavigation Navigation => GetCurrentPage().Navigation;
        public Shell Shell => Application.Current.MainPage as Shell;
        public Page CurrentPage => GetCurrentPage();
        public NavigationDirection NavigationDirection { get; set; }
        public string Root { get; set; } = "root";
        public TabBar CurrentTabBar => (TabBar)Shell.Current.GetType().GetProperty("CurrentTabBar")?.GetValue(Shell.Current);

        public class TabChangedEventArgs : EventArgs
        {
            public bool CancelNavigation { get; set; }
            public string CurrentTabRoute { get; set; }
            public string PreviousTabRoute { get; set; }

        }

        public event EventHandler<TabChangedEventArgs> OnTabChanged;

        private readonly IPopupNavigation _popupNavigation;
        private string _currentTabRoute;
        private string _previousTabRoute;
        private readonly Stack<string> _currentTabStack;

        public NavigationService(IInitialiserService initialiserService, IPopupNavigation popupNavigation)
        {
            _initialiserService = initialiserService;

            _popupNavigation = popupNavigation;
            _currentTabStack = new Stack<string>();
        }

        public void Initialise()
        {
            InitialiseTarBarHandler();
        }

        private void InitialiseTarBarHandler()
        {
            // Init TabBar Events
            if (Shell.Current != null && CurrentTabBar != null)
            {
                CurrentTabBar.PropertyChanging += (sender, args) =>
                {
                    _previousTabRoute = CurrentTabBar.CurrentItem.Route;
                };

                CurrentTabBar.PropertyChanged += (sender, args) =>
                {
                    _currentTabRoute = CurrentTabBar.CurrentItem.Route;

                    if (_previousTabRoute != _currentTabRoute)
                    {
                        var cancelNavigation = TabChanged(new TabChangedEventArgs
                        {
                            CancelNavigation = false,
                            CurrentTabRoute = _currentTabRoute,
                            PreviousTabRoute = _previousTabRoute,
                        });

                        ClearTabBarStack();
                    }
                };
            };
        }

        public void ClearTabBarStack()
        {
            _currentTabStack.Clear();

            _currentTabStack.Push($"//{Root}/{_currentTabRoute}/{CurrentTabBar.CurrentItem.CurrentItem.Route}");

            foreach (var item in CurrentTabBar.Items)
            {
                if (item?.Stack?.Count > 1)
                    item.Stack[1].Navigation.PopToRootAsync();
            }
        }

        protected virtual bool TabChanged(TabChangedEventArgs e)
        {
            OnTabChanged?.Invoke(this, e);

            return e.CancelNavigation;
        }

        private Page GetCurrentPage()
        {
            var mainPage = GetShellPage(Application.Current.MainPage);

            if (mainPage?.Navigation?.NavigationStack?.Count() > 1)
                return mainPage?.Navigation?.NavigationStack.LastOrDefault();

            switch (mainPage)
            {
                case MasterDetailPage page:
                    return page.Detail;
                case TabbedPage _:
                case CarouselPage _:
                    return ((MultiPage<Page>)mainPage).CurrentPage;
                default:
                    return mainPage;
            }
        }

        private Page GetShellPage(object item)
        {
            if (item is null)
                return null;

            if (item is ShellContent content)
            {
                if (content.Content is null)
                {
                    if (((IShellContentController)content).Page is Page page)
                    {
                        return page;
                    }
                }
                return content.Content as Page;
            }

            if (item is Shell shell)
                return GetShellPage(shell.CurrentItem);

            if (item is ShellItem shellItem)
                return GetShellPage(shellItem.CurrentItem);

            if (item is ShellSection shellSection)
                return GetShellPage(shellSection.CurrentItem);

            return null;
        }

        public async Task GoToAsync<TViewModel>(string route, Action<TViewModel> initialiser = null, bool animate = true)
        {
            if (initialiser != null)
                _initialiserService.SetInitialiser(initialiser);

            await GoToAsync(route, animate).ConfigureAwait(true); ;
        }

        public async Task GoToAsync(string route, bool animate = true)
        {
            if (Shell == null)
                return;

            NavigationDirection = NavigationDirection.Forward;

            // add to tab stack if not exist
            if (!_currentTabStack.Contains(route))
                _currentTabStack.Push(route);

            if (AddToNavigationStack(route))
                await Shell.Current.GoToAsync(route, animate).ConfigureAwait(true); ;
        }

        public async Task GoBackAsync<TViewModel>(Action<TViewModel> initialiser = null)
        {
            if (initialiser != null)
                _initialiserService.SetInitialiser(initialiser);

            await GoBackAsync().ConfigureAwait(true); ;
        }

        public async Task GoBackAsync()
        {
            if (Shell == null)
                return;

            NavigationDirection = NavigationDirection.Backwards;

            // if on tab stack, pop back
            if (_currentTabStack.Count > 1)
            {
                // remove current route from stack
                _currentTabStack.Pop();

                //go to previous route on that tab
                _currentTabStack.Pop();

                // We don't need the below as we are using non xaml defined shellitems - leaving in case of issues
                // await GoToAsync(_currentTabStack.Pop()).ConfigureAwait(true);
            }
            //else // as we are using non xaml defined shellitems, we are good to go for all
            Shell.SendBackButtonPressed();
        }

        public async Task GoToRootAsync(bool animate = true)
        {
            if (Shell == null)
                return;

            NavigationDirection = NavigationDirection.Backwards;

            await Navigation.PopToRootAsync(true).ConfigureAwait(true); ;
        }

        public async Task GoToShellRootAsync(bool animate = true)
        {
            if (Shell == null)
                return;

            NavigationDirection = NavigationDirection.Backwards;
            if (Shell.Current != null && Shell.Current.Items != null && Shell.Current.Items.Count > 0)
            {
                Shell.Current.CurrentItem = Shell.Current.Items.First(x => x.Route == Root);
            }
        }

        public Task GoToPopupAsync(PopupPage page, bool animate = true)
        {
            return Navigation.PushPopupAsyncSafe(page, _popupNavigation, animate);
        }

        public Task GoBackPopupAsync(bool animate = true)
        {
            return Navigation.PopPopupAsyncSafe(_popupNavigation, animate);
        }

        public Task RemoveAllPopupAsync(bool animate = true)
        {
            return Navigation.PopAllPopupAsyncSafe(_popupNavigation, animate);
        }

        public Task RemovePopupAsync(PopupPage page, bool animate = true)
        {
            return Navigation.RemovePopupPageAsyncSafe(page, _popupNavigation, animate);
        }

        public async Task GoToPopupAsync<TViewModel>(PopupPage page, Action<TViewModel> initialiser = null, bool animate = true)
        {
            if (initialiser != null)
                _initialiserService.SetInitialiser(initialiser);

            await Navigation.PushPopupAsyncSafe(page, _popupNavigation, animate).ConfigureAwait(true);
        }

        public bool AddToNavigationStack(string route)
        {
            if (NavigationDirection == NavigationDirection.Backwards)
                return false;

            NavigationDirection = NavigationDirection.Forward;

            return true;
        }

        public async Task GoToShellAsync(string title, bool animate = true)
        {
            if (Shell == null)
                return;

            NavigationDirection = NavigationDirection.Backwards;

            await Navigation.PopToRootAsync(true).ConfigureAwait(true);

            Shell.Current.CurrentItem = Shell.Current.Items.FirstOrDefault(x => x.Title == title);
        }
    }
}
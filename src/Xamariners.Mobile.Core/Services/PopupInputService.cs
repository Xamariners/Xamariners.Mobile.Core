using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Contracts;
using Xamarin.Forms;
using Xamariners.Mobile.Core.Controls.AlertViews;
using Xamariners.Mobile.Core.Helpers;
using Xamariners.Mobile.Core.Interfaces;

namespace Xamariners.Mobile.Core.Services
{
     public class PopupInputService : IPopupInputService, IDisposable
    {
        private readonly IPopupNavigation _popupNavigation;

        public string OkButtonText { get; set; }
        public string MessageText { get; set; }
        public string TitleText { get; set; }
        public string PlaceHolderText { get; set; }
        public string ValidationLabelText { get; set; }
        public string CancelButtonText { get; set; }
        public bool IsShowing { get; set; }

        private InputTextOkCancelView _inputTextOkCancelView;
        private MessageAlertOkCancelView _messageAlertOkCancelView;
        private MessageAlertOkView _messageAlertOkView;
        private InfoMarkdownView _infoMarkdownView;

        public PopupInputService(IPopupNavigation popupNavigation)
        {
            _popupNavigation = popupNavigation;
        }

        public async Task<string> ShowInputTextOkCancelAlertPopup(string titleText, string messageText, string placeHolderText, string okButtonText,
            string cancelButtonText, string validationLabelText)
        {
            TitleText = titleText;
            MessageText = messageText;
            OkButtonText = okButtonText;
            PlaceHolderText = placeHolderText;
            CancelButtonText = cancelButtonText;
            ValidationLabelText = validationLabelText;

             _inputTextOkCancelView = new InputTextOkCancelView(
                titleText,
                messageText,
                placeHolderText,
                okButtonText,
                cancelButtonText,
                validationLabelText);

            var page = new CustomAlertPopupPage<string>(_inputTextOkCancelView);

            var clicked = false;

            _inputTextOkCancelView.OkButtonEventHandler += (sender, args) =>
            {
                try
                {
                    if (clicked) return;
                    clicked = true;

                    if (!string.IsNullOrEmpty(((InputTextOkCancelView) sender).TextInputResult))
                    {
                        ((InputTextOkCancelView) sender).IsValidationLabelVisible = false;
                        page.PageClosingTaskCompletionSource.SetResult(((InputTextOkCancelView) sender)
                            .TextInputResult);
                    }
                    else
                    {
                        ((InputTextOkCancelView) sender).IsValidationLabelVisible = true;
                    }

                    IsShowing = false;
                }
                catch
                {
                    clicked = false;
                }
            };

            _inputTextOkCancelView.CancelButtonEventHandler += (sender, args) =>
            {
                try
                {
                    if (clicked) return;
                    clicked = true;

                    page.PageClosingTaskCompletionSource.SetResult(null);
                    IsShowing = false;
                }
                catch
                {
                    clicked = false;
                }
            };

            IsShowing = true;

            return await Navigate(page).ConfigureAwait(true);
        }

        public async Task<bool> ShowMessageOkCancelAlertPopup(
            string titleText, string messageText,
            string okButtonText, string cancelButtonText)
        {
            TitleText = titleText;
            MessageText = messageText;
            OkButtonText = okButtonText;
            CancelButtonText = cancelButtonText;

            if (Device.RuntimePlatform == "Test")
                return true;

            _messageAlertOkCancelView = new MessageAlertOkCancelView(
                titleText, messageText,
                okButtonText, cancelButtonText);

            var page = new CustomAlertPopupPage<bool>(_messageAlertOkCancelView);

            var clicked = false;

            _messageAlertOkCancelView.OkButtonEventHandler += (sender, args) =>
            {
                try
                {
                    if (clicked) return;
                    clicked = true;

                    page.PageClosingTaskCompletionSource.SetResult(true);

                    IsShowing = false;
                }
                catch
                {
                    clicked = false;
                }
            };

            _messageAlertOkCancelView.CancelButtonEventHandler += (sender, args) =>
            {
                try
                {
                    if (clicked) return;

                    clicked = true;

                    page.PageClosingTaskCompletionSource.SetResult(false);

                    IsShowing = false;

                }
                catch
                {
                    clicked = false;
                }
            };

            IsShowing = true;

            return await Navigate(page).ConfigureAwait(true);
        }
        
        public Task<string> ShowInputSelectionCancelAlertPopup(string titleText, List<string> selectionList, string cancelButtonText)
        {
            throw new NotImplementedException();
        }

        public async Task<T> ShowCustomViewAlertPopup<T>(object viewObject)
        {
            var page = new CustomAlertPopupPage<T>((View)viewObject);
            IsShowing = true;
            return await Navigate(page).ConfigureAwait(true); ;
        }

        public async Task CloseLastPopup()
        {
            if (Device.RuntimePlatform == "Test")
            {
                IsShowing = false;
                return;
            }

            if (_popupNavigation.PopupStack.Any())
                await _popupNavigation.PopAsync().ConfigureAwait(true); ;

            IsShowing = false;
        }

        public async Task ShowInfoMarkdownViewPopup(string markDownText, ImageSource imageSource)
        {
            _infoMarkdownView = new InfoMarkdownView(markDownText, imageSource);
            var page = new CustomAlertPopupPage<string>(_infoMarkdownView);

            var clicked = false;

            _infoMarkdownView.CancelButtonEventHandler += async (sender, args) =>
            {
                try
                {
                    if (clicked) return;
                    clicked = true;

                    await CloseLastPopup().ConfigureAwait(true); ;
                }
                catch
                {
                    clicked = false;
                }
            };

            await Navigate(page).ConfigureAwait(true);
            IsShowing = true;
        }

        public async Task<string> ShowMessageOkAlertPopup(
            string titleText, string messageText,
            string okButtonText)
        {
            TitleText = titleText;
            MessageText = messageText;
            OkButtonText = okButtonText;

            _messageAlertOkView = new MessageAlertOkView(
                titleText, messageText, okButtonText);

            var page = new CustomAlertPopupPage<string>(_messageAlertOkView);

            var clicked = false;

            _messageAlertOkView.OkButtonEventHandler += (sender, args) =>
            {
                try
                {
                    if (clicked) return;
                    clicked = true;

                    IsShowing = false;

                    page.PageClosingTaskCompletionSource.SetResult(okButtonText);

                    _messageAlertOkView = null;
                }
                catch
                {
                    clicked = false;
                }
            };

            IsShowing = true;

            return await Navigate(page).ConfigureAwait(true); ;
        }

        /// <summary>
        /// Handle popup page Navigation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="popup"></param>
        /// <returns></returns>
        private async Task<T> Navigate<T>(CustomAlertPopupPage<T> popup)
        {
            if (Device.RuntimePlatform == "Test")
                return default(T);

            ThreadingHelpers.InvokeOnMainThread(async () =>
            {
                await _popupNavigation.PushAsync(popup).ConfigureAwait(true); ;
            });

            // await for the user to enter the text input
            var result = await popup.PageClosingTask.ConfigureAwait(true); ;

            // Pop the page from Navigation Stack
            ThreadingHelpers.InvokeOnMainThread(async () =>
            {
                await _popupNavigation.RemovePageAsync(popup).ConfigureAwait(true); ;
            });

            return result;
        }

        public void Dispose()
        {
            _inputTextOkCancelView = null;
            _messageAlertOkCancelView = null;
            _messageAlertOkView = null;
            _infoMarkdownView = null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using PCLAppConfig;
using Xamariners.Core.Common;
using Xamariners.Core.Common.Helpers;
using Xamariners.Core.Interface;
using Xamariners.Mobile.Core.Exceptions;
using Xamariners.Mobile.Core.Helpers;
using Xamariners.Mobile.Core.Infrastructure;
using Xamariners.Mobile.Core.Interfaces;
using Xamariners.RestClient.Helpers.Models;

namespace Xamariners.Mobile.Core.Services
{
    public class ErrorService : IErrorService, IDisposable
    {
        // errors are cross view models
        private List<ViewModelError> _errors;
        private readonly IPopupInputService _popupInputService;
        private readonly INavigationService _navigationService;
        private readonly ISpinner _spinner;
        private readonly ILogger _logger;

        private readonly bool _isCuteError;
        private string _cuteErrorTitle;
        private readonly string _cuteErrorDescription;
        public List<ViewModelError> TrashCan { get; private set; }

        public Dictionary<string, string> ActiveInlineValidators { get; set; }
        public bool ClearErrorsAfterProcessing { get; set; }

        public ErrorService(ILogger logger, IPopupInputService popupInputService, ISpinner spinner, INavigationService navigationService)
        {
            _popupInputService = popupInputService;
            _spinner = spinner;
            _logger = logger;
            _navigationService = navigationService;
            _errors = new List<ViewModelError>();
            TrashCan = new List<ViewModelError>();
            ActiveInlineValidators = new Dictionary<string, string>();
            ClearErrorsAfterProcessing = true;

            _isCuteError = bool.Parse(ConfigurationManager.AppSettings["cuteerror.enabled"]);
            _cuteErrorTitle = ConfigurationManager.AppSettings["cuteerror.title"];
            _cuteErrorDescription = ConfigurationManager.AppSettings["cuteerror.description"];
        }

        /// <summary>
        /// The add error.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="title"></param>
        /// <param name="errorItems"></param>
        /// <param name="ex">
        /// The ex.
        /// </param>
        /// <param name="viewModelType"></param>
        /// <param name="errors"></param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public List<ViewModelError> AddError(string description, ViewModelError.ErrorAction action, ViewModelError.ErrorSeverity severity, string title = null, List<string> errorItems = null, Exception ex = null, string viewModelType = null, ReadOnlyDictionary<string, ReadOnlyCollection<string>> errors = null, Action onPopupClosed = null)
        {
            var error = new ViewModelError(title, description, action, severity, errorItems, ex, viewModelType, errors);
            error.OnPopupClosed = onPopupClosed;
            _errors.Add(error);
            return _errors;
        }

        /// <summary>
        /// The add error.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="ex">
        /// The ex.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public List<ViewModelError> AddError<T>(ServiceResponse<T> serviceResponse)
        {
            Exception exception = null;

            if (!serviceResponse.IsStatusOK())
                exception = new Exception(serviceResponse.ErrorMessage);

            var error = new ViewModelError(serviceResponse.Message,
                serviceResponse.ErrorMessage,
                ViewModelError.ErrorAction.Display,
                ViewModelError.ErrorSeverity.Error, serviceResponse.Errors.ToList(), exception);

            _errors.Add(error);
            return _errors;
        }

        public void ProcessErrors()
        {
            try
            {
                if (_errors == null || !_errors.Any())
                    return;

                var errorsToLog = _errors.Where(x => x.Action != ViewModelError.ErrorAction.Display);
                var errorsToDisplay = _errors.Where(x => x.Action != ViewModelError.ErrorAction.Log);

                foreach (var errorToLog in errorsToLog)
                    LogError(errorToLog);

                DisplayInlineErrors(ref errorsToDisplay);

                foreach (var errorToDisplay in errorsToDisplay)
                    DisplayError(errorToDisplay);

                ErrorClear();
            }
            catch
            {
                // swallow errors while processing errors
                ;
            }
        }

        public void ClearInlineValidators()
        {
            var page = _navigationService.CurrentPage;

            foreach (var validatorKey in ActiveInlineValidators)
            {
                if (validatorKey.Value != page.GetType().Name)
                    return;

                // Looking for UI element called {PropertyNameValidator}
                var validator = page?.FindByName($"{validatorKey.Key}Validator");

                validator?.SetProperty("Text", "");
            }

            ActiveInlineValidators.Clear();
        }

        protected virtual void ErrorClear()
        {
            TrashCan.AddRange(_errors); // for testing and other purposes
            _errors.Clear();
        }

        public List<ViewModelError> GetErrors()
        {
            return _errors;
        }

        private void DisplayInlineErrors(ref IEnumerable<ViewModelError> viewModelErrors)
        {
            var page = _navigationService.CurrentPage;

            var popupErrorItems = viewModelErrors.ToList();

            // check if PropertyNameValidator exist on the current page

            foreach (var error in viewModelErrors)
            {
                if (error?.Errors == null)
                    continue;

                foreach (var e in error.Errors)
                {
                    // Looking for UI element called {PropertyNameValidator}
                    var validator = page?.FindByName($"{e.Key}Validator");

                    if (validator == null)
                        continue;

                    validator.SetProperty("Text", e.Value.FirstOrDefault());
                    ActiveInlineValidators.AddReplace(e.Key, page.GetType().Name);
                    popupErrorItems?.Remove(error);
                }
            }

            viewModelErrors = popupErrorItems.AsEnumerable();
        }

        private async void DisplayError(ViewModelError error)
        {
            _spinner.HideLoading();

            if (_popupInputService.IsShowing)
                return;

                if (error.Exception is NetworkException)
            {
                DisplayNetworkError(error);
                return;
            }

            var showCuteError = _isCuteError
                                && error.Exception != null
                                && !(error.Exception is HandledException || error.Exception is RankException);


            var popupErrorItems = error.ErrorItems?.ToDictionary(x => x);

            // check if PropertyNameValidator exist on the current page

            if (error.ErrorItems != null)
            {
                foreach (var errorItem in error.ErrorItems.ToDictionary(x => x))
                {
                    // Looking for UI element called {PropertyNameValidator}
                    var validator = _navigationService.CurrentPage.FindByName($"{errorItem.Key}Validator");

                    if (validator == null)
                        continue;

                    validator.SetProperty("Text", errorItem.Value);
                    popupErrorItems?.Remove(errorItem.Key);
                }
            }

            var alert = new ErrorAlert()
            {
                Description = showCuteError ? _cuteErrorDescription : error.Description, // TODO:  cute vs dev errors
                Title = showCuteError ? _cuteErrorTitle : error.Title ?? error.Severity.ToString(), // TODO:  cute vs dev errors
                ErrorItems = popupErrorItems, // when doing validation, add error/property Pair
                ViewModelType = this.GetType(),
                Severity = error.Severity
            };

            await _popupInputService.ShowMessageOkAlertPopup(alert.Title, alert.Description, "OK").ConfigureAwait(true);
        }

        private void DisplayNetworkError(ViewModelError error)
        {
            var alert = new ErrorAlert()
            {
                Description = error.Description,
                Title = _cuteErrorTitle,
                ViewModelType = this.GetType(),
                Severity = ViewModelError.ErrorSeverity.Warning
            };

            ThreadingHelpers.InvokeOnMainThread(async () => await _popupInputService.ShowMessageOkAlertPopup(alert.Title, alert.Description, "OK").ConfigureAwait(true));
        }

        private void LogError(ViewModelError error)
        {
            _logger.LogAction(new Exception($"{error.ViewModelType} - {error.Description}", error.Exception ?? new Exception($"{error.ViewModelType} - {error.Description}")));
        }

        public void Dispose() { }
    }
}

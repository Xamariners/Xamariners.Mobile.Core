using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamariners.Mobile.Core.Infrastructure;
using Xamariners.RestClient.Helpers.Models;

namespace Xamariners.Mobile.Core.Interfaces
{
    public interface IErrorService
    {
        List<ViewModelError> AddError(string description, ViewModelError.ErrorAction action, ViewModelError.ErrorSeverity severity, string title = null, List<string> errorItems = null, Exception ex = null, string viewModelType = null, ReadOnlyDictionary<string, ReadOnlyCollection<string>> errors = null, Action onPopupClosed = null);
        List<ViewModelError> AddError<T>(ServiceResponse<T> serviceResponse);
        List<ViewModelError> GetErrors();
        void ProcessErrors();
        List<ViewModelError> TrashCan { get; }
        void ClearInlineValidators();
    }
}

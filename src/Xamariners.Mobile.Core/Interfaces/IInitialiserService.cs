using System;

namespace Xamariners.Mobile.Core.Interfaces
{
    public interface IInitialiserService
    {
        void SetInitialiser<TViewModel>(Action<TViewModel> initialiser);
        void Initialise<TViewModel>(TViewModel viewModel);
    }
}
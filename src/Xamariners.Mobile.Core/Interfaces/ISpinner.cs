namespace Xamariners.Mobile.Core.Interfaces
{
    public interface ISpinner
    {
        void HideLoading();

        void ShowLoading(bool isCancellable = true);
    }
}
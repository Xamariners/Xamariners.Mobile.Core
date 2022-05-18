using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace Xamariners.Mobile.Core.Controls.AlertViews
{
    public class CustomAlertPopupPage<T> : PopupPage
    {
        public Task<T> PageClosingTask => PageClosingTaskCompletionSource.Task;

        public TaskCompletionSource<T> PageClosingTaskCompletionSource { get; set; }

        public CustomAlertPopupPage(View inputView)
        {
            Content = inputView;

            PageClosingTaskCompletionSource = new TaskCompletionSource<T>();

            this.BackgroundColor = new Color(0, 0, 0, 0.5);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
        // Method for animation child in PopupPage
        // Invoked after custom animation end

        protected override void OnAppearingAnimationEnd()
        {
            Content.FadeTo(1);
        }

        // Method for animation child in PopupPage
        // Invoked before custom animation begin
        protected override void OnDisappearingAnimationBegin()
        {
            Content.FadeTo(1);
        }

        protected override bool OnBackButtonPressed()
        {
            // Prevent hide popup
            //return base.OnBackButtonPressed();
            return true;
        }

        // Invoced when background is clicked
        protected override bool OnBackgroundClicked()
        {
            // Return default value - CloseWhenBackgroundIsClicked
            return false;
        }
    }
}

using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamariners.Mobile.Core.Exceptions;

namespace Xamariners.Mobile.Core.Controls.AlertViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MessageAlertOkView : ContentView
    {
        public EventHandler OkButtonEventHandler { get; set; }

        public MessageAlertOkView(
            string titleText,
            string messageText,
            string okButtonText)
        {
            try
            {
                InitializeComponent();
            }
            catch (XamlParseException xp)
            {
                if (!xp.Message.Contains(ExceptionLiteral.StaticResNotFound))
                    throw;
            }

            TitleLabel.Text = titleText;
            MessageLabel.Text = messageText;
            OkButton.Text = okButtonText;

            OkButton.Clicked += OkButton_Clicked;

            if (Device.RuntimePlatform == Device.Android)
            {
                FrameView.CornerRadius = 2;
                TitleLabel.Text = TitleLabel.Text.ToUpper();
            }
            else
            {
                FrameView.CornerRadius = 8;
                TitleLabel.HorizontalTextAlignment = TextAlignment.Center;
                MessageLabel.HorizontalTextAlignment = TextAlignment.Center;
            }
        }

        private void OkButton_Clicked(object sender, EventArgs e)
        {
            OkButtonEventHandler?.Invoke(this, e);
        }
    }
}
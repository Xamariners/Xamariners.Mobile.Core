using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xamariners.Mobile.Core.Controls.AlertViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InfoMarkdownView : ContentView
    {
        public EventHandler CancelButtonEventHandler { get; set; }

        public InfoMarkdownView(string markDownText, ImageSource imageSource)
        {
            InitializeComponent();
            markdownCustomView.Markdown = markDownText;
            imageButtonInfo.Source = imageSource;
        }

        private void CloseInfoImageButton_OnClicked(object sender, EventArgs e)
        {
            CancelButtonEventHandler?.Invoke(this, e);
        }
    }
}
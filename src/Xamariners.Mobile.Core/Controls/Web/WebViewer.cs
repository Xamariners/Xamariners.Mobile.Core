using Newtonsoft.Json.Linq;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Xamariners.Mobile.Core.Controls.Web
{
    public class WebViewer : View
    {
        private Action<string> _action;

        public static readonly BindableProperty HtmlDataProperty =
            BindableProperty.Create(nameof(HtmlData), typeof(string), typeof(WebViewer), default(string));

        public static readonly BindableProperty SuccessCallbackCommandProperty =
            BindableProperty.Create(nameof(SuccessCallbackCommand), typeof(ICommand), typeof(WebViewer), null);

        public static readonly BindableProperty ErrorCallbackCommandProperty =
            BindableProperty.Create(nameof(ErrorCallbackCommand), typeof(ICommand), typeof(WebViewer), null);

        public static readonly BindableProperty CancelCallbackCommandProperty =
            BindableProperty.Create(nameof(CancelCallbackCommand), typeof(ICommand), typeof(WebViewer), null);

        public ICommand SuccessCallbackCommand
        {
            get { return (ICommand)GetValue(SuccessCallbackCommandProperty); }
            set { SetValue(SuccessCallbackCommandProperty, value); }
        }

        public ICommand ErrorCallbackCommand
        {
            get { return (ICommand)GetValue(ErrorCallbackCommandProperty); }
            set { SetValue(ErrorCallbackCommandProperty, value); }
        }

        public ICommand CancelCallbackCommand
        {
            get { return (ICommand)GetValue(CancelCallbackCommandProperty); }
            set { SetValue(CancelCallbackCommandProperty, value); }
        }

        public string HtmlData
        {
            get { return (string)GetValue(HtmlDataProperty); }
            set { SetValue(HtmlDataProperty, value); }
        }

        public void RegisterAction(Action<string> callback)
        {
            _action = callback;
        }

        public void Cleanup()
        {
            _action = null;
        }

        public void InvokeAction(object data)
        {
            if (data is null) return;

            JObject o = JObject.Parse(data.ToString());
            var status = o["status"].Value<string>();
            var body = o["body"].Value<string>();

            if (string.IsNullOrEmpty(status) || string.IsNullOrEmpty(body)) return;

            switch (status)
            {
                case "complete":
                    SuccessCallbackCommand?.Execute(body);
                    break;
                case "cancelled":
                    CancelCallbackCommand?.Execute(body);
                    break;
                case "error":
                    ErrorCallbackCommand?.Execute(body);
                    break;
            }
        }

    }
}

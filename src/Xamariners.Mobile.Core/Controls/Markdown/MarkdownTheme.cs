using Xamarin.Forms;

namespace Xamariners.Mobile.Core.Controls.Markdown
{
    public class MarkdownTheme
    {
        public MarkdownTheme()
        {
            this.Paragraph = new MarkdownStyle
            {
                Attributes = FontAttributes.None,
                FontSize = 13,
            };

            this.Heading1 = new MarkdownStyle
            {
                Attributes = FontAttributes.Bold,
                BorderSize = 1,
                FontSize = 19,
            };

            this.Heading2 = new MarkdownStyle
            {
                Attributes = FontAttributes.Bold,
                BorderSize = 1,
                FontSize = 18,
            };

            this.Heading3 = new MarkdownStyle
            {
                Attributes = FontAttributes.Bold,
                FontSize = 17,
            };

            this.Heading4 = new MarkdownStyle
            {
                Attributes = FontAttributes.Bold,
                FontSize = 16,
            };

            this.Heading5 = new MarkdownStyle
            {
                BackgroundColor = Color.Brown,
                Attributes = FontAttributes.Bold,
                FontSize = 15,
            };

            this.Heading6 = new MarkdownStyle
            {
                BackgroundColor = Color.Violet,
                Attributes = FontAttributes.Bold,
                FontSize = 14,
            };

            this.Link = new MarkdownStyle
            {
                Attributes = FontAttributes.None,
                FontSize = 12,
            };

            this.Code = new MarkdownStyle
            {
                Attributes = FontAttributes.None,
                FontSize = 12,
            };

            this.Quote = new MarkdownStyle
            {
                Attributes = FontAttributes.None,
                BorderSize = 4,
                FontSize = 12,
                BackgroundColor = Color.Gray.MultiplyAlpha(.1),
            };

            this.Separator = new MarkdownStyle
            {
                BorderSize = 2,
            };

            // Platform specific properties
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    Code.FontFamily = "Courier";
                    break;

                case Device.Android:
                    Code.FontFamily = "monospace";
                    break;
            }
        }

        public Color BackgroundColor { get; set; }

        public MarkdownStyle Paragraph { get; set; }

        public MarkdownStyle Heading1 { get; set; }

        public MarkdownStyle Heading2 { get; set; }

        public MarkdownStyle Heading3 { get; set; }

        public MarkdownStyle Heading4 { get; set; }

        public MarkdownStyle Heading5 { get; set; }

        public MarkdownStyle Heading6 { get; set; }

        public MarkdownStyle Quote { get; set; }

        public MarkdownStyle Separator { get; set; }

        public MarkdownStyle Link { get; set; }

        public MarkdownStyle Code { get; set; }

        public float Margin { get; set; } = 10;
    }

    public class LightMarkdownTheme : MarkdownTheme
    {
        public LightMarkdownTheme()
        {
            this.BackgroundColor = DefaultBackgroundColor;
            this.Paragraph.ForegroundColor = DefaultTextColor;
            this.Heading1.ForegroundColor = DefaultTextColor;
            this.Heading1.BorderColor = DefaultSeparatorColor;
            this.Heading2.ForegroundColor = DefaultTextColor;
            this.Heading2.BorderColor = DefaultSeparatorColor;
            this.Heading3.ForegroundColor = DefaultTextColor;
            this.Heading4.ForegroundColor = DefaultTextColor;
            this.Heading5.ForegroundColor = DefaultTextColor;
            this.Heading6.ForegroundColor = DefaultTextColor;
            this.Link.ForegroundColor = DefaultAccentColor;
            this.Code.ForegroundColor = DefaultTextColor;
            this.Code.BackgroundColor = DefaultCodeBackground;
            this.Quote.ForegroundColor = DefaultQuoteTextColor;
            this.Quote.BorderColor = DefaultQuoteBorderColor;
            this.Separator.BorderColor = DefaultSeparatorColor;
        }

        public static readonly Color DefaultBackgroundColor = Color.FromHex("#ffffff");

        public static readonly Color DefaultAccentColor = Color.FromHex("#0366d6");

        public static readonly Color DefaultTextColor = Color.FromHex("#24292e");

        public static readonly Color DefaultCodeBackground = Color.FromHex("#f6f8fa");

        public static readonly Color DefaultSeparatorColor = Color.FromHex("#eaecef");

        public static readonly Color DefaultQuoteTextColor = Color.FromHex("#6a737d");

        public static readonly Color DefaultQuoteBorderColor = Color.FromHex("#dfe2e5");
    }
}

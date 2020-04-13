using System;
using DocumentFormat.OpenXml.Wordprocessing;
using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Paragraphs.Builders
{
    internal static class Extensions
    {
        public static bool OverridesFont(this Run run)
        {
            return run.RunProperties.OverridesFont();
        }

        public static bool OverridesFont(this RunProperties properties)
        {
            return !(
                properties.FontSize == null
                && properties.Bold == null
                && properties.Italic == null
                && properties.Underline == null
                && properties.Strike == null)
                ;
        }

        public static XFont CreateRunFont(this RunProperties properties, XFont defaultFont)
        {
            if (!properties.OverridesFont())
            {
                return defaultFont;
            }

            var fontSize = properties.ToXFontSize(defaultFont);
            var fontStyle = properties.ToXFontStyle();

            return new XFont(
                    defaultFont.FontFamily.Name,
                    fontSize,
                    fontStyle,
                    defaultFont.PdfOptions);
        }

        public static double ToXFontSize(this RunProperties properties, XFont defaultFont)
        {
            if (properties.FontSize == null)
            {
                return defaultFont.Size;
            }

            var d = Convert.ToInt32(properties.FontSize.Val.Value);
            return d / 2d;
        }

        public static XFontStyle ToXFontStyle(this RunProperties properties)
        {
            var style = XFontStyle.Regular;
            if (properties?.Bold?.Val ?? false)
            {
                style |= XFontStyle.Bold;
            }

            if (properties?.Italic?.Val ?? false)
            {
                style |= XFontStyle.Italic;
            }

            if (properties?.Strike?.Val ?? false)
            {
                style |= XFontStyle.Strikeout;
            }

            if ((properties?.Underline?.Val.Value ?? UnderlineValues.None) != UnderlineValues.None)
            {
                style |= XFontStyle.Underline;
            }

            return style;
        }
    }
}

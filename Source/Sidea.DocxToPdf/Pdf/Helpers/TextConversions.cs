using System.Globalization;
using PdfSharp.Drawing;
using Sidea.DocxToPdf.Core;

using Word = DocumentFormat.OpenXml.Wordprocessing;

namespace Sidea.DocxToPdf.Pdf
{
    internal static class TextConversions
    {
        public static XFont ToXFont(this TextStyle textStyle)
        {
            var f = textStyle.Font;
            return new XFont(f.FontFamily.Name, f.Size, (XFontStyle)f.Style);
        }

        public static XBrush ToXBrush(this TextStyle textStyle)
        {
            var color = XColor.FromArgb(textStyle.Brush.ToArgb());
            return new XSolidBrush(color);
        }

        public static XBrush ToXBrush(this Word.Color color)
        {
            var brushColor = color.ToXColor();

            XBrush brush = new XSolidBrush(brushColor);
            return brush;
        }

        public static XPen GetXPen(this Line line)
        {
            return new XPen(line.Color.ToXColor(), line.Width);
        }

        private static XColor ToXColor(this Word.Color color)
        {
            var hex = color?.Val?.Value;
            return hex.ToXColor();
        }

        private static XColor ToXColor(this string hex)
        {
            if (string.IsNullOrWhiteSpace(hex) || hex == "auto")
            {
                return XColor.FromArgb(0, 0, 0);
            }

            var (r, g, b) = hex.ToRgb();
            return XColor.FromArgb(r, g, b);
        }

        private static (int r, int g, int b) ToRgb(this string hex)
        {
            var r = int.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
            var g = int.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
            var b = int.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
            return (r, g, b);
        }
    }
}

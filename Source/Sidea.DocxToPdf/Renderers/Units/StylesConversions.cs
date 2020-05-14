using System;
using System.Globalization;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers
{
    internal static class StylesConversions
    {
        public static XColor ToXColor(this StringValue color)
        {
            var hex = color?.Value;
            var result = hex.ToXColor();
            return result;
        }

        public static XBrush ToXBrush(this Color color)
        {
            var hex = color?.Val?.Value;
            var brushColor = hex.ToXColor();

            XBrush brush = new XSolidBrush(brushColor);
            return brush;
        }

        private static XColor ToXColor(this string hex)
        {
            if(string.IsNullOrWhiteSpace(hex) || hex == "auto")
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

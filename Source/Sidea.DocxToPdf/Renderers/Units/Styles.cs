using System;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers
{
    internal static class Styles
    {
        public static XColor ToXColor(this StringValue color)
        {
            var hex = color?.Value;
            if (hex == null || hex == "auto")
            {
                return XColor.FromArgb(255, 0, 0, 0);
            }

            var r = int.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);// Convert.ToInt32($"0x{color.Value.Substring(0, 2)}");
            var g = int.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);// Convert.ToInt32($"0x{color.Value.Substring(2, 2)}");
            var b = int.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);// Convert.ToInt32($"0x{color.Value.Substring(4, 2)}");

            return XColor.FromArgb(255, r, g, b);
        }

        public static XBrush ToXBrush(this Color color)
        {
            var hex = color?.Val?.Value ?? "000000";
            var r = Convert.ToInt32(hex.Substring(0, 2), 16);
            var g = Convert.ToInt32(hex.Substring(2, 2), 16);
            var b = Convert.ToInt32(hex.Substring(4, 2), 16);

            XBrush brush = new XSolidBrush(XColor.FromArgb(r, g, b));
            return brush;
        }
    }
}

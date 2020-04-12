using System;
using DocumentFormat.OpenXml.Wordprocessing;
using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers
{
    internal static class Extensions
    {
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

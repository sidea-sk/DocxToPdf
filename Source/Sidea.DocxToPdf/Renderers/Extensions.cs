using System;
using DocumentFormat.OpenXml.Wordprocessing;
using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers
{
    internal static class Extensions
    {
        public static XUnit ToXUnit(this TableCellWidth width)
        {
            // Nil = 0,
            // Summary:
            //     Width in Fiftieths of a Percent.
            //     When the item is serialized out as xml, its value is "pct".
            // Pct = 1,
            // Summary:
            //     
            //     When the item is serialized out as xml, its value is "dxa".
            // Dxa = 2,
            // Summary:
            //     Automatically Determined Width.
            //     When the item is serialized out as xml, its value is "auto".
            // Auto = 3

            switch (width.Type.Value)
            {
                case TableWidthUnitValues.Nil:
                    break;
                case TableWidthUnitValues.Pct: // Width in Fiftieths of a Percent
                    break;
                case TableWidthUnitValues.Dxa: // Width in Twentieths of a Point.
                    var v = Convert.ToInt32(width.Width.Value);
                    return XUnit.FromPoint(v / 20d);
                case TableWidthUnitValues.Auto:
                    break;
                default:
                    break;
            }

            return XUnit.FromPoint(0);
        }

        public static XUnit ToXUnit(this TableWidthType width)
        {

            //var v = Convert.ToInt32(doublePoint);
            //return XUnit.FromPoint(v / 2d);
            return XUnit.FromPoint(0);
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

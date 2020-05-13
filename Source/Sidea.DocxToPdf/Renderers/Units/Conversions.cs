using System;
using DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers
{
    internal static class Conversions
    {
        // private const double PERCENT = 50;
        // private const double INCH = 72;
        // private const double CM = INCH * 2.54d;

        // public static XUnit ToXUnit(this TableWidthType width) => width.ToXUnit(XUnit.Zero);

        //public static XUnit ToXUnit(this TableWidthType width, XUnit outOf)
        //{
        //    // Nil = 0,
        //    // Summary:
        //    //     Width in Fiftieths of a Percent.
        //    //     When the item is serialized out as xml, its value is "pct".
        //    // Pct = 1,
        //    // Summary:
        //    //     
        //    //     When the item is serialized out as xml, its value is "dxa".
        //    // Dxa = 2,
        //    // Summary:
        //    //     Automatically Determined Width.
        //    //     When the item is serialized out as xml, its value is "auto".
        //    // Auto = 3

        //    switch (width.Type.Value)
        //    {
        //        case TableWidthUnitValues.Nil:
        //            break;
        //        case TableWidthUnitValues.Pct:
        //            var p = Convert.ToInt32(width.Width.Value);
        //            return outOf * p / PERCENT / 100;
        //        case TableWidthUnitValues.Dxa:
        //            var v = Convert.ToInt32(width.Width.Value);
        //            return v.DxaToPoint();
        //        case TableWidthUnitValues.Auto:
        //            break;
        //        default:
        //            break;
        //    }

        //    return XUnit.FromPoint(0);
        //}

        public static XUnit ToXUnit(this PositionOffset positionOffset)
        {
            var offset = Convert.ToInt64(positionOffset.Text);
            return offset.EmuToXUnit();
        }
    }
}

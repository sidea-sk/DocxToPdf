using System;
using DocumentFormat.OpenXml;
using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers
{
    internal static class HalfPoint
    {
        private const double Factor = 2;

        public static XUnit HPToPoint(this StringValue value, XUnit ifNull)
        {
            if(value?.Value == null)
            {
                return ifNull;
            }

            var v = Convert.ToInt32(value.Value);
            return v.HPToPoint();
        }

        public static XUnit HPToPoint(this int value)
            => value.ToXUnit(Factor);
    }
}

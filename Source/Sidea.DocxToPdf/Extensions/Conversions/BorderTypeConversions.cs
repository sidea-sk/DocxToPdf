using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using Sidea.DocxToPdf.Core;
using Word = DocumentFormat.OpenXml.Wordprocessing;

namespace Sidea.DocxToPdf
{
    internal static class BorderTypeConversions
    {
        public static Pen ToPen(this Word.BorderType border, Pen defaultIfNull = null)
        {
            if (border == null)
            {
                return defaultIfNull;
            }

            var color = border.Color.ToColor();
            var width = border.Size.EpToPoint();
            var val = border.Val?.Value ?? Word.BorderValues.Single;
            var pen = new Pen(color, width);
            pen.UpdateStyle(val);
            return pen;
        }

        private static void UpdateStyle(this Pen pen, Word.BorderValues borderValue)
        {
            switch (borderValue)
            {
                case Word.BorderValues.Nil:
                case Word.BorderValues.None:
                    pen.Color = Color.Transparent;
                    pen.Width = 0;
                    break;
                case Word.BorderValues.Single:
                case Word.BorderValues.Thick:
                    pen.DashStyle = DashStyle.Solid;
                    break;
                case Word.BorderValues.Dotted:
                    pen.DashStyle = DashStyle.Dot;
                    break;
                case Word.BorderValues.DashSmallGap:
                case Word.BorderValues.Dashed:
                    pen.DashStyle = DashStyle.Dash;
                    break;
                case Word.BorderValues.DotDash:
                    pen.DashStyle = DashStyle.DashDot;
                    break;
                case Word.BorderValues.DotDotDash:
                    pen.DashStyle = DashStyle.DashDotDot;
                    break;
            }
        }
    }
}

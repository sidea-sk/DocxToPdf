using DocumentFormat.OpenXml.Wordprocessing;
using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Borders
{
    internal static class BordeStyleExtensions
    {
        public static BorderStyle GetBorder(
            TopBorder top,
            RightBorder right,
            BottomBorder bottom,
            LeftBorder left)
        {
            var topPen = top.ToXPen();
            var rightPen = right.ToXPen();
            var bottomPen = bottom.ToXPen();
            var leftPen = left.ToXPen();

            return new BorderStyle(topPen, rightPen, bottomPen, leftPen);
        }

        public static BorderStyle GetBorder(this TableCellBorders borders)
        {
            if (borders == null)
            {
                return BorderStyle.Default;
            }

            var borderStyle = GetBorder(borders.TopBorder, borders.RightBorder, borders.BottomBorder, borders.LeftBorder);
            return borderStyle;
        }

        public static XPen ToXPen(this BorderType border, XPen defaultIfNull = null)
        {
            if(border == null)
            {
                return defaultIfNull;
            }

            var color = border.Color.ToXColor();
            var width = border.Size.EpToXUnit();
            var val = border.Val?.Value ?? BorderValues.Single;
            var pen = new XPen(color, width);
            pen.UpdateStyle(val);
            return pen;
        }

        private static void UpdateStyle(this XPen pen, BorderValues borderValue)
        {
            switch (borderValue)
            {
                case BorderValues.Nil:
                case BorderValues.None:
                    pen.Color = XColors.Transparent;
                    pen.Width = 0;
                    break;
                case BorderValues.Single:
                case BorderValues.Thick:
                    pen.DashStyle = XDashStyle.Solid;
                    break;
                case BorderValues.Dotted:
                    pen.DashStyle = XDashStyle.Dot;
                    break;
                case BorderValues.DashSmallGap:
                case BorderValues.Dashed:
                    pen.DashStyle = XDashStyle.Dash;
                    break;
                case BorderValues.DotDash:
                    pen.DashStyle = XDashStyle.DashDot;
                    break;
                case BorderValues.DotDotDash:
                    pen.DashStyle = XDashStyle.DashDotDot;
                    break;
            }
        }
    }
}

using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Tables.Models
{
    internal class RPadding
    {
        private RPadding(XUnit top, XUnit right, XUnit bottom, XUnit left)
        {
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
            this.Left = left;
        }

        public XUnit Top { get; }
        public XUnit Right { get; }
        public XUnit Bottom { get; }
        public XUnit Left { get; }

        public XUnit HorizonalPaddings => this.Left + this.Right;
        public XUnit VerticalPaddings => this.Top + this.Bottom;

        public static RPadding Padding(XUnit padding) => new RPadding(padding, padding, padding, padding);
        public static RPadding Padding(XUnit top, XUnit right, XUnit bottom, XUnit left) => new RPadding(top, right, bottom, left);
    }
}

using System.Drawing;

namespace Sidea.DocxToPdf.Models.Tables.Elements
{
    internal class BorderStyle
    {
        public BorderStyle(Pen top,Pen right, Pen bottom, Pen left)
        {
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
            this.Left = left;
        }

        public Pen Top { get; }
        public Pen Right { get; }
        public Pen Bottom { get; }
        public Pen Left { get; }
    }
}

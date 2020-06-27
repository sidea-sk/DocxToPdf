using System.Drawing;

namespace Sidea.DocxToPdf.Core
{
    internal class Line
    {
        public Line(
            Point start,
            Point end,
            double? width = null,
            Color? color = null
            /*, style*/)
        {
            this.Start = start;
            this.End = end;
            this.Color = color ?? Color.Black;
            this.Width = width ?? 0.5;
        }

        public Point Start { get; }
        public Point End { get; }
        public Color Color { get; }
        public double Width { get; }
    }
}

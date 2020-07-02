using System.Drawing;

namespace Sidea.DocxToPdf.Core
{
    internal class Line
    {
        public Line(
            Point start,
            Point end,
            Pen pen = null)
        {
            this.Start = start;
            this.End = end;
            this.Pen = pen;
        }

        public Line(
            Point start,
            Point end,
            float? width = null,
            Color? color = null)
        {
            this.Start = start;
            this.End = end;

            var c = color ?? Color.Black;
            var w = width ?? 0.5f;
            this.Pen = new Pen(c, w);
        }

        public Point Start { get; }
        public Point End { get; }
        //public Color Color { get; }
        //public double Width { get; }
        public Pen Pen { get; }
    }
}

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

        public Point Start { get; }
        public Point End { get; }
        public Pen Pen { get; }
    }
}

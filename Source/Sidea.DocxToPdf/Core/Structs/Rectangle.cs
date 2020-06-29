using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Drawing = System.Drawing;

namespace Sidea.DocxToPdf.Core
{
    [DebuggerDisplay("{TopLeft} - {BottomRight}: {Width}x{Height}")]
    internal struct Rectangle
    {
        public static readonly Rectangle Empty = new Rectangle(0, 0, 0, 0);

        public Rectangle(double x, double y, double width, double height)
        {
            if (width < 0 || height < 0)
            {
                throw new RendererException("Width and Height must be 0 or positive");
            }

            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;

            this.TopLeft = new Point(this.X, this.Y);
            this.TopRight = new Point(this.X + this.Width, this.Y);
            this.BottomRight = new Point(this.X + this.Width, this.Y + this.Height);
            this.BottomLeft = new Point(this.X, this.Y + this.Height);

            this.Size = new Size(this.Width, this.Height);
        }

        public Rectangle(double x, double y, Size size) : this(x, y, size.Width, size.Height)
        {
        }

        public Rectangle(Point position, double width, double height) : this(position.X, position.Y, width, height)
        {
        }

        public Rectangle(Point position, Size size) : this(position.X, position.Y, size)
        {
        }

        public Rectangle(Size size) : this(0, 0, size.Width, size.Height)
        {
        }

        public double X { get; }
        public double Y { get; }
        public double Width { get; }
        public double Height { get; }

        public double BottomY => this.Y + this.Height;

        public Point TopLeft { get; }
        public Point TopRight { get; }
        public Point BottomRight { get; }
        public Point BottomLeft { get; }

        public Line TopLine(Drawing.Color? color)
            => new Line(this.TopLeft, this.TopRight, color: color);

        public Line RightLine(Drawing.Color? color)
            => new Line(this.TopRight, this.BottomRight, color: color);

        public Line BottomLine(Drawing.Color? color)
            => new Line(this.BottomRight, this.BottomLeft, color: color);

        public Line LeftLine(Drawing.Color? color)
            => new Line(this.BottomLeft, this.TopLeft, color: color);

        public Line TopLeftBottomRightDiagonal(Drawing.Color? color)
            => new Line(this.TopLeft, this.BottomRight, color: color);

        public Line BottomLeftTopRightDiagonal(Drawing.Color? color)
            => new Line(this.BottomLeft, this.TopRight, color: color);

        public Size Size { get; }

        public Rectangle Pan(Point vector)
            => this.Pan(vector.X, vector.Y);

        public Rectangle Pan(double dx, double dy)
        {
            return new Rectangle(this.X + dx, this.Y + dy, this.Width, this.Height);
        }

        public Rectangle ExpandWidth(double width)
            => this.Expand(width, 0);

        public Rectangle ExpandHeight(double height)
            => this.Expand(0, height);

        public Rectangle Expand(double width, double height)
            => new Rectangle(this.X, this.Y, this.Width + width, this.Height + height);

        public Rectangle RestrictLeftRight(double left, double right)
            => this.RestrictLeftWidth(left, this.Width - left - right);

        public Rectangle RestrictLeftWidth(double left, double width)
        {
            return new Rectangle(this.X + left, this.Y, width, this.Height);
        }

        public Rectangle Shrink(double top, double bottom)
        {
            return new Rectangle(this.X, this.Y + top, this.Width, this.Height - top - bottom);
        }

        public Rectangle Clip(Point topLeft)
        { 
            var width = this.Width - (topLeft.X - this.X);
            var height = this.Height - (topLeft.Y - this.Y);

            return new Rectangle(topLeft, width, height);
        }

        public Rectangle Union(params Rectangle[] rectangles)
        {
            if (rectangles.Length == 0)
            {
                return this;
            }

            return Union(rectangles.Union(new[] { this }));
        }

        public static Rectangle Union(IEnumerable<Rectangle> rectangles)
        {
            var x = double.MaxValue;
            var y = double.MaxValue;
            var rx = double.MinValue;
            var ry = double.MinValue;
            var count = 0;
            foreach (var r in rectangles)
            {
                count++;
                x = Math.Min(x, r.X);
                y = Math.Min(y, r.Y);
                rx = Math.Max(rx, r.TopRight.X);
                ry = Math.Max(ry, r.BottomRight.Y);
            }

            return count == 0
                ? Empty
                : new Rectangle(x, y, rx - x, ry - y);
        }
    }
}

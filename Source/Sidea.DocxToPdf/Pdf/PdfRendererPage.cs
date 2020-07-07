using System.IO;
using PdfSharp.Drawing;
using Sidea.DocxToPdf.Core;
using Drawing = System.Drawing;

namespace Sidea.DocxToPdf.Pdf
{
    internal class PdfRendererPage : IRendererPage
    {
        private readonly XGraphics _graphics;
        private readonly Point _offset;

        private PdfRendererPage(PageNumber pageNumber, XGraphics graphics, RenderingOptions options, Point offset)
        {
            this.PageNumber = pageNumber;
            _graphics = graphics;
            this.Options = options;
            _offset = offset;
        }

        public PdfRendererPage(PageNumber pageNumber, XGraphics graphics, RenderingOptions options)
            : this(pageNumber, graphics, options, Point.Zero)
        {
        }

        public PageNumber PageNumber { get; }
        public RenderingOptions Options { get; }

        public void RenderText(string text, TextStyle textStyle, Rectangle layout)
        {
            var rect = layout.Pan(_offset).ToXRect();
            _graphics.DrawString(text, textStyle.ToXFont(), textStyle.ToXBrush(), rect, XStringFormats.TopLeft);
        }

        public void RenderRectangle(Rectangle rectangle, Drawing.Color brush)
        {
            var rect = rectangle.Pan(_offset).ToXRect();
            _graphics.DrawRectangle(brush.ToXBrush(), rect);
        }

        public void RenderLine(Line line)
        {
            var start = (line.Start + _offset).ToXPoint();
            var end = (line.End + _offset).ToXPoint();

            _graphics.DrawLine(line.GetXPen(), start, end);
        }

        public void RenderImage(Stream imageStream, Point position, Size size)
        {
            if (imageStream == null)
            {
                this.RenderNoImagePlaceholder(position, size);
                return;
            }

            Drawing.Image bmp = new Drawing.Bitmap(imageStream);
            using (var ms = new MemoryStream())
            {
                bmp.Save(ms, bmp.RawFormat);
                var image = XImage.FromStream(ms);
                var offsetPosition = position + _offset;
                _graphics.DrawImage(image, offsetPosition.X, offsetPosition.Y, size.Width, size.Height);
            }
        }

        private void RenderNoImagePlaceholder(Point position, Size size)
        {
            var rect = new Rectangle(position, size).Pan(_offset);
            var pen = new Drawing.Pen(Drawing.Color.Red, 0.5f);

            this.RenderLine(rect.TopLine(pen));
            this.RenderLine(rect.RightLine(pen));
            this.RenderLine(rect.BottomLine(pen));
            this.RenderLine(rect.LeftLine(pen));
            this.RenderLine(rect.TopLeftBottomRightDiagonal(pen));
            this.RenderLine(rect.BottomLeftTopRightDiagonal(pen));
        }

        public IRendererPage Offset(Point vector)
            => new PdfRendererPage(this.PageNumber, _graphics, Options, vector);
    }
}

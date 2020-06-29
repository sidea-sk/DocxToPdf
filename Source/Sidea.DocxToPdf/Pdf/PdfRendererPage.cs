using System.IO;
using PdfSharp.Drawing;
using Sidea.DocxToPdf.Core;
using Drawing = System.Drawing;

namespace Sidea.DocxToPdf.Pdf
{
    internal class PdfRendererPage : IRendererPage
    {
        private readonly XGraphics _graphics;

        public PdfRendererPage(PageNumber pageNumber, XGraphics graphics, RenderingOptions options)
        {
            this.PageNumber = pageNumber;
            _graphics = graphics;
            this.Options = options;
        }

        public PageNumber PageNumber { get; }
        public RenderingOptions Options { get; }

        public void RenderText(string text, TextStyle textStyle, Rectangle layout)
        {
            _graphics.DrawString(text, textStyle.ToXFont(), textStyle.ToXBrush(), layout.ToXRect(), XStringFormats.TopLeft);
        }

        public void RenderLine(Line line)
        {
            var start = line.Start.ToXPoint();
            var end = line.End.ToXPoint();

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
                _graphics.DrawImage(image, position.X, position.Y, size.Width, size.Height);
            }
        }

        private void RenderNoImagePlaceholder(Point position, Size size)
        {
            var rect = new Rectangle(position, size);
            this.RenderLine(rect.TopLine(Drawing.Color.Red));
            this.RenderLine(rect.RightLine(Drawing.Color.Red));
            this.RenderLine(rect.BottomLine(Drawing.Color.Red));
            this.RenderLine(rect.LeftLine(Drawing.Color.Red));
            this.RenderLine(rect.TopLeftBottomRightDiagonal(Drawing.Color.Red));
            this.RenderLine(rect.BottomLeftTopRightDiagonal(Drawing.Color.Red));
        }
    }
}

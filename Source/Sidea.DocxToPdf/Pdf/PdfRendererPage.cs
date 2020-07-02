﻿using System.IO;
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

        public void RenderRectangle(Rectangle rectangle, Drawing.Color brush)
        {
            _graphics.DrawRectangle(brush.ToXBrush(), rectangle.ToXRect());
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
            var pen = new Drawing.Pen(Drawing.Color.Red, 0.5f);

            this.RenderLine(rect.TopLine(pen));
            this.RenderLine(rect.RightLine(pen));
            this.RenderLine(rect.BottomLine(pen));
            this.RenderLine(rect.LeftLine(pen));
            this.RenderLine(rect.TopLeftBottomRightDiagonal(pen));
            this.RenderLine(rect.BottomLeftTopRightDiagonal(pen));
        }
    }
}

﻿using System.Drawing;
using System.IO;
using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Core.Services;

namespace Sidea.DocxToPdf.Renderers.Core.RenderingAreas
{
    internal class RenderArea :
        IPrerenderArea,
        IRenderArea
    {
        private readonly XGraphics _graphics;
        private readonly IImageAccessor _imageAccessor;
        private readonly XVector _translate;

        public RenderArea(
            XFont font,
            XGraphics graphics,
            XRect area,
            IImageAccessor imageAccessor,
            RenderingOptions renderingOptions)
        {
            _graphics = graphics;
            _translate = new XVector(area.X, area.Y);
            _imageAccessor = imageAccessor;

            this.AreaFont = font;
            this.AreaRectangle = area;
            this.Options = renderingOptions;
        }

        public XUnit Width => AreaRectangle.Width;

        public XUnit Height => AreaRectangle.Height;

        public XFont AreaFont { get; }

        public XRect AreaRectangle { get; }

        public RenderingOptions Options { get; }

        public void DrawLine(XPen pen, XPoint start, XPoint end)
        {
            _graphics.DrawLine(pen, start + _translate, end + _translate);
        }

        public void DrawText(string text, XFont font, XBrush brush, XPoint position)
        {
            _graphics.DrawString(text, font, brush, position + _translate);
        }

        public void DrawText(string text, XFont font, XBrush brush, XRect layout, XStringFormat stringFormat)
        {
            _graphics.DrawString(text, font, brush, XRect.Offset(layout, _translate), stringFormat);
        }

        public void DrawRectangle(XPen pen, XBrush brush, XRect rect)
        {
            _graphics.DrawRectangle(pen, brush, XRect.Offset(rect, _translate));
        }

        public void DrawImage(string imageId, XSize size)
        {
            var stream = _imageAccessor.GetImageStream(imageId);
            Image bmp = new Bitmap(stream);
            using(var ms = new MemoryStream())
            {
                bmp.Save(ms, bmp.RawFormat);
                var image = XImage.FromStream(ms);
                _graphics.DrawImage(image, new XRect(new XPoint(0,0) + _translate, size));
            }
        }

        public XSize MeasureText(string text, XFont font) => _graphics.MeasureString(text, font);

        IRenderArea IRenderArea.PanLeft(XUnit unit) => this.PanLeftCore(unit);

        IRenderArea IRenderArea.PanDown(XUnit height) => this.PanLeftDownCore(new XSize(0, height));

        IRenderArea IRenderArea.PanLeftDown(XUnit width, XUnit height) => this.PanLeftDownCore(new XSize(width, height));

        IRenderArea IRenderArea.PanLeftDown(XSize size) => this.PanLeftDownCore(size);

        IPrerenderArea IPrerenderArea.Restrict(XUnit width) => this.RestrictCore(width);

        IRenderArea IRenderArea.Restrict(XUnit width) => this.RestrictCore(width);

        IRenderArea IRenderArea.Restrict(XUnit width, XUnit height) => this.RestrictCore(new XSize(width, height));

        IRenderArea IRenderArea.RestrictFromBottom(XUnit height)
        {
            var r = new XRect(this.AreaRectangle.X, this.AreaRectangle.Y, this.AreaRectangle.Width, this.AreaRectangle.Height - height);
            return new RenderArea(this.AreaFont, _graphics, r, _imageAccessor, this.Options);
        }

        private RenderArea PanLeftCore(XUnit unit)
        {
            return new RenderArea(
                AreaFont,
                _graphics,
                new XRect(AreaRectangle.X + unit, AreaRectangle.Y, AreaRectangle.Width - unit, AreaRectangle.Height),
                _imageAccessor,
                this.Options);
        }

        private RenderArea PanLeftDownCore(XSize size)
        {
            // check XRect methods Offset, Inflate, etc.
            return new RenderArea(
                AreaFont,
                _graphics,
                new XRect(AreaRectangle.X + size.Width, AreaRectangle.Y + size.Height, AreaRectangle.Width - size.Width, AreaRectangle.Height - size.Height),
                _imageAccessor,
                this.Options);
        }

        private RenderArea RestrictCore(XUnit width)
        {
            return new RenderArea(
                 AreaFont,
                 _graphics,
                 new XRect(AreaRectangle.X, AreaRectangle.Y, width, AreaRectangle.Height),
                 _imageAccessor,
                 this.Options);
        }

        private RenderArea RestrictCore(XSize size)
        {
            return new RenderArea(
                 AreaFont,
                 _graphics,
                 new XRect(AreaRectangle.X, AreaRectangle.Y, size.Width, size.Height),
                 _imageAccessor,
                 this.Options);
        }
    }
}

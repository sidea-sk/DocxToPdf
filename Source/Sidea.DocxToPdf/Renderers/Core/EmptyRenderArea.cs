﻿using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Core
{
    internal class EmptyRenderArea : IRenderArea
    {
        public double Width => 0;

        public double Height => 0;

        public XFont AreaFont { get; } = new XFont("Arial", 10);

        public void DrawText(string text, XFont font, XBrush brush, XPoint position)
        {
        }

        public XSize MeasureText(string text, XFont font) => new XSize(0, 0);

        public IRenderArea PanLeft(double x) => this;

        public IRenderArea PanLeftDown(XSize size) => this;
    }
}

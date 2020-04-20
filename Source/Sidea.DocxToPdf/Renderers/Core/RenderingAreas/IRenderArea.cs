﻿using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Core.RenderingAreas
{
    internal interface IRenderArea : IRenderingAreaBase
    {
        void DrawText(string text, XFont font, XBrush brush, XPoint position);
        void DrawText(string text, XFont font, XBrush brush, XRect layout, XStringFormat stringFormat);

        void DrawLine(XPen pen, XPoint start, XPoint end);

        void DrawRectangle(XPen pen, XBrush brush, XRect rect);

        IRenderArea PanLeft(XUnit width);
        IRenderArea PanDown(XUnit height);
        IRenderArea PanLeftDown(XUnit width, XUnit height);
        IRenderArea PanLeftDown(XSize size);
        IRenderArea Restrict(XUnit width);
    }
}

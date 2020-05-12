using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Sections.Models
{
    internal class ColumnProperties
    {
        public ColumnProperties(XUnit width, XUnit spaceBetween)
        {
            this.Width = width;
            this.SpaceBetween = spaceBetween;
        }

        public XUnit Width { get; }
        public XUnit SpaceBetween { get; }
    }
}

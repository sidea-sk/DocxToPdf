using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Sections.Models
{
    internal class SectionColumn
    {
        public SectionColumn(XUnit width, XUnit space)
        {
            this.Width = width;
            this.Space = space;
        }

        public XUnit Width { get; }
        public XUnit Space { get; }
    }
}

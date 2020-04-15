using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Tables.Models
{
    internal class RTable
    {
        private readonly Alignment _alignment;
        private readonly XUnit _width;

        public RTable(Alignment alignment, XUnit width)
        {
            _alignment = alignment;
            _width = width;
        }
    }
}

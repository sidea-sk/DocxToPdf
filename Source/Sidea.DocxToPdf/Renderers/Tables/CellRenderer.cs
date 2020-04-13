using DocumentFormat.OpenXml.Wordprocessing;
using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Core;

namespace Sidea.DocxToPdf.Renderers.Tables
{
    internal class CellRenderer : IRenderer
    {
        private readonly TableCell _tableCell;

        public CellRenderer(TableCell tableCell)
        {
            _tableCell = tableCell;
        }

        public RenderingState Render(IRenderArea renderArea)
        {
            return new RenderingState(RenderingStatus.Error, new XPoint(0, 0));
        }
    }
}

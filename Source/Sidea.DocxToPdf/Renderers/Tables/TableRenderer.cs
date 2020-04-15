using DocumentFormat.OpenXml.Wordprocessing;
using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Core;
using Sidea.DocxToPdf.Renderers.Tables.Builders;

namespace Sidea.DocxToPdf.Renderers.Tables
{
    internal class TableRenderer : IRenderer
    {
        private readonly Table _table;

        public TableRenderer(Table table)
        {
            _table = table;
        }

        public RenderingState Prepare(IRenderArea renderArea)
        {
            return new RenderingState(RenderingStatus.Done, new XPoint(0d, 0d));
        }

        public RenderingState Render(IRenderArea renderArea)
        {
            var tableGrid = _table.InitializeGrid();
            var rowIndex = 0;
            foreach (var row in _table.Rows())
            {
                foreach (var cell in row.RCells(rowIndex, tableGrid))
                {
                    var state = cell.Render(renderArea);
                }

                rowIndex++;
            }

            return new RenderingState(RenderingStatus.Done, new XPoint(0, rowIndex * XUnit.FromPoint(10)));
        }
    }
}

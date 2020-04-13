using System.Linq;
using DocumentFormat.OpenXml.Wordprocessing;
using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Borders;
using Sidea.DocxToPdf.Renderers.Core;

namespace Sidea.DocxToPdf.Renderers.Tables
{
    internal class TableRenderer : IRenderer
    {
        private readonly Table _table;

        public TableRenderer(Table table)
        {
            _table = table;
        }

        public RenderingState Render(IRenderArea renderArea)
        {
            var rowHeight = XUnit.FromPoint(10);
            var tableProperties = _table.Properties();

            var dy = 0.0d;
            foreach (var row in _table.Rows())
            {
                var dx = 0d;
                var rowProperties = row.Properties();

                var cells = row.Cells().ToArray();
                foreach (var cell in cells)
                {
                    var cellProp = cell.Properties();
                    var width = cellProp.TableCellWidth.ToXUnit();
                    var border = new RBorder(XPens.Black, XPens.Black, XPens.Black, XPens.Black, new XRect(dx, dy, width, rowHeight));
                    border.Render(renderArea);
                    dx += width;
                }

                dy += rowHeight;
            }

            return new RenderingState(RenderingStatus.Done, new XPoint(0, dy));
        }
    }
}

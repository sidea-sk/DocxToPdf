using System.Collections.Generic;
using DocumentFormat.OpenXml.Wordprocessing;
using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Core;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;
using Sidea.DocxToPdf.Renderers.Tables.Builders;
using Sidea.DocxToPdf.Renderers.Tables.Models;

namespace Sidea.DocxToPdf.Renderers.Tables
{
    internal class TableRenderer : IRenderer
    {
        private readonly Table _table;
        private readonly RGrid _grid;
        private readonly List<RCell> _cells = new List<RCell>();

        private RLayout _layout = null;

        public TableRenderer(Table table)
        {
            _table = table;
            _grid = _table.InitializeGrid();
        }

        public XSize TotalArea { get; } = new XSize(0, 0);

        public RenderingState Prepare(IPrerenderArea renderArea)
        {
            var rowIndex = 0;

            foreach (var row in _table.Rows())
            {
                foreach (var cell in row.RCells(rowIndex, _grid))
                {
                    _cells.Add(cell);
                    var state = cell.Prepare(renderArea);
                }

                rowIndex++;
            }

            return RenderingState.Done(new XRect(new XSize(0,0)));
        }

        public RenderingState Render(IRenderArea renderArea)
        {
            if(_layout == null)
            {
                _layout = new RLayout(_grid, _cells);
            }

            _layout.Render(renderArea);

            return RenderingState.Done(new XRect(_layout.TotalArea));
        }
    }
}

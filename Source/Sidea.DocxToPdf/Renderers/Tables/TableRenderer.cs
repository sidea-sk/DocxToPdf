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
        private readonly RenderingOptions _renderingOptions;
        private readonly RGrid _grid;
        private readonly List<RCell> _cells = new List<RCell>();

        private RLayout _layout = null;

        public TableRenderer(Table table, RenderingOptions renderingOptions)
        {
            _table = table;
            _renderingOptions = renderingOptions;
            _grid = _table.InitializeGrid();
        }

        public XSize TotalArea { get; } = new XSize(0, 0);

        public XSize CalculateContentSize(IPrerenderArea prerenderArea)
        {
            var rowIndex = 0;

            foreach (var row in _table.Rows())
            {
                foreach (var cell in row.RCells(rowIndex, _grid, _renderingOptions))
                {
                    _cells.Add(cell);
                }

                rowIndex++;
            }

            _layout = new RLayout(_grid, _cells);
            var size = _layout.CalculateContentSize(prerenderArea);
            return size;
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

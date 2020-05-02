using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Wordprocessing;
using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Core;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;
using Sidea.DocxToPdf.Renderers.Tables.Builders;
using Sidea.DocxToPdf.Renderers.Tables.Models;

namespace Sidea.DocxToPdf.Renderers.Tables
{
    internal class TableRenderer : RendererBase
    {
        private readonly Table _table;
        private readonly RenderingOptions _renderingOptions;
        private readonly RGrid _grid;
        private RCell[] _cells = new RCell[0];

        private RLayout _layout = null;

        public TableRenderer(Table table, RenderingOptions renderingOptions)
        {
            _table = table;
            _renderingOptions = renderingOptions;
            _grid = _table.InitializeGrid();
        }

        protected override sealed XSize CalculateContentSizeCore(IPrerenderArea prerenderArea)
        {
            _cells = _table
                .RCells(_grid, _renderingOptions)
                .ToArray();

            _layout = new RLayout(_grid, _cells);
            _layout.CalculateContentSize(prerenderArea);
            return _layout.PrecalulatedSize;
        }

        protected override sealed RenderResult RenderCore(IRenderArea renderArea)
        {
            _layout.Render(renderArea);
            return _layout.RenderResult;
        }
    }
}

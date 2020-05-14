using System.Linq;
using DocumentFormat.OpenXml.Wordprocessing;
using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Core;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;
using Sidea.DocxToPdf.Renderers.Styles;
using Sidea.DocxToPdf.Renderers.Tables.Builders;
using Sidea.DocxToPdf.Renderers.Tables.Models;

namespace Sidea.DocxToPdf.Renderers.Tables
{
    internal class TableRenderer : RendererBase
    {
        private readonly Table _table;
        private readonly IStyleAccessor _styleAccessor;
        private RLayout _layout = null;

        public TableRenderer(Table table, IStyleAccessor styleAccessor)
        {
            _table = table;
            _styleAccessor = styleAccessor;
        }

        protected override sealed XSize CalculateContentSizeCore(IPrerenderArea prerenderArea)
        {
            var grid = _table.InitializeGrid();
            var cells = _table
                .RCells(grid, _styleAccessor)
                .ToArray();

            var tableBorder = _table.Properties().TableBorders.GetBorder();

            _layout = new RLayout(grid, cells, tableBorder);
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

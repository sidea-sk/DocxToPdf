using System;
using System.Linq;
using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Styles;
using Sidea.DocxToPdf.Models.Tables.Builders;
using Sidea.DocxToPdf.Models.Tables.Elements;
using Word = DocumentFormat.OpenXml.Wordprocessing;

namespace Sidea.DocxToPdf.Models.Tables
{
    internal class Table : ContainerElement
    {
        private readonly Word.Table _wordTable;
        private readonly IStyleFactory _styleFactory;
        private Grid _grid;
        private Cell[] _cells = new Cell[0];

        public Table(Word.Table wordTable, IStyleFactory styleFactory)
        {
            _wordTable = wordTable;
            _styleFactory = styleFactory;
        }

        public override void Initialize()
        {
            _grid = _wordTable.InitializeGrid();
            _cells = _wordTable
                .InitializeCells(_styleFactory)
                .OrderBy(c => c.GridPosition.Row)
                .ThenBy(c => c.GridPosition.Column)
                .ToArray();
        }

        public override void Prepare(PageContext pageContext, Func<PageNumber, ContainerElement, PageContext> pageFactory)
        {

            this.ResetPageRegionsFrom(_cells);
        }

        public override void Render(IRenderer renderer)
        {
            _cells.Render(renderer);
        }
    }
}

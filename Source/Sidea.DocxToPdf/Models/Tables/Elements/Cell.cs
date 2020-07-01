using System;
using System.Linq;
using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Common;
using Sidea.DocxToPdf.Models.Styles;
using Sidea.DocxToPdf.Models.Tables.Builders;

using Word = DocumentFormat.OpenXml.Wordprocessing;

namespace Sidea.DocxToPdf.Models.Tables.Elements
{
    internal class Cell : ContainerElement
    {
        private readonly Word.TableCell _wordCell;
        private readonly BorderStyle _borderStyle;
        private readonly IStyleFactory _styleFactory;

        private ContainerElement[] _childs = new ContainerElement[0];

        public Cell(
            Word.TableCell wordCell,
            GridPosition gridPosition,
            IStyleFactory styleFactory)
        {
            _wordCell = wordCell;

            this.GridPosition = gridPosition;

            _borderStyle = wordCell.GetBorderStyle();
            _styleFactory = styleFactory;
        }

        public GridPosition GridPosition { get; }

        public override void Initialize()
        {
            _childs = _wordCell
                .RenderableChildren()
                .CreateInitializeElements(_styleFactory)
                .ToArray();
        }

        public override void Prepare(PageContext pageContext, Func<PageNumber, ContainerElement, PageContext> pageFactory)
        {

        }

        public override void Render(IRenderer renderer)
        {
        }
    }
}

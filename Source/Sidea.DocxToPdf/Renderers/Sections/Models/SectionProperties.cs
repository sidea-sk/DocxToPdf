using System.Collections.Generic;
using System.Linq;
using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Common;

namespace Sidea.DocxToPdf.Renderers.Sections.Models
{
    internal class SectionProperties
    {
        public SectionProperties(
            PageConfiguration pageConfiguration,
            IEnumerable<SectionColumn> columns,
            RenderBehaviour renderBehaviour)
        {
            this.PageConfiguration = pageConfiguration;
            this.Columns = columns.ToArray();
            this.RenderBehaviour = renderBehaviour;
        }

        public PageConfiguration PageConfiguration { get; }
        public IReadOnlyCollection<SectionColumn> Columns { get; }
        public RenderBehaviour RenderBehaviour { get; }

        public XUnit ColumnLeftMargin(int columnIndex)
        {
            var result = this.Columns
                .Take(columnIndex)
                .Aggregate(this.PageConfiguration.Margin.Left, (acc, column) =>
                {
                    return acc + column.Width + column.Space;
                });

            return result;
        }

        public XUnit ColumnWidth(int columnIndex)
        {
            return this.Columns.ElementAt(columnIndex).Width;
        }
    }
}

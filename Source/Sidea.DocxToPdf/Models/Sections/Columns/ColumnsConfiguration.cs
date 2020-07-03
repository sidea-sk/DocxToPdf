using System.Collections.Generic;
using System.Linq;
using Sidea.DocxToPdf.Models.Common;

namespace Sidea.DocxToPdf.Models.Sections.Columns
{
    internal class ColumnsConfiguration
    {
        private readonly ColumnConfig[] _columns;

        public ColumnsConfiguration(IEnumerable<ColumnConfig> columns)
        {
            _columns = columns.ToArray();
        }

        public HorizontalSpace CalculateColumnSpace(int columnIndex)
        {
            var xOffset = this.ColumnOffset(columnIndex);
            var width = this.ColumnWidth(columnIndex);

            return new HorizontalSpace(xOffset, width);
        }

        public PagePosition GetNextPagePosition(PagePosition currentPosition)
        {
            return currentPosition.PageColumn < _columns.Length - 1
                ? currentPosition.NextColumn()
                : currentPosition.NewPage();
        }

        private double ColumnOffset(int columnIndex)
        {
            var columnConfigIndex = columnIndex % _columns.Length;

            var result = _columns
                .Take(columnConfigIndex)
                .Aggregate(0.0, (acc, column) =>
                {
                    return acc + column.Width + column.Space;
                });

            return result;
        }

        private double ColumnWidth(int columnIndex)
        {
            var columnConfigIndex = columnIndex % _columns.Length;
            return _columns[columnConfigIndex].Width;
        }
    }
}

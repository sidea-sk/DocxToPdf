﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Sidea.DocxToPdf.Renderers.Tables.Models
{
    [DebuggerDisplay("{Column}-{Row}")]
    internal class GridPosition
    {
        private int[] _rowIndeces;

        public GridPosition(
            int row,
            int column,
            int rowSpan,
            int columnSpan,
            bool isLastVerticalCell)
        {
            this.Row = row;
            this.Column = column;
            this.RowSpan = rowSpan;
            this.ColumnSpan = columnSpan;
            this.IsLastVerticalCell = isLastVerticalCell;

            _rowIndeces = rowSpan > 0
                ? Enumerable.Range(row, rowSpan).ToArray()
                : new int[0];
        }

        public int Column { get; }
        public int ColumnSpan { get; }
        public bool IsFirstVerticalCell => this.RowSpan > 0;
        public bool IsLastVerticalCell { get; }
        public int Row { get; }
        public int RowSpan { get; }

        public IReadOnlyCollection<int> RowIndeces => _rowIndeces;

        public bool IsRowMergedCell => this.RowSpan <= 0;

        public bool IsInRow(int rowIndex)
        {
            return this.Row == rowIndex || _rowIndeces.Contains(rowIndex);
        }

        public bool IsInColumn(int column)
        {
            return this.Column <= column
                && this.Column + this.ColumnSpan - 1 >= column;
        }

        public bool IsFirstVerticalCellOfRow(int rowIndex)
        {
            return this.Row == rowIndex && this.IsFirstVerticalCell;
        }

        public bool IsLastVerticalCellOfRow(int rowIndex)
        {
            return this.Row == rowIndex && this.IsLastVerticalCell;
        }
    }
}

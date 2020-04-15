namespace Sidea.DocxToPdf.Renderers.Tables.Models
{
    internal class GridPosition
    {
        public GridPosition(
            int row,
            int column,
            int rowSpan,
            int columnSpan)
        {
            this.Row = row;
            this.Column = column;
            this.RowSpan = rowSpan;
            this.Span = columnSpan;
        }

        public int Row { get; }
        public int Column { get; }
        public int RowSpan { get; }
        public int Span { get; }
    }
}

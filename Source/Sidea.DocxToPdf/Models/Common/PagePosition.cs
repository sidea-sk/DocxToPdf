using System;
using Sidea.DocxToPdf.Core;

namespace Sidea.DocxToPdf.Models.Common
{
    internal class PagePosition : IEquatable<PagePosition>, IComparable<PagePosition>
    {
        public static readonly PagePosition None = new PagePosition(PageNumber.None, -1, 0);
        private readonly int _totalColumns;

        public PagePosition(PageNumber pageNumber, int column, int totalColumns)
        {
            this.PageNumber = pageNumber;
            this.PageColumn = column;
            _totalColumns = totalColumns;
        }

        public PageNumber PageNumber { get; }

        public int PageColumn { get; }

        public PagePosition Next()
        {
            if(_totalColumns <= 0)
            {
                throw new Exception("Total Columns is zero");
            }

            return this.PageColumn == _totalColumns - 1
                ? this.NewPage()
                : this.NextColumn();
        }

        private PagePosition NextColumn()
            => new PagePosition(this.PageNumber, this.PageColumn + 1, _totalColumns);

        private PagePosition NewPage()
            => new PagePosition(this.PageNumber.Next(), 0, _totalColumns);

        public int CompareTo(PagePosition other)
        {
            var pnSign = this.PageNumber.CompareTo(other.PageNumber);
            if(pnSign != 0)
            {
                return pnSign;
            }

            return this.PageColumn - other.PageColumn;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.PageNumber.GetHashCode(), this.PageColumn.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as PagePosition);
        }

        public bool Equals(PagePosition other)
        {
            return other is object
                && other.PageNumber == this.PageNumber
                && other.PageColumn == this.PageColumn;
        }

        public static bool operator ==(PagePosition p1, PagePosition p2)
        {
            return p1.Equals(p2);
        }

        public static bool operator !=(PagePosition p1, PagePosition p2)
        {
            return !(p1 == p2);
        }
    }
}

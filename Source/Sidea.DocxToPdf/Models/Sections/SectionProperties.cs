using System.Collections.Generic;
using System.Linq;
using Sidea.DocxToPdf.Core;

namespace Sidea.DocxToPdf.Models.Sections
{
    internal class SectionProperties
    {
        public static readonly SectionProperties Empty = new SectionProperties(
            PageConfiguration.Empty,
            HeaderFooterConfiguration.Empty,
            PageMargin.None,
            new SectionColumnConfig[0],
            false);

        public SectionProperties(
            PageConfiguration pageConfiguration,
            HeaderFooterConfiguration headerFooterConfiguration,
            PageMargin margin,
            IEnumerable<SectionColumnConfig> columnConfigs,
            bool requiresNewPage)
        {
            this.PageConfiguration = pageConfiguration;
            this.HeaderFooterConfiguration = headerFooterConfiguration;
            this.Margin = margin;
            this.RequiresNewPage = requiresNewPage;
            this.ColumnConfigs = columnConfigs.ToArray();
        }

        public PageConfiguration PageConfiguration { get; }
        public HeaderFooterConfiguration HeaderFooterConfiguration { get; }
        public PageMargin Margin { get; }
        public bool RequiresNewPage { get; }
        public bool HasTitlePage { get; }
        public IReadOnlyCollection<SectionColumnConfig> ColumnConfigs { get; }

        public double ColumnOffset(int columnIndex)
        {
            var result = this.ColumnConfigs
                .Take(columnIndex)
                .Aggregate(0.0, (acc, column) =>
                {
                    return acc + column.Width + column.Space;
                });

            return result;
        }

        public double ColumnWidth(int columnIndex)
        {
            return this.ColumnConfigs.ElementAt(columnIndex).Width;
        }
    }
}

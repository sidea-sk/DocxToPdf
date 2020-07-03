using Sidea.DocxToPdf.Core;

namespace Sidea.DocxToPdf.Models.Sections
{
    internal class SectionProperties
    {
        public static readonly SectionProperties Empty = new SectionProperties(
            PageConfiguration.Empty,
            HeaderFooterConfiguration.Empty,
            PageMargin.PageNone,
            false);

        public SectionProperties(
            PageConfiguration pageConfiguration,
            HeaderFooterConfiguration headerFooterConfiguration,
            PageMargin margin,
            bool requiresNewPage)
        {
            this.PageConfiguration = pageConfiguration;
            this.HeaderFooterConfiguration = headerFooterConfiguration;
            this.Margin = margin;
            this.RequiresNewPage = requiresNewPage;
        }

        public PageConfiguration PageConfiguration { get; }
        public HeaderFooterConfiguration HeaderFooterConfiguration { get; }
        public PageMargin Margin { get; }
        public bool RequiresNewPage { get; }
        public bool HasTitlePage { get; }
    }
}

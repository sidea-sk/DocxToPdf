using System.Collections.Generic;
using System.Linq;
using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Styles;

using OpenXml = DocumentFormat.OpenXml;
using Pack = DocumentFormat.OpenXml.Packaging;
using Word = DocumentFormat.OpenXml.Wordprocessing;

namespace Sidea.DocxToPdf.Models.Sections.Builders
{
    internal static class SectionBuilder
    {
        public static IEnumerable<Section> SplitToSections(
            this Pack.MainDocumentPart mainDocument,
            IStyleFactory styleFactory)
        {
            var useEvenOdd = mainDocument.DocumentSettingsPart.EvenOddHeadersAndFooters();

            var secProp = mainDocument.Document.Body
                   .ChildsOfType<Word.SectionProperties>()
                   .Single()
                   .ToModel(mainDocument, true, HeaderFooterConfiguration.Empty);

            var sectionElements = mainDocument.Document.Body.RenderableChildren();

            var sections = mainDocument.Document.Body
                .Select(data => new Section(sectionElements, secProp, styleFactory))
                .ToArray();

            return sections.Take(1);
        }

        //private static IEnumerable<Data> GetSectionData(
        //    this Word.Body body,
        //    Pack.MainDocumentPart mainDocumentPart,
        //    IStyleFactory styleFactory)
        //{
        //    var sectionData = new List<Data>();

        //    var sectionElements = new List<OpenXml.OpenXmlCompositeElement>();
        //    var headerFooterConfiguration = HeaderFooterConfiguration.Empty;
        //    foreach (var e in body.RenderableChildren())
        //    {
        //        sectionElements.Add(e);
        //        if (!(e is Word.Paragraph paragraph))
        //        {
        //            continue;
        //        }

        //        var sp = paragraph.GetSectionProperties();
        //        if (sp == null)
        //        {
        //            continue;
        //        }

        //        var sectionParts = sectionElements.SplitToSectionParts(styleFactory);
        //        var sd = new Data(sp.ToModel(mainDocumentPart, sectionData.Count == 0, headerFooterConfiguration), sectionParts);

        //        headerFooterConfiguration = sd.Properties.HeaderFooterConfiguration;
        //        sectionData.Add(sd);

        //        sectionElements.Clear();
        //    }

        //    var lastSectionProperties = body
        //       .ChildsOfType<Word.SectionProperties>()
        //       .Single();

        //    var lastSectionParts = sectionElements.SplitToSectionParts(styleFactory);

        //    sectionData.Add(new Data(lastSectionProperties.ToModel(mainDocumentPart, sectionData.Count == 0, headerFooterConfiguration), lastSectionParts));
        //    return sectionData;
        //}

        //private static IEnumerable<SectionColumn> SplitToSectionParts(
        //    this IEnumerable<OpenXml.OpenXmlCompositeElement> xmlElements,
        //    IStyleFactory styleFactory)
        //{
        //    var sectionParts = new List<SectionColumn>();

        //    var stack = xmlElements.ToStack();
        //    var partElements = new List<OpenXml.OpenXmlCompositeElement>();

        //    while (stack.Count > 0)
        //    {
        //        var e = stack.Pop();
        //        switch (e)
        //        {
        //            case Word.Paragraph p:
        //                {
        //                    var (begin, @break, end) = p.SplitByNextBreak();
        //                    if (@break == SectionBreak.None)
        //                    {
        //                        partElements.Add(p);
        //                    }
        //                    else
        //                    {
        //                        if (end != null)
        //                        {
        //                            stack.Push(end);
        //                        }

        //                        partElements.Add(begin);
        //                        sectionParts.Add(new SectionColumn(@break, partElements.ToArray(), styleFactory));
        //                        partElements.Clear();
        //                    }
        //                }
        //                break;
        //            default:
        //                partElements.Add(e);
        //                break;
        //        }
        //    }

        //    if (partElements.Count > 0)
        //    {
        //        sectionParts.Add(new SectionColumn(SectionBreak.None, partElements, styleFactory));
        //    }

        //    return sectionParts;
        //}

        private static Word.SectionProperties GetSectionProperties(this Word.Paragraph paragraph)
        {
            return paragraph.ParagraphProperties?.SectionProperties;
        }

        private static SectionProperties ToModel(
            this Word.SectionProperties wordSectionProperties,
            Pack.MainDocumentPart mainDocument,
            bool isFirstSection,
            HeaderFooterConfiguration inheritHeaderFooterConfiguration)
        {
            var pageCongifuration = wordSectionProperties.GetPageConfiguration();
            var pageMargin = wordSectionProperties.GetPageMargin();

            var sectionMark = wordSectionProperties.ChildsOfType<Word.SectionType>().SingleOrDefault()?.Val ?? Word.SectionMarkValues.NextPage;

            var requiresNewPage = isFirstSection || sectionMark == Word.SectionMarkValues.NextPage;

            var columns = wordSectionProperties.GetSectionColumns(pageCongifuration, pageMargin);
            var headerFooterConfiguration = wordSectionProperties
                .GetHeaderFooterConfiguration(mainDocument, inheritHeaderFooterConfiguration);


            return new SectionProperties(
                pageCongifuration,
                headerFooterConfiguration,
                pageMargin,
                columns,
                requiresNewPage);
        }

        private static PageConfiguration GetPageConfiguration(
            this Word.SectionProperties sectionProperties)
        {
            var pageSize = sectionProperties.ChildsOfType<Word.PageSize>().Single();
            var w = pageSize.Width.DxaToPoint();
            var h = pageSize.Height.DxaToPoint();


            var orientation = (pageSize.Orient?.Value ?? Word.PageOrientationValues.Portrait) == Word.PageOrientationValues.Portrait
                ? PageOrientation.Portrait
                : PageOrientation.Landscape;

            

            return new PageConfiguration(new Size(w, h), orientation);
        }

        private static PageMargin GetPageMargin(this Word.SectionProperties sectionProperties)
        {
            var pageMargin = sectionProperties.ChildsOfType<Word.PageMargin>().Single();
            return new PageMargin(
                pageMargin.Top.DxaToPoint(),
                pageMargin.Right.DxaToPoint(),
                pageMargin.Bottom.DxaToPoint(),
                pageMargin.Left.DxaToPoint(),
                pageMargin.Header.DxaToPoint(),
                pageMargin.Footer.DxaToPoint());
        }

        private static HeaderFooterConfiguration GetHeaderFooterConfiguration(
            this Word.SectionProperties wordSectionProperties,
            Pack.MainDocumentPart mainDocument,
            HeaderFooterConfiguration previousHeaderFooterConfiguration)
        {
            var hasTitlePage = wordSectionProperties.ChildsOfType<Word.TitlePage>().SingleOrDefault()
                  .IsOn(ifOnOffTypeNull: false, ifOnOffValueNull: true);

            var headerRefs = wordSectionProperties
                .ChildsOfType<Word.HeaderReference>()
                .Select(fr => new HeaderFooterRef(fr.Id, fr.Type));

            var footerRefs = wordSectionProperties
                .ChildsOfType<Word.FooterReference>()
                .Select(fr => new HeaderFooterRef(fr.Id, fr.Type));

            return previousHeaderFooterConfiguration.Inherited(mainDocument, hasTitlePage, headerRefs, footerRefs);
        }

        private static IEnumerable<SectionColumnConfig> GetSectionColumns(
            this Word.SectionProperties wordSectionProperties,
            PageConfiguration page,
            PageMargin pageMargin)
        {
            var columns = wordSectionProperties
                .ChildsOfType<Word.Columns>()
                .SingleOrDefault();

            var totalColumnsWidth = page.Width - pageMargin.HorizontalMargins;
            var columnsCount = columns.ColumnCount?.Value ?? 1;
            if (columnsCount == 1)
            {
                return new[] { new SectionColumnConfig(totalColumnsWidth, 0) };
            }

            if (columns.EqualWidth.IsOn(true))
            {
                var space = columns.Space.ToPoint();
                var columnWidth = (totalColumnsWidth - space * (columnsCount - 1)) / columnsCount;

                return Enumerable.Range(0, columnsCount)
                    .Select(i =>
                    {
                        var s = i == columnsCount - 1
                            ? 0
                            : space;
                        return new SectionColumnConfig(columnWidth, s);
                    })
                    .ToArray();
            }

            var cols = columns
                .ChildsOfType<Word.Column>()
                .Select(col =>
                {
                    var cw = col.Width.ToPoint();
                    var space = col.Space.ToPoint();
                    return new SectionColumnConfig(cw, space);
                });
            return cols;
        }

        private static (Word.Paragraph, SectionBreak, Word.Paragraph) SplitByNextBreak(this Word.Paragraph paragraph)
        {
            var index = paragraph
                .ChildElements
                .IndexOf(e => e is Word.Run r && r.ChildsOfType<Word.Break>().Any(b => b.Type != null && (b.Type == Word.BreakValues.Page || b.Type == Word.BreakValues.Column)));

            if (index == -1)
            {
                return (paragraph, SectionBreak.None, null);
            }

            var beginElements = paragraph
                .ChildElements
                .Take(index + 1)
                .Select(r => r.CloneNode(true))
                .ToArray();

            var begin = new Word.Paragraph(beginElements)
            {
                ParagraphProperties = (Word.ParagraphProperties)paragraph.ParagraphProperties.CloneNode(true)
            };

            var endElements = paragraph
                .ChildElements
                .Skip(index + 1)
                .Select(r => r.CloneNode(true))
                .ToArray();

            var end = endElements.Length == 0
                ? null
                : new Word.Paragraph(endElements)
                {
                    ParagraphProperties = (Word.ParagraphProperties)paragraph.ParagraphProperties.CloneNode(true)
                };

            var breakRun = (Word.Run)paragraph
                .ChildElements
                .ElementAt(index);

            var breakType = breakRun
                .ChildElements
                .OfType<Word.Break>()
                .Single()
                .Type;


            var @break = SectionBreak.None;
            switch (breakType.Value)
            {
                case Word.BreakValues.Column:
                    @break = SectionBreak.Column;
                    break;

                case Word.BreakValues.Page:
                    @break = SectionBreak.Page;
                    break;
                default:
                    throw new System.Exception("Unexpected value");
            };

            return (begin, @break, end);
        }

        private class Data
        {
            public Data(
                SectionProperties properties,
                IEnumerable<SectionColumn> sectionColumns,
                IEnumerable<OpenXml.OpenXmlCompositeElement> elements)
            {
                this.Properties = properties;
                this.SectionColumns = sectionColumns.ToArray();
                this.Elements = elements.ToArray();
            }

            public SectionProperties Properties { get; }
            public IReadOnlyCollection<SectionColumn> SectionColumns { get; }
            public IReadOnlyCollection<OpenXml.OpenXmlCompositeElement> Elements { get; }
        }
    }
}

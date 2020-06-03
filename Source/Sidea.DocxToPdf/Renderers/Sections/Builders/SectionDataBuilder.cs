using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml;
using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Common;
using Sidea.DocxToPdf.Renderers.Sections.Models;
using Sidea.DocxToPdf.Renderers.Styles;
using Word = DocumentFormat.OpenXml.Wordprocessing;

namespace Sidea.DocxToPdf.Renderers.Sections.Builders
{
    internal static class SectionDataBuilder
    {
        public static IEnumerable<SectionData> SplitToSections(
            this Word.Body body,
            bool useEvenOddFootersAndHeaders,
            IStyleAccessor styleAccessor)
        {
            var sectionData = new List<SectionData>();

            var sectionElements = new List<OpenXmlCompositeElement>();
            foreach(var e in body.RenderableChildren())
            {
                sectionElements.Add(e);
                if(!(e is Word.Paragraph paragraph))
                {
                    continue;
                }

                var sp = paragraph.GetSectionProperties();
                if (sp == null)
                {
                    continue;
                }

                var sectionParts = sectionElements.SplitToSectionParts(styleAccessor);
                var sd = new SectionData(sp.ToModel(sectionData.Count == 0, useEvenOddFootersAndHeaders), sectionParts);
                sectionData.Add(sd);
                sectionElements.Clear();
            }

            var lastSectionProperties = body
               .ChildsOfType<Word.SectionProperties>()
               .Single();

            var lastSectionParts = sectionElements.SplitToSectionParts(styleAccessor);
            sectionData.Add(new SectionData(lastSectionProperties.ToModel(sectionData.Count == 0, useEvenOddFootersAndHeaders), lastSectionParts));
            return sectionData;
        }

        private static IEnumerable<SectionPart> SplitToSectionParts(
            this IEnumerable<OpenXmlCompositeElement> xmlElements,
            IStyleAccessor styleAccessor)
        {
            var sectionParts = new List<SectionPart>();

            var stack = xmlElements.ToStack();
            var partElements = new List<OpenXmlCompositeElement>();

            while(stack.Count > 0)
            {
                var e = stack.Pop();
                switch (e)
                {
                    case Word.Paragraph p:
                        {
                            var (begin, @break, end) = p.SplitByNextBreak();
                            if(@break == SectionBreak.None)
                            {
                                partElements.Add(p);
                            }
                            else
                            {
                                if (end != null)
                                {
                                    stack.Push(end);
                                }

                                partElements.Add(begin);
                                sectionParts.Add(new SectionPart(@break, partElements.ToArray(), styleAccessor));
                                partElements.Clear();
                            }
                        }
                        break;
                    default:
                        partElements.Add(e);
                        break;
                }
            }

            if(partElements.Count > 0)
            {
                sectionParts.Add(new SectionPart(SectionBreak.None, partElements, styleAccessor));
            }

            return sectionParts;
        }

        private static Word.SectionProperties GetSectionProperties(this Word.Paragraph paragraph)
        {
            return paragraph.ParagraphProperties?.SectionProperties;
        }

        private static SectionProperties ToModel(this Word.SectionProperties wordSectionProperties, bool isFirstSection, bool useEvenOddFootersAndHeaders)
        {
            var pageCongifuration = wordSectionProperties.GetPageConfiguration();
            var sectionMark = wordSectionProperties.ChildsOfType<Word.SectionType>().SingleOrDefault()?.Val ?? Word.SectionMarkValues.NextPage;
            var renderBehaviour = isFirstSection || sectionMark == Word.SectionMarkValues.NextPage
                ? RenderBehaviour.NewPage
                : RenderBehaviour.Continue;

            var columns = wordSectionProperties.GetSectionColumns(pageCongifuration);
            var headerFooterConfiguration = wordSectionProperties.GetHeaderFooterConfiguration(useEvenOddFootersAndHeaders);

            return new SectionProperties(
                pageCongifuration,
                headerFooterConfiguration,
                columns,
                renderBehaviour);
        }

        private static PageConfiguration GetPageConfiguration(this Word.SectionProperties sectionProperties)
        {
            var pageSize = sectionProperties.ChildsOfType<Word.PageSize>().Single();
            var w = pageSize.Width.DxaToPoint();
            var h = pageSize.Height.DxaToPoint();

            var orientation = (pageSize.Orient?.Value ?? Word.PageOrientationValues.Portrait) == Word.PageOrientationValues.Portrait
                ? PdfSharp.PageOrientation.Portrait
                : PdfSharp.PageOrientation.Landscape;

            var margin = sectionProperties.GetPageMargin();

            return new PageConfiguration(new XSize(w, h), margin, orientation);
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

        private static  HeaderFooterConfiguration GetHeaderFooterConfiguration(
            this Word.SectionProperties wordSectionProperties,
            bool useEvenOddFootersAndHeaders)
        {
            var hasTitlePage = wordSectionProperties.ChildsOfType<Word.TitlePage>().SingleOrDefault()
                  .IsOn(ifOnOffTypeNull: false, ifOnOffValueNull: true);

            var headerRefs = wordSectionProperties
                .ChildsOfType<Word.HeaderReference>()
                .Select(fr => new HeaderFooterRef(fr.Id, fr.Type));

            var footerRefs = wordSectionProperties
                .ChildsOfType<Word.FooterReference>()
                .Select(fr => new HeaderFooterRef(fr.Id, fr.Type));

            return new HeaderFooterConfiguration(hasTitlePage, useEvenOddFootersAndHeaders, headerRefs, footerRefs);
        }

        private static IEnumerable<SectionColumn> GetSectionColumns(this Word.SectionProperties wordSectionProperties, PageConfiguration page)
        {
            var columns = wordSectionProperties
                .ChildsOfType<Word.Columns>()
                .SingleOrDefault();

            var totalColumnsWidth = page.Width - page.Margin.HorizontalMargins;
            var columnsCount = columns.ColumnCount?.Value ?? 1;
            if (columnsCount == 1)
            {
                return new[] { new SectionColumn(totalColumnsWidth, new XUnit(0)) };
            }

            if(columns.EqualWidth.IsOn(true))
            {
                var space = columns.Space.ToXUnit();
                var columnWidth = (totalColumnsWidth - space * (columnsCount - 1)) / columnsCount;

                return Enumerable.Range(0, columnsCount)
                    .Select(i =>
                    {
                        var s = i == columnsCount - 1
                            ? XUnit.Zero
                            : space;
                        return new SectionColumn(columnWidth, s);
                    })
                    .ToArray();
            }

            var cols = columns
                .ChildsOfType<Word.Column>()
                .Select(col =>
                {
                    var cw = col.Width.ToXUnit();
                    var space = col.Space.ToXUnit();
                    return new SectionColumn(cw, space);
                });
            return cols;
        }

        private static (Word.Paragraph, SectionBreak, Word.Paragraph) SplitByNextBreak(this Word.Paragraph paragraph)
        {
            var index = paragraph
                .ChildElements
                .IndexOf(e => e is Word.Run r && r.ChildsOfType<Word.Break>().Any(b => b.Type == Word.BreakValues.Page || b.Type == Word.BreakValues.Column));

            if(index == -1)
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
    }
}

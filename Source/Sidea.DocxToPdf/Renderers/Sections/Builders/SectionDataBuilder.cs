using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml;
using Sidea.DocxToPdf.Renderers.Common;
using Sidea.DocxToPdf.Renderers.Sections.Models;
using Word = DocumentFormat.OpenXml.Wordprocessing;

namespace Sidea.DocxToPdf.Renderers.Sections.Builders
{
    internal static class SectionDataBuilder
    {
        public static IEnumerable<SectionData> SplitToSections(this Word.Body body)
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

                var sd = new SectionData(sp.ToModel(sectionData.Count == 0), sectionElements);
                sectionData.Add(sd);
                sectionElements.Clear();
            }

            var lastSectionProperties = body
               .ChildsOfType<Word.SectionProperties>()
               .Single();

            sectionData.Add(new SectionData(lastSectionProperties.ToModel(sectionData.Count == 0), sectionElements));
            return sectionData;
        }

        private static Word.SectionProperties GetSectionProperties(this Word.Paragraph paragraph)
        {
            return paragraph.ParagraphProperties?.SectionProperties;
        }

        private static SectionProperties ToModel(this Word.SectionProperties wordSectionProperties, bool isFirstSection)
        {
            var pageMargin = wordSectionProperties.ChildsOfType<Word.PageMargin>().Single();

            var margin = new Margin(
                pageMargin.Top.ToXUnit(),
                pageMargin.Right.ToXUnit(),
                pageMargin.Bottom.ToXUnit(),
                pageMargin.Left.ToXUnit());

            var sectionMark = wordSectionProperties.ChildsOfType<Word.SectionType>().SingleOrDefault()?.Val ?? Word.SectionMarkValues.NextPage;
            var renderBehaviour = isFirstSection || sectionMark == Word.SectionMarkValues.NextPage
                ? RenderBehaviour.NewPage
                : RenderBehaviour.Continue;

            return new SectionProperties(margin, PdfSharp.PageOrientation.Portrait, renderBehaviour);
        }
    }
}

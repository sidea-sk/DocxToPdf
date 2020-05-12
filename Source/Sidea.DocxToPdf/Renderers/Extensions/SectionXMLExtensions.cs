using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Common;
using Word = DocumentFormat.OpenXml.Wordprocessing;

namespace Sidea.DocxToPdf.Renderers
{
    internal static class SectionXMLExtensions
    {
        public static PageMargin GetPageMargin(this MainDocumentPart mainDocumentPart)
        {
            var sectionProperties = mainDocumentPart.Document.Body
                .ChildsOfType<Word.SectionProperties>()
                .Single();

            return sectionProperties.GetPageMargin();
        }

        public static PageConfiguration GetPageConfiguration(this Word.SectionProperties sectionProperties)
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

        public static Word.SectionMarkValues GetSectionMark(this Word.SectionProperties sectionProperties)
        {
            var st = sectionProperties.ChildsOfType<Word.SectionType>().SingleOrDefault();
            return st?.Val ?? Word.SectionMarkValues.NextPage;
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
    }
}

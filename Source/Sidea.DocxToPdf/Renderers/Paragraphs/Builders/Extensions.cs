using DocumentFormat.OpenXml.Wordprocessing;
using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Paragraphs.Models;
using Sidea.DocxToPdf.Renderers.Styles;

namespace Sidea.DocxToPdf.Renderers.Paragraphs.Builders
{
    internal static class Extensions
    {
        public static XUnit CalculateSpaceAfterLine(this LineSpacing lineSpacing, RLine line)
            => lineSpacing.CalculateSpaceAfterLine(line.PrecalulatedSize.Height);
    }
}

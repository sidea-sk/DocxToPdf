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

        public static RStyle Style(this Run run, XFont defaultFont)
        {
            XFont font = run.RunProperties.CreateRunFont(defaultFont);
            XBrush brush = run.RunProperties?.Color.ToXBrush() ?? XBrushes.Black;

            return new RStyle(font, brush);
        }
    }
}

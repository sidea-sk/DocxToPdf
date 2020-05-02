using DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers
{
    internal static class DrawingXmlExtensions
    {
        public static XSize Size(this Extent extent)
        {
            var width = extent.Cx.EmuToXUnit();
            var height = extent.Cy.EmuToXUnit();
            return new XSize(width, height);
        }
    }
}

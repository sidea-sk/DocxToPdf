using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Core.Services
{
    internal interface ITextMeasuringService
    {
        XSize MeasureText(string text, XFont font);
    }
}

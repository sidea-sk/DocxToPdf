using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Core
{
    internal interface ITextMeasuringService
    {
        XSize MeasureText(string text, XFont font);
    }
}

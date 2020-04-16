using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Core.Serices
{
    internal interface ITextMeasuringService
    {
        XSize MeasureText(string text, XFont font);
    }
}

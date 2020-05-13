using DocumentFormat.OpenXml;
using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers
{
    internal static class Emu
    {
        private const double INCH = 72;
        private const double EMU = 914400;

        public static XUnit EmuToXUnit(this Int64Value value)
        {
            return value.Value.EmuToXUnit();
        }

        public static XUnit EmuToXUnit(this long value)
        {
            return value / EMU * INCH;
        }
    }
}

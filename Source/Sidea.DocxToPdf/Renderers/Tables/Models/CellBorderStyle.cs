using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Borders;

namespace Sidea.DocxToPdf.Renderers.Tables.Models
{
    internal class CellBorderStyle : BorderStyle
    {
        public CellBorderStyle(XPen top, XPen right, XPen bottom, XPen left) : base(top, right, bottom, left)
        {
        }
    }
}

using DocumentFormat.OpenXml.Wordprocessing;
using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Tables.Models
{
    internal class RGridRow
    {
        public RGridRow(XUnit height, HeightRuleValues heightRule)
        {
            this.Height = height;
            this.HeightRule = heightRule;
        }

        public XUnit Height { get; }
        public HeightRuleValues HeightRule { get; }
    }
}

using System.Collections.Generic;
using DocumentFormat.OpenXml.Wordprocessing;
using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Styles
{
    internal class TextStyle
    {
        public static readonly TextStyle Default = new TextStyle(new XFont("Calibri", 11), XBrushes.Black);

        public TextStyle(XFont font, XBrush brush)
        {
            this.Font = font;
            this.Brush = brush;
        }

        public XFont Font { get; }
        public XBrush Brush { get; }

        public TextStyle Override(RunProperties runProperties, IReadOnlyCollection<StyleRunProperties> styleRuns)
        {
            if (runProperties == null && styleRuns.Count == 0)
            {
                return this;
            }

            var fontSize = runProperties.EffectiveFontSize(styleRuns, this.Font.Size);
            var fontStyle = runProperties.EffectiveFontStyle(styleRuns, this.Font.Style);

            var font = new XFont("Arial", fontSize, fontStyle);
            var brush = runProperties.EffectiveColor(styleRuns, this.Brush);

            return new TextStyle(font, brush);
        }

        public static TextStyle From(RunPropertiesBaseStyle runPropertiesBaseStyle)
        {
            var fs = runPropertiesBaseStyle.FontSize.ToXUnit(XUnit.FromPoint(11));

            var font = new XFont("Arial", fs);
            var brush = runPropertiesBaseStyle.Color.ToXBrush();
            return new TextStyle(font, brush);
        }
    }
}

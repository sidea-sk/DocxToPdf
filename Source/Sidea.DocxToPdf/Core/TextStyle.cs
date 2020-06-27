using System.Drawing;

namespace Sidea.DocxToPdf.Core
{
    internal class TextStyle
    {
        private static readonly Graphics _graphics;
        private static StringFormat _stringFormat;

        static TextStyle()
        {
            var b = new Bitmap(1, 1);
            _graphics = Graphics.FromImage(b);
            _graphics.PageUnit = GraphicsUnit.Point;

            _stringFormat = StringFormat.GenericTypographic;
            _stringFormat.Alignment = StringAlignment.Center;
            _stringFormat.Trimming = StringTrimming.None;
            _stringFormat.FormatFlags = StringFormatFlags.MeasureTrailingSpaces;
        }

        public TextStyle(Font font, Color brush, Color background)
        {
            this.Font = font;
            this.Brush = brush;
            this.Background = background;
        }

        public Font Font { get; }
        public Color Brush { get; }
        public Color Background { get; }

        public double CellAscent
        {
            get
            {
                var ca = this.Font.SizeInPoints * (double)this.Font.FontFamily.GetCellAscent(this.Font.Style) / this.Font.FontFamily.GetEmHeight(this.Font.Style);
                return ca;
            }
        }

        public Size MeasureText(string text)
        {
            var sizeF = _graphics.MeasureString(text, this.Font, PointF.Empty, _stringFormat);
            return new Size(sizeF.Width, sizeF.Height);
        }

        public TextStyle WithChanged(Font font = null, Color? brush = null, Color? background = null)
        {
            return new TextStyle(
                font ?? this.Font,
                brush ?? this.Brush,
                background ?? this.Background);
        }
    }
}

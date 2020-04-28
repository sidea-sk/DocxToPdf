using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Common
{
    internal class Box<T>
    {
        public Box(
            T element,
            XPoint offset)
        {
            this.Element = element;
            this.Offset = offset;
        }

        public T Element { get; }
        public XPoint Offset { get; }
    }
}

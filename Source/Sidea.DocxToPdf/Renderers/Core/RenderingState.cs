using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Core
{
    internal class RenderingState
    {
        public RenderingState(RenderingStatus status, XPoint finishedAtPosition)
        {
            this.Status = status;
            this.FinishedAtPosition = finishedAtPosition;
        }

        public RenderingStatus Status { get; }

        public XPoint FinishedAtPosition { get; }
    }
}

using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;

namespace Sidea.DocxToPdf.Renderers.Core
{
    internal interface IRenderer
    {
        /// <summary>
        /// The precalculated size of the content.
        /// It does not contain page breaks, which may extend the real size.
        /// It may be considered as the minimum size to render.
        /// </summary>
        XSize PrecalulatedSize { get; }

        /// <summary>
        /// The aggregate of rendered size.
        /// It is extended in each Render round if the rendering takes place over multiple pages.
        /// </summary>
        XSize RenderedSize { get; }

        /// <summary>
        /// The result of last rendering.
        /// Its's used to signalize whether the rendering has reached the end of rendering area and needs more.
        /// </summary>
        RenderingState CurrentRenderingState { get; }

        void CalculateContentSize(IPrerenderArea prerenderArea);

        void Render(IRenderArea renderArea);
    }
}

using Sidea.DocxToPdf.Core;

namespace Sidea.DocxToPdf.Models
{
    internal abstract class ContainerElement
    {
        public abstract void Initialize();

        public abstract void Prepare(
            IPage page,
            Rectangle region,
            IPageManager pageManager);
    }
}

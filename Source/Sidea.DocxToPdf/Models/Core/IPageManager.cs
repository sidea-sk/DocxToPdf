using Sidea.DocxToPdf.Core;

namespace Sidea.DocxToPdf.Models
{
    internal interface IPageManager
    {
        void EnsurePage(PageNumber pageNumber);

        IPage GetPage(PageNumber pageNumber);
    }
}

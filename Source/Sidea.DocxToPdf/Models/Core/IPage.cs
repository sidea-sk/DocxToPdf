using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Common;

namespace Sidea.DocxToPdf.Models
{
    internal interface IPage
    {
        PageNumber PageNumber { get; }

        PageConfiguration Configuration { get; }

        Margin Margin { get; }

        Rectangle GetContentRegion();
    }
}

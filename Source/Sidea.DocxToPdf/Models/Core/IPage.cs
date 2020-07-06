using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Common;

namespace Sidea.DocxToPdf.Models
{
    internal interface IPage
    {
        PageNumber PageNumber { get; }

        DocumentVariables DocumentVariables { get; }

        PageConfiguration Configuration { get; }

        Margin Margin { get; }

        Rectangle GetPageRegion();

        Rectangle GetContentRegion();
    }
}

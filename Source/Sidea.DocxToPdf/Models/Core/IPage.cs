using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Common;

namespace Sidea.DocxToPdf.Models
{
    internal interface IPage
    {
        PageNumber PageNumber { get; }

        DocumentVariables DocumentVariables { get; }

        PageConfiguration Configuration { get; }

        PageMargin Margin { get; }

        Rectangle GetPageRegion();

        Rectangle GetContentRegion();

        void SetHorizontalMargins(double left, double right);
        void SetTopMargins(double header, double top);
        void SetBottomMargins(double footer, double bottom);
    }
}

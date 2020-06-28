using Sidea.DocxToPdf.Core;

namespace Sidea.DocxToPdf.Models
{
    internal interface IPage
    {
        PageNumber PageNumber { get; }

        PageConfiguration Configuration { get; }
        
        double TopMargin { get; }
        double BottomMargin { get; }
    }
}

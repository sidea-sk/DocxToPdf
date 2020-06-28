using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Sections;
using Sidea.DocxToPdf.Models.Sections.Builders;
using Sidea.DocxToPdf.Models.Styles;

namespace Sidea.DocxToPdf.Models
{
    internal class Document
    {
        private Section[] _sections = new Section[0];
        private readonly WordprocessingDocument _docx;
        private readonly IStyleFactory _styleAccessor;

        public Document(WordprocessingDocument docx)
        {
            _docx = docx;
            _styleAccessor = StyleFactory.Default(docx.MainDocumentPart);
        }

        public void Render(IRenderer renderer)
        {
            this.InitializeSections();
        }

        private void InitializeSections()
        {
            _sections = _docx.MainDocumentPart
                .SplitToSections(_styleAccessor)
                .ToArray();

            foreach(var section in _sections)
            {
                section.Initialize();
            }
        }
    }
}

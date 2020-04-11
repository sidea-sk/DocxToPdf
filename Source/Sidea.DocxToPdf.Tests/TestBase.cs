using System.Text;
using DocumentFormat.OpenXml.Packaging;

namespace Sidea.DocxToPdf.Tests
{
    public class TestBase
    {
        public TestBase()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        private const string RelativePath = "../../../..";

        protected void Generate(string docxSampleFileName)
        {
            var document = WordprocessingDocument.Open($"{RelativePath}/Samples/{docxSampleFileName}.docx", false);
            var pdfGenerator = new PdfGenerator();
            var pdf = pdfGenerator.Generate(document);

            pdf.Save($"{RelativePath}/TestOutputs/{docxSampleFileName}.pdf");
        }
    }
}

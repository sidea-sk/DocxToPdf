using System.IO;
using System.Text;
using DocumentFormat.OpenXml.Packaging;

namespace Sidea.DocxToPdf.Tests
{
    public abstract class TestBase
    {
        private readonly string _samplesFolder;
        private readonly string _outputFolder;

        protected TestBase(string samplesSubFolder)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            _samplesFolder = $"../../../../Samples/{samplesSubFolder}";
            _outputFolder = $"../../../../TestOutputs/{samplesSubFolder}";
        }

        protected void Generate(string docxSampleFileName)
        {
            var inputFileName = $"{_samplesFolder}/{docxSampleFileName}.docx";
            var document = WordprocessingDocument.Open(inputFileName, false);
            var pdfGenerator = new PdfGenerator();
            var pdf = pdfGenerator.Generate(document);

            if (!Directory.Exists(_outputFolder))
            {
                Directory.CreateDirectory(_outputFolder);
            }

            pdf.Save($"{_outputFolder}/{docxSampleFileName}.pdf");
        }
    }
}

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
            var options = new RenderingOptions(
                renderParagraphCharacter: true
                );

            if (!Directory.Exists(_outputFolder))
            {
                Directory.CreateDirectory(_outputFolder);
            }

            var outputFileName = $"{_outputFolder}/{docxSampleFileName}.pdf";
            if (File.Exists(outputFileName))
            {
                File.Delete(outputFileName);
            }

            var inputFileName = $"{_samplesFolder}/{docxSampleFileName}.docx";
            using var fileStream = File.Open(inputFileName, FileMode.Open, FileAccess.Read);
            var pdfGenerator = new PdfGenerator();
            var pdf = pdfGenerator.Generate(fileStream, options);

            pdf.Save(outputFileName);
        }
    }
}

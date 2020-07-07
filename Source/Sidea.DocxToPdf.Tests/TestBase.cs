using System.IO;
using System.Text;

namespace Sidea.DocxToPdf.Tests
{
    public abstract class TestBase
    {
        private readonly string _samplesFolder;
        private readonly string _outputFolder;
        private readonly bool _useNextGeneration;

        protected RenderingOptions Options { get; set; } = new RenderingOptions(hiddenChars: true);

        protected TestBase(string samplesSubFolder, bool useNextGeneration = false)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            _samplesFolder = $"../../../../Samples/{samplesSubFolder}";
            _outputFolder = $"../../../../TestOutputs/{samplesSubFolder}";
            _useNextGeneration = useNextGeneration;
        }

        protected void Generate(string docxSampleFileName)
        {
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

            var pdf = _useNextGeneration
                ? this.GenerateV2(fileStream, this.Options)
                : this.GenerateV1(fileStream, this.Options);
            pdf.Save(outputFileName);
        }

        private PdfSharp.Pdf.PdfDocument GenerateV1(Stream docxStream, RenderingOptions options)
        {
            var pdfGenerator = new PdfGenerator();
            var pdf = pdfGenerator.Generate(docxStream, options);
            return pdf;
        }

        private PdfSharp.Pdf.PdfDocument GenerateV2(Stream docxStream, RenderingOptions options)
        {
            var pdfGenerator = new PdfGeneratorGen2();
            var pdf = pdfGenerator.Generate(docxStream, options);
            return pdf;
        }
    }
}

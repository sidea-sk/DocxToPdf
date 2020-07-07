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
            using var docxStream = File.Open(inputFileName, FileMode.Open, FileAccess.Read);

            var pdfGenerator = new PdfGenerator();
            var pdf = pdfGenerator.Generate(docxStream, this.Options);
            pdf.Save(outputFileName);
        }
    }
}

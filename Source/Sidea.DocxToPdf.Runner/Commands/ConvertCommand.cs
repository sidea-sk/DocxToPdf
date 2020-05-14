using CommandLine;

namespace Sidea.DocxToPdf.Runner.Commands
{
    [Verb("c", HelpText = "Convert docx document to pdf")]
    class ConvertCommand
    {
        [Option('d', "doc", Required = true, HelpText = "Docx input Filepath")]
        public string DocxPath { get; set; }

        [Option('p', "pdf", Required = true, HelpText = "Pdf output Filepath")]
        public string PdfOutputPath { get; set; }

        [Option('o', "continue", Default = false, HelpText = "Continue working (Do not close console)")]
        public bool Continue { get; set; }
    }
}

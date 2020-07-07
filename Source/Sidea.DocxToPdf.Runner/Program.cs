using System;
using System.IO;
using System.Linq;
using System.Text;
using CommandLine;
using Sidea.DocxToPdf.Runner.Commands;

namespace Sidea.DocxToPdf.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            bool close = false;
            string[] verb = args;

            do
            {
                close = Parser.Default
                    .ParseArguments<ConvertCommand, QuitCommand>(verb)
                    .MapResult(
                        (ConvertCommand cc) => Convert(cc),
                        (QuitCommand qc) => true,
                        err => false);

                if (!close)
                {
                    Console.Write("$  ");
                    verb = Console.ReadLine().Split(" ").ToArray();
                }
            } while (!close);
        }

        private static bool Convert(ConvertCommand command)
        {
            try
            {
                ExecuteConvert(command.DocxPath, command.PdfOutputPath);

                Console.WriteLine("Done...");
            }
            catch(Exception ex)
            {
                Console.WriteLine("Convert failed");
                Console.WriteLine(ex);
            }

            return !command.Continue;
        }

        private static void ExecuteConvert(string docxFilePath, string pdfOutputFilePath)
        {
            using var docxStream = File.Open(docxFilePath, FileMode.Open, FileAccess.Read);
            var pdfGenerator = new PdfGeneratorGen2();
            var pdf = pdfGenerator.GenerateAsByteArray(docxStream);
            File.WriteAllBytes(pdfOutputFilePath, pdf);
        }
    }
}

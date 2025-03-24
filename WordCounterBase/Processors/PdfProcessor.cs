using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf;
using PdfiumViewer;
using System.Drawing;
using WordCounterBase.Models;


namespace WordCounterBase.Processors
{
    internal static class PdfProcessor
    {
        internal static PdfProcessingResult ProcessFile(string pdfFilePath)
        {
            using iText.Kernel.Pdf.PdfDocument pdfDocument = new iText.Kernel.Pdf.PdfDocument(new PdfReader(pdfFilePath));
            int totalPages = pdfDocument.GetNumberOfPages();

            string pdfText = ExtractTextFromPdf(pdfDocument);
            int wordCount = WordProcessor.CountWords(pdfText);
            return new PdfProcessingResult() { PageCount = totalPages, WordCount = wordCount };
        }


        private static string ExtractTextFromPdf(iText.Kernel.Pdf.PdfDocument pdfDocument)
        {
            StringWriter textWriter = new StringWriter();
            for (int page = 1; page <= pdfDocument.GetNumberOfPages(); page++)
            {
                var strategy = new SimpleTextExtractionStrategy();
                var parser = new PdfCanvasProcessor(strategy);
                parser.ProcessPageContent(pdfDocument.GetPage(page));

                textWriter.WriteLine(strategy.GetResultantText());
            }
            return textWriter.ToString();
        }


        private static string CreateTempDirectory()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }

        public static string Pdf2Image(string pdfFilePath)
        {
            string tempDirectory = CreateTempDirectory();

            using (PdfiumViewer.PdfDocument pdfDocument = PdfiumViewer.PdfDocument.Load(pdfFilePath))
            {
                int dpi = 300;

                for (int pageIndex = 0; pageIndex < pdfDocument.PageCount; pageIndex++)
                {
                    SizeF sizeInPoints = pdfDocument.PageSizes[pageIndex];
                    int widthInPixels = (int)Math.Round(sizeInPoints.Width * (float)dpi / 72F);
                    int heightInPixels = (int)Math.Round(sizeInPoints.Height * (float)dpi / 72F);

                    using (Image image = pdfDocument.Render(pageIndex, widthInPixels, heightInPixels, dpi, dpi, true))
                    {
                        // Save the image to the temporary directory
                        string outputPath = Path.Combine(tempDirectory, $"Page_{pageIndex + 1}.png");
                        image.Save(outputPath);
                    }
                }
            }
            return tempDirectory;
        }
    }
}

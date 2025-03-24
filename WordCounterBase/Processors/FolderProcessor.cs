using iText.Kernel.Pdf.Annot.DA;
using WordCounterBase.Models;

namespace WordCounterBase.Processors
{
    public static class FolderProcessor
    {
        private static readonly string[] ext = ["png", "bmp"];
        public static List<WordCountResult> ProcessFolder(string folderPath)
        {
            var result = new List<WordCountResult>();

            var filePaths = Directory.EnumerateFiles(folderPath, "*.*");
            var imageFiles = filePaths.Where(file => ext.Any(x => file.EndsWith(x, StringComparison.OrdinalIgnoreCase))).ToList();
            var pdfFiles = filePaths.Where(file => file.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase));

            //Parallel.ForEach(imageFiles, file =>
            //{
            //    var wordQty = ImageProcessor.GetWordCount(file);
            //    result.Add(
            //        new WordCountResult()
            //        {
            //            FileName = file,
            //            WordCount = wordQty
            //        }
            //    );
            //});

            //Parallel.ForEach(pdfFiles, file =>
            //{
            //    var wordQty = PdfProcessor.GetWordCount(file);
            //    result.Add(
            //        new WordCountResult()
            //        {
            //            FileName = file,
            //            WordCount = wordQty
            //        }
            //    );
            //});

            foreach (var filePath in filePaths)
            {
                if(filePath.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                {
                    var scannedDocument = false;
                    var pdfProcessingResult = new PdfProcessingResult();
                    pdfProcessingResult = PdfProcessor.ProcessFile(filePath);

                    if (pdfProcessingResult.WordCount == 0)
                    {
                        var tempPathToImages = PdfProcessor.Pdf2Image(filePath);
                        pdfProcessingResult.WordCount = ProcessImagesFromPDF(tempPathToImages);
                        if (pdfProcessingResult.WordCount > 0) scannedDocument = true;
                    }

                    result.Add(
                    new WordCountResult()
                    {
                        FileName = filePath,
                        WordCount = pdfProcessingResult?.WordCount ?? 0,
                        ScannedDocument = scannedDocument,
                        PageCount = pdfProcessingResult?.PageCount ?? 0
                    }
                );
                }


                if (filePath.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                {

                    var pdfProcessingResult = JsonProcessor.ProcessFile(filePath);

                    result.Add(
                    new WordCountResult()
                    {
                        FileName = filePath,
                        WordCount = pdfProcessingResult?.WordCount ?? 0,
                        ScannedDocument = false,
                        PageCount = 0
                    });
                }
            }



            var subfolders = Directory.EnumerateDirectories(folderPath);
            foreach (string subfolder in subfolders)
            {
                ProcessFolder(subfolder);
            }

            return result;
        }

        private static int ProcessImagesFromPDF(string folderPath)
        {
            var files = Directory.EnumerateFiles(folderPath, "*.png");
            var wordQty = default(int); 

            foreach (var file in files)
            {
                if (ext.Any(x => file.EndsWith(x, StringComparison.OrdinalIgnoreCase)))
                {
                    wordQty += ImageProcessor.GetWordCount(file);
                }
            }

            return wordQty;
        }


        //private async Task ProcessFolder(string folderPath)
        //{
        //    string[] pdfFiles = Directory.GetFiles(folderPath, "*.pdf");
        //    string folderName = new DirectoryInfo(folderPath).Name;
        //    Console.WriteLine($"{folderName}:");
        //    foreach (string pdfFile in pdfFiles)
        //    {
        //        int wordCount = GetWordCountFromPdf(pdfFile);

        //        string fileName = Path.GetFileName(pdfFile);

        //        Console.WriteLine($"{wordCount}, {fileName} ");

        //        wordCountResults.Add(new WordCountResult
        //        {
        //            FileName = fileName,
        //            WordCount = wordCount
        //        });
        //        await InvokeAsync(() => StateHasChanged());
        //        totalWordsAllFiles += wordCount;
        //    }

        //    int totalFiles = pdfFiles.Length;
        //    //Console.WriteLine($"Total PDF files in {folderName}: {totalFiles}");

        //    string[] subfolders = Directory.GetDirectories(folderPath);
        //    foreach (string subfolder in subfolders)
        //    {
        //        await ProcessFolder(subfolder);
        //    }
        //}
    }
}

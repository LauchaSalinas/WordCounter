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

            var files = Directory.EnumerateFiles(folderPath, "*.*");
            var imageFiles = files.Where(file => ext.Any(x => file.EndsWith(x, StringComparison.OrdinalIgnoreCase))).ToList();
            var pdfFiles = files.Where(file => file.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase));

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

            foreach (var file in files)
            {
                var scannedDocument = false;
                var wordQty = default(int);
                if(file.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                {
                    wordQty = PdfProcessor.GetWordCount(file);

                    if (wordQty == 0)
                    {
                        var tempPathToImages = PdfProcessor.Pdf2Image(file);
                        wordQty = ProcessImagesFromPDF(tempPathToImages);
                        if (wordQty > 0) scannedDocument = true;
                    }
                }

                result.Add(
                    new WordCountResult()
                    {
                        FileName = file,
                        WordCount = wordQty,
                        ScannedDocument = scannedDocument
                    }
                );
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

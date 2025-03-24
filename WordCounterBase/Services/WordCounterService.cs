using System.Diagnostics;
using WordCounterBase.Models;
using WordCounterBase.Processors;

namespace WordCounterBase.Services
{
    public static class WordCounterService
    {
        private static readonly string[] _imgExtensions = ["png", "bmp"];
        public static ProcessFolderResult ProcessFolder(string folderPath)
        {
            var result = new ProcessFolderResult();
            var stpwtch = new Stopwatch();
            stpwtch.Start();

            var filePaths = Directory.EnumerateFiles(folderPath, "*.*", SearchOption.AllDirectories);
            var imageFiles = filePaths.Where(file => _imgExtensions.Any(x => file.EndsWith(x, StringComparison.OrdinalIgnoreCase))).ToList();
            var pdfFiles = filePaths.Where(file => file.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase));

            foreach (var filePath in filePaths)
            {
                if (filePath.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                {
                    var scannedDocument = false;
                    var pdfProcessingResult = new PdfProcessingResult();
                    pdfProcessingResult = PdfProcessor.ProcessFile(filePath);

                    if (pdfProcessingResult.WordCount == 0) // id is zerp prob are all images of scanned pdf
                    {
                        var tempPathToImages = PdfProcessor.Pdf2Image(filePath);
                        pdfProcessingResult.WordCount = ProcessImagesFromPDF(tempPathToImages);
                        if (pdfProcessingResult.WordCount > 0) scannedDocument = true;
                    }

                    result.Files.Add(
                        new ProcessFileResult()
                        {
                            FileName = filePath,
                            WordCount = pdfProcessingResult?.WordCount ?? 0,
                            ScannedDocument = scannedDocument,
                            PageCount = pdfProcessingResult?.PageCount ?? 0
                        }
                    );
                }

                imageFiles.ForEach(img =>
                {
                    var imagefProcessingResult = ImageProcessor.GetWordCount(img);
                    result.Files.Add(
                        new ProcessFileResult()
                        {
                            FileName = filePath,
                            WordCount = imagefProcessingResult.WordCount,
                            ScannedDocument = true,
                            PageCount = 0
                        }
                    );
                });

                if (filePath.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                {

                    var jsonProcessingResult = JsonProcessor.ProcessFile(filePath);

                    result.Files.Add(
                        new ProcessFileResult()
                        {
                            FileName = filePath,
                            WordCount = jsonProcessingResult?.WordCount ?? 0,
                            ScannedDocument = false,
                            PageCount = 0
                        }
                    );
                }
            }

            //var subfolders = Directory.EnumerateDirectories(folderPath);
            //foreach (string subfolder in subfolders)
            //{
            //    ProcessFolder(subfolder);
            //}
            stpwtch.Stop();
            result.ElapsedTime = stpwtch.Elapsed;
            return result;
        }

        private static int ProcessImagesFromPDF(string folderPath)
        {
            var files = Directory.EnumerateFiles(folderPath, "*.png");
            var wordQty = default(int);

            foreach (var file in files)
            {
                if (_imgExtensions.Any(x => file.EndsWith(x, StringComparison.OrdinalIgnoreCase)))
                {
                    wordQty += ImageProcessor.GetWordCount(file).WordCount;
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

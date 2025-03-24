using System.Reflection;
using Tesseract;
using WordCounterBase.Models;


namespace WordCounterBase.Processors
{
    public static class ImageProcessor
    {

        public static ImageProcessingResult GetWordCount(string path)
        {
            string tesseractDataPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly()?.Location);

            var ocrengine = new TesseractEngine(tesseractDataPath, "eng", EngineMode.Default);
            var img = Pix.LoadFromFile(path);
            var res = ocrengine.Process(img);
            var text = res.GetText();
            return new ImageProcessingResult()
            {
                WordCount = WordCounter.CountWords(text)
            };
        }
    }
}

using System.Reflection;
using Tesseract;


namespace WordCounterBase.Processors
{
    public static class ImageProcessor
    {

        public static int GetWordCount(string path)
        {
            string tesseractDataPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly()?.Location);

            var ocrengine = new TesseractEngine(tesseractDataPath, "eng", EngineMode.Default);
            var img = Pix.LoadFromFile(path);
            var res = ocrengine.Process(img);
            var text = res.GetText();
            return WordProcessor.CountWords(text);
        }
    }
}

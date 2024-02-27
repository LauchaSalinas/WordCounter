namespace WordCounterBase.Models
{
    public class WordCountResult
    {
        public string FileName { get; set; }
        public int WordCount { get; set; }

        public bool ScannedDocument { get; set; }
    }
}

namespace WordCounterBase.Models
{
    public class ProcessFileResult
    {
        public required string FileName { get; set; }
        public int WordCount { get; set; }

        public bool ScannedDocument { get; set; }
        public required int PageCount { get; set; }
    }
}

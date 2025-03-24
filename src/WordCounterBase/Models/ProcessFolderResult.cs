namespace WordCounterBase.Models
{
    public class ProcessFolderResult
    {
        public List<ProcessFileResult> Files { get; set; } = [];
        public TimeSpan ElapsedTime { get; set; }
    }
}
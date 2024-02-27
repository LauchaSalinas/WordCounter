namespace WordCounterBase.Processors
{
    internal class WordProcessor
    {
        public static int CountWords(string text)
        {
            string[] words = text.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            var wordsList = words.ToList();
            if (wordsList.Contains("•"))
            {
                wordsList.RemoveAll(x => x == "•");
            }

            if (wordsList.Contains("·"))
            {
                wordsList.RemoveAll(x => x == "·");
            }

            return wordsList.Count;
        }
    }
}

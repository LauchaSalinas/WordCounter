using WordCounterBase.Models;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace WordCounterBase.Processors
{
    internal class JsonProcessor
    {
        private static string StringResult = string.Empty;
        internal static JsonProcessingResult ProcessFile(string jsonFilePath)
        {
            StringResult = string.Empty;
            var jsonString = File.ReadAllText(jsonFilePath);
            JObject jsonObject = JObject.Parse(jsonString);
            ExtractTextFromJsonObject(jsonObject);

            int wordCount = WordProcessor.CountWords(StringResult);
            return new JsonProcessingResult() { WordCount = wordCount };
        }

        private static void ExtractTextFromJsonObject(JObject jsonObject)
        {
            foreach (JObject jobj in jsonObject.Children<JObject>())
            {
                ExtractTextFromJsonObject(jobj);

            }
            foreach (JArray jarr in jsonObject.Children<JArray>())
            {
                ExtractTextFromJsonArray(jarr);

            }
            foreach (JProperty jprop in jsonObject.Children<JProperty>())
            {
                ExtractTextFromJsonProperty(jprop);

            }
        }

        private static void ExtractTextFromJsonArray(JArray jsonArray)
        {
            foreach( JObject jobj in jsonArray.Children<JObject>())
            {
                ExtractTextFromJsonObject(jobj);

            }
            foreach (JArray jarr in jsonArray.Children<JArray>())
            {
                ExtractTextFromJsonArray(jarr);

            }
            foreach (JProperty jprop in jsonArray.Children<JProperty>())
            {
                ExtractTextFromJsonProperty(jprop);

            }
        }

        private static void ExtractTextFromJsonProperty(JProperty jprop)
        {
            foreach (JObject jobj in jprop.Children<JObject>())
            {
                ExtractTextFromJsonObject(jobj);

            }
            foreach (JArray jarr in jprop.Children<JArray>())
            {
                ExtractTextFromJsonArray(jarr);

            }
            foreach (JProperty jpropChild in jprop.Children<JProperty>())
            {
                ExtractTextFromJsonProperty(jpropChild);

            }
            if (jprop.Count == 1 && (JsonProcessingConfiguration.ImportKeys.Count == 0 || JsonProcessingConfiguration.ImportKeys.Contains(jprop.Name, StringComparer.OrdinalIgnoreCase)) && jprop.Children().Children<JToken>().Count<JToken>() == 0)
                StringResult += jprop.Value.ToString() + " ";
        }
    }
}

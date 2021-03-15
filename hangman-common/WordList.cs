using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace hangman_common
{
    public interface IWordList
    {
        public string GetRandomWord();
    }

    internal class WordList : IWordList
    {
        private readonly List<string> _words = new List<string>();
        
        public WordList(string json)
        {
            JObject.Parse(json)["words"]?.ToImmutableList().ForEach(x => _words.Add(x?.ToString()));
        }
        
        public string GetRandomWord()
        {
            if (_words.Count == 0)
                return "Missing word list";
            
            var rand = new Random();
            return _words[rand.Next(0, _words.Count)];
        }
    }
    
    public static class WordListFactory
    {
        public static IWordList MakeWordList(Language lang, IEnumerable<(string, string)> dataFiles)
        {
            var (_, content) = dataFiles.FirstOrDefault(file =>
            {
                var (name, _) = file;
                return name == lang.ShortName();
            });

            if (string.IsNullOrEmpty(content)) content = "{}";
            
            return new WordList(content);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using hangman_common;
using HarmonyLib;
using Newtonsoft.Json.Linq;

namespace hangman_dynamic
{
    internal class WordList : IWordList
    {
        private readonly List<string> _words = new List<string>();
        
        public WordList(string listPath)
        {
            if (!File.Exists(listPath))
                return;
            
            var json = File.ReadAllText(listPath);
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
    
    [HarmonyPatch(typeof(WordListFactory), "MakeWordList")]
    internal class WordListFactoryPatch
    {
        private static bool Prefix(Language lang, ref IWordList __result)
        {
            __result = new WordList(Path.Combine(Loader.Data.ModDir, "data", $"{lang.ShortName()}.json"));
            return false;
        }
    }
}
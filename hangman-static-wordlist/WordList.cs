using System;
using hangman_common;
using HarmonyLib;

namespace hangman_static
{
    public class WordListFrFr: IWordList
    {
        private readonly string[] _words = {
            "Bonjour",
            "Au revoir",
            "Aimer"
        };
        public string GetRandomWord()
        {
            var rand = new Random();
            return _words[rand.Next(0, _words.Length)];
        }
    }
    public class WordListFrCa: IWordList
    {
        private readonly string[] _words = {
            "Bonjour",
            "Au revoir",
            "Aimer"
        };
        public string GetRandomWord()
        {
            var rand = new Random();
            return _words[rand.Next(0, _words.Length)];
        }
    }
    public class WordListEnGb: IWordList
    {
        private readonly string[] _words = {
            "Hello",
            "Goodbye",
            "To love"
        };
        public string GetRandomWord()
        {
            var rand = new Random();
            return _words[rand.Next(0, _words.Length)];
        }
    }
    public class WordListEnUs: IWordList
    {
        private readonly string[] _words = {
            "Hello",
            "Goodbye",
            "To love"
        };
        public string GetRandomWord()
        {
            var rand = new Random();
            return _words[rand.Next(0, _words.Length)];
        }
    }
    
    [HarmonyPatch(typeof(WordListFactory), "MakeWordList")]
    internal class WordListFactoryPatch
    {
        private static bool Prefix(Language lang, ref IWordList __result)
        {
            __result = lang switch
            {
                Language.FrFr => new WordListFrFr(),
                Language.FrCa => new WordListFrCa(),
                Language.EnGb => new WordListEnGb(),
                _ => new WordListEnUs()
            };
            return false;
        }
    }
}
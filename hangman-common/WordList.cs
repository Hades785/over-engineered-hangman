using System;

namespace hangman_common
{
    public interface IWordList
    {
        public string GetRandomWord();
    }

    internal class DefaultWordList : IWordList
    {
        private readonly string[] _words = {
            "Test",
            "Test2",
            "Test3",
            "Test4"
        };
        
        public string GetRandomWord()
        {
            var rand = new Random();
            return _words[rand.Next(0, _words.Length)];
        }
    }
    
    public static class WordListFactory
    {
        public static IWordList MakeWordList(Language lang)
        {
            return new DefaultWordList();
        }
    }
}
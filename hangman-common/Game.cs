using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using utilities;

namespace hangman_common
{
    public class Game
    {
        private const char BlankChar = '■';

        private ushort _attempts;
        private readonly string _word;
        private readonly HashSet<char> _played = new HashSet<char>();

        public bool IsOver
        {
            get
            {
                // If all characters in the word have been played (first and last excluded) or that all attempts have
                // been used, game over.
                var charSet = _word.Substring(1,_word.Length - 2).ToCharArray().ToImmutableHashSet();
                return charSet.IsSubsetOf(_played) || _attempts == 0;
            }
        }

        public string WordState
        {
            get
            {
                // Over-engineered procedure, demonstrations purpose.
                var word = new StringBuilder(_word);
                foreach (var (character, index) in _word.ToCharArray().WithIndex())
                    if (index > 0 && index < word.Length - 1 && !_played.Contains(character))
                        word[index] = BlankChar;
                return word.ToString();
            }
        }

        public Game(string word, ushort attempts)
        {
            _word = word;
            _attempts = attempts;
            
            // Prefill special characters.
            _played.Add(' ');
        }

        public (bool, string, ushort, bool) PlayChar(char playedChar)
        {
            // Add both lowercase and uppercase character, check that either got added.
            var added = _played.Add(char.ToLower(playedChar)) || _played.Add(char.ToUpper(playedChar));
            
            // If the given character was not added (previously played) or is not part of the word, set failed flag.
            var failedChar = !(added && _word.Contains(playedChar));
            
            // If the given character resulted in a failed attempt, reduce the attempts counter.
            if (failedChar) _attempts--;

            // Return a tuple containing the failed flag, the word state (with hidden characters) and the number of
            // failed attempts left before game over, and if the word state doesn't contains hidden characters (win).
            return (failedChar, WordState, _attempts, !WordState.Contains(BlankChar));
        }
    }
}
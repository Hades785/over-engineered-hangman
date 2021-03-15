using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using hangman_common;
using utilities;

namespace app_console
{
    internal static class Program
    {
        private static IEnumerable<string> GetFilesRecursive(string startingDirectory) =>
            GetFilesRecursive(startingDirectory, "*");
        
        private static IEnumerable<string> GetFilesRecursive(string startingDirectory, string searchPattern)
        {
            var files = new List<string>();
            
            files.AddRange(Directory.GetFiles(startingDirectory, searchPattern));
            foreach (var directory in Directory.GetDirectories(startingDirectory))
                files.AddRange(GetFilesRecursive(directory, searchPattern));

            return files;
        }
        
        private static IEnumerable<(string, string)> LoadData()
        {
            var dataDir = Path.Combine(Environment.CurrentDirectory, "data");
            if (!Directory.Exists(dataDir)) Directory.CreateDirectory(dataDir);
            
            var files = GetFilesRecursive(dataDir);
            var dataFiles = new List<(string, string)>();

            foreach (var file in files)
            {
                var content = File.ReadAllText(file);
                dataFiles.Add((Path.GetFileNameWithoutExtension(file), content));
            }
            
            return dataFiles;
        }
        
        private static Language ChooseLanguage()
        {
            var choice = ushort.MaxValue;
            var numLang = Enum.GetNames(typeof(Language)).Length;
            Console.WriteLine("Choose a language to pull a word from:");

            foreach (var (lang, i) in ((Language[]) Enum.GetValues(typeof(Language))).WithIndex())
            {
                Console.WriteLine($"{i}) {lang.Name()}");
            }

            while (choice > numLang)
            {
                Console.Write("Type the number next to your choice: ");
                try
                {
                    choice = ushort.Parse(Console.ReadLine() ?? string.Empty);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Please input a number from the list.");
                    choice = ushort.MaxValue;
                }
                catch (OverflowException)
                {
                    Console.WriteLine("The inputted number exceeds the unsigned 16bit integer limit, don't do that :)");
                    choice = ushort.MaxValue;
                }
            }

            var l = Enum.Parse<Language>(choice.ToString());
            Console.WriteLine($"You have chosen the {l.Name()} language.");
            return l;
        }

        private static void Main()
        {
            var playing = true;
            var lang = ChooseLanguage();
            var wordList = WordListFactory.MakeWordList(lang, LoadData());

            while (playing)
            {
                var word = wordList.GetRandomWord();
                var game = new Game(word, 10);
                
                Console.WriteLine($"Word to find: {game.WordState}");

                while (!game.IsOver)
                {
                    var attempt = string.Empty;
                    while (string.IsNullOrEmpty(attempt))
                    {
                        Console.Write("Input a character: ");
                        attempt = Console.ReadLine();
                    }

                    var (failed, wordState, attemptsLeft, win) = game.PlayChar(attempt[0]);

                    Console.WriteLine(win
                        ? $"Congratulations, you have found the word: \"{wordState}\" with {attemptsLeft} failed attempts left."
                        : $"{(failed ? "NO" : "OK")}: {wordState}. You have {attemptsLeft} failed attempts left.");
                }
                
                Console.WriteLine("Game ended.");
                Console.Write("Replay? (y/N) ");
                var replayChoice = Console.ReadLine() ?? string.Empty;
                if (!replayChoice.StartsWith("y", true, CultureInfo.InvariantCulture))
                    playing = false;
            }
        }
    }
}
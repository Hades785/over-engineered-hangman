using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using hangman_common;
using utilities;

namespace app_console
{
    internal static class Program
    {
        private static string[] GetFilesRecursive(string startingDirectory) =>
            GetFilesRecursive(startingDirectory, "*");
        
        private static string[] GetFilesRecursive(string startingDirectory, string searchPattern)
        {
            var files = new List<string>();
            
            files.AddRange(Directory.GetFiles(startingDirectory, searchPattern));
            foreach (var directory in Directory.GetDirectories(startingDirectory))
                files.AddRange(GetFilesRecursive(directory, searchPattern));

            return files.ToArray();
        }
        private static void LoadMods()
        {
            var modsDir = Path.Combine(Environment.CurrentDirectory, "mods");
            if (!Directory.Exists(modsDir))
                Directory.CreateDirectory(modsDir);
            var files = GetFilesRecursive(modsDir, "*.dll");

            foreach (var file in files)
            {
                Console.WriteLine($"Found dll: {file}");
                var asm = Assembly.LoadFrom(file);

                Type loaderType = null;
                try
                {
                    loaderType = asm.GetTypes().First(x => x.Name == "Loader");
                }
                catch (InvalidOperationException)
                {
                }

                if (loaderType == null) continue;
                
                try
                {
                    var basename = Path.GetFileNameWithoutExtension(file);
                    var modDir = Path.Combine(modsDir, modsDir, basename);
                    
                    if (!Directory.Exists(modDir))
                        Directory.CreateDirectory(modDir);
                    
                    if (!GetFilesRecursive(modDir).Contains(file))
                        File.Move(file, Path.Combine(modDir, $"{basename}.dll"));

                    var loader = Activator.CreateInstance(loaderType, asm, modDir);

                    var getModDetails = loaderType.GetMethod("GetModDetails");
                    var (modName, modVersion) = ("Unnamed Mod", "0.0.0");
                    if (loader != null)
                    {
                        var result = getModDetails?.Invoke(loader, null);
                        if (result != null)
                            (modName, modVersion) = (ValueTuple<string, string>) result;
                    }

                    Console.WriteLine($"Mod loaded: [{modName}]@{modVersion}");
                }
                catch (MissingMethodException)
                {
                }
            }
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
            LoadMods();
            
            var playing = true;
            var lang = ChooseLanguage();
            var wordList = WordListFactory.MakeWordList(lang);

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
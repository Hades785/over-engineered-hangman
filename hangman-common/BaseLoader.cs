using System.IO;
using System.Reflection;
using HarmonyLib;

namespace hangman_common
{
    public abstract class BaseLoader
    {
        protected readonly Harmony Harmony;

        public BaseLoader(Assembly asm,  string modDir)
        {
            Harmony = new Harmony($"fuzuki.hangman.{modDir.Split(Path.DirectorySeparatorChar)[^1]}");
            Harmony.PatchAll(asm);
        }
        public abstract (string, string) GetModDetails();
    }
}
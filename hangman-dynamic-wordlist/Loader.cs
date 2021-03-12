using System.IO;
using System.Reflection;
using hangman_common;

namespace hangman_dynamic
{
    public class Loader: BaseLoader
    {
        public static class Data
        {
            public static string ModDir
            {
                get;
                internal set;
            }
        }
        
        public Loader(Assembly asm, string modDir) : base(asm, modDir)
        {
            Data.ModDir = modDir;
        }

        public override (string, string) GetModDetails()
        {
            return ("Dynamic Word Lists", "0.1.0");
        }
    }
}
using System.Reflection;
using hangman_common;

namespace hangman_static
{
    public class Loader : BaseLoader
    {
        public Loader(Assembly asm, string modDir) : base(asm, modDir)
        {
        }

        public override (string, string) GetModDetails()
        {
            return ("Static Word Lists", "0.1.0");
        }
    }
}
using hangman_common;

namespace hangman_dynamic
{
    public static class LanguageExtensions
    {
        public static string ShortName(this Language lang)
        {
            var name = lang switch
            {
                Language.EnGb => "en-gb",
                Language.EnUs => "en-us",
                Language.FrCa => "fr-ca",
                Language.FrFr => "fr-fr",
                _ => "undf-lang"
            };

            return name;
        }
    }
}
namespace hangman_common
{
    public enum Language
    {
        FrFr,
        FrCa,
        EnGb,
        EnUs
    }

    public static class LanguageExtensions
    {
        public static string Name(this Language lang)
        {
            var name = lang switch
            {
                Language.EnGb => "English (GB)",
                Language.EnUs => "English (US)",
                Language.FrCa => "Français (CA)",
                Language.FrFr => "Français (FR)",
                _ => "undefined language"
            };

            return name;
        }
        
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
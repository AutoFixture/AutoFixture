using System.Collections.Generic;

namespace Fare
{
    public static class RegExpMatchingOptions
    {
        /// <summary>
        /// Uses case-insensitive matching.
        /// </summary>
        public const char IgnoreCase = 'i';

        /// <summary>
        /// Use single-line mode, where the period matches every character,
        /// instead of every character except <code>\n</code>.
        /// </summary>
        public const char Singleline = 's';

        /// <summary>
        /// Use multiline mode, where <code>^</code> and <code>$</code> match
        /// the beginning and end of each line, instead of the beginning and end of the input string.
        /// </summary>
        public const char Multiline = 'm';

        /// <summary>
        /// Do not capture unnamed groups.
        /// </summary>
        public const char ExplicitCapture = 'n';

        /// <summary>
        /// Exclude unescaped white space from the pattern
        /// and enable comments after a hash sign <code>#</code>.
        /// </summary>
        public const char IgnorePatternWhitespace = 'x';

        public static IEnumerable<char> All()
        {
            yield return IgnoreCase;
            yield return Singleline;
            yield return Multiline;
            yield return ExplicitCapture;
            yield return IgnorePatternWhitespace;
        }
    }
}

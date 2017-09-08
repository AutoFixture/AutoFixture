using System.Text.RegularExpressions;

namespace Ploeh.AutoFixtureUnitTest
{
    internal class TextGuidRegex : Regex
    {
        internal TextGuidRegex()
            : base("^(?<text>.*)(?<guid>[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12})$", RegexOptions.Compiled)
        {
        }

        internal string GetGuid(string s)
        {
            return this.Match(s).Groups["guid"].Value;
        }

        internal string GetText(string s)
        {
            return this.Match(s).Groups["text"].Value;
        }
    }
}

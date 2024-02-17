using System.Text.RegularExpressions;

namespace Nuke.Common.CI.GitHubActions
{
    public static class GitHubActionsExtensions
    {
        private static readonly Regex SemVerRef = new(@"^refs\/tags\/v(?<version>\d+\.\d+\.\d+)", RegexOptions.Compiled);

        public static bool IsOnSemVerTag(this GitHubActions source)
        {
            if (string.IsNullOrWhiteSpace(source?.Ref))
            {
                return false;
            }

            return SemVerRef.IsMatch(source.Ref);
        }
    }
}
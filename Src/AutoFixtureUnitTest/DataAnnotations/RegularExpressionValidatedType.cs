using System.ComponentModel.DataAnnotations;

namespace AutoFixtureUnitTest.DataAnnotations
{
    public class RegularExpressionValidatedType
    {
        public const string Pattern = @"^[a-zA-Z''-'\s]{20,40}$";

        [RegularExpression(Pattern)]
        public string Property { get; set; }
    }
}

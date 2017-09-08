using System.ComponentModel.DataAnnotations;

namespace Ploeh.AutoFixtureUnitTest.DataAnnotations
{
    public class RegularExpressionValidatedType
    {
        public const string Pattern = @"^[a-zA-Z''-'\s]{1,40}$";

        [RegularExpression(Pattern)]
        public string Property { get; set; }
    }
}

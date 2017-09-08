using System.ComponentModel.DataAnnotations;

namespace Ploeh.AutoFixtureUnitTest.DataAnnotations
{
    public class StringLengthValidatedType
    {
        public const int MaximumLength = 3;

        [StringLength(MaximumLength)]
        public string Property { get; set; }
    }
}